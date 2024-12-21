using DiscordRPC;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using Windows.Media.Control;
using Windows.Storage.Streams;

namespace Media_Info_To_VRChat_Discord
{
    public partial class Form1 : Form
    {
        private AppConfig config = new();

        public AppConfig GlobalConfig
        {
            get { return config; }
            set { config = value; }
        }

        private bool isSettingFormOpen = false;

        private enum MediaType
        {
            Video,
            Audio
        }

        private CancellationTokenSource? _cancellationTokenSource;
        private readonly IPEndPoint _oscEndpoint = new(IPAddress.Parse("127.0.0.1"), 9000);
        private static UdpClient? _vrchatUdpClient;
        private static DiscordRpcClient? _discordClient;
        private static bool discordTimestampsInited = false;
        private static Timestamps? startTime;
        private int VRC_OSC_Delay_Counter = 0;
        private int Discord_Delay_Counter = 0;
        private string title_temp = "";
        private bool needUpdateThumbnail = true;
        private bool needUpdateGUI = true;
        private bool needUpdateIdleInfo = false;
        private bool needUpdateExportedInfo = false;
        private MediaType CurrentMediaType = MediaType.Audio;

        private bool isThumbnailExportTaskRunning = false;
        private bool isMediaInfoExportTaskRunning = false;

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public Form1(bool isRunningWithSystem)
        {
            InitializeComponent();
            _vrchatUdpClient = new UdpClient();
            _discordClient = null;

            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Exit", null, ExitApp!);
            trayIcon = new NotifyIcon
            {
                Text = "Media Info Transmitter",
                Icon = this.Icon,
                ContextMenuStrip = trayMenu,
                Visible = true
            };
            trayIcon.MouseDoubleClick += new MouseEventHandler(TrayIcon_MouseDoubleClick!);
            this.Resize += new EventHandler(Form1_Resize!);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing!);
            this.Shown += (sender, e) => Form1_Shown(sender!, e, isRunningWithSystem);
        }

        private void Form1_Shown(object sender, EventArgs e, bool isRunningWithSystem)
        {
            if (isRunningWithSystem) Hide();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void ExitApp(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            System.Windows.Forms.Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                if (!config.HideMinimizedNotification)
                {
                    trayIcon.ShowBalloonTip(1000, "Media Info Transmitter", "Just Notice, the app is still running in the background.", ToolTipIcon.Info);
                }
            }
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartBackgroundTask();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            _vrchatUdpClient?.Close();
            _discordClient?.Dispose();
        }

        private void StartBackgroundTask()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => BackgroundUpdateTask(_cancellationTokenSource.Token));
        }

        private async Task BackgroundUpdateTask(CancellationToken token)
        {
            config = ConfigManager.LoadConfig();
            Image thumbnailImage = new Bitmap(100, 100);
            var mediaManager = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult();

            while (!token.IsCancellationRequested)
            {

                try
                {
                    var currentSession = mediaManager.GetCurrentSession();
                    if (currentSession == null)
                    {
                        title_label.Invoke(new Action(() => title_label.Text = "No playing media detected."));
                        if (needUpdateIdleInfo)
                        {
                            SendOSCMessage(BuildMediaInfoMsgForVRC(null, null, null));
                            if (_discordClient is not null) _discordClient!.ClearPresence();
                            needUpdateIdleInfo = false;
                        }
                        await Task.Delay(2000, token);
                        continue;
                    }

                    needUpdateIdleInfo = true;

                    var playbackInfo = currentSession.GetPlaybackInfo();
                    var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();



                    if (mediaProperties!.Artist == "" && mediaProperties!.AlbumTitle == "")
                    {
                        if (CurrentMediaType != MediaType.Video)
                        {
                            if (_discordClient is not null)
                            {
                                _discordClient!.Dispose();
                            }
                            CurrentMediaType = MediaType.Video;
                        }
                        if (config.IgnoreSongsWithoutBothArtistAndAlbum)
                        {
                            if (Visible)
                            {
                                title_label.Invoke(new Action(() => title_label.Text = "No playing media detected."));
                                media_title.Invoke(new Action(() => media_title.Text = $""));
                                media_title.Invoke(new Action(() => media_artist.Text = $""));
                                media_title.Invoke(new Action(() => media_album.Text = $""));
                                statusStrip1.Invoke(new MethodInvoker(delegate { PlayStatusLabel.Text = $"Null"; }));
                                statusStrip1.Invoke(new MethodInvoker(delegate { playtime_label.Text = $"Null"; }));
                            }

                            await Task.Delay(2000, token);
                            continue;
                        }
                    }
                    else
                    {
                        if (CurrentMediaType != MediaType.Audio)
                        {
                            if (_discordClient is not null)
                            {
                                _discordClient!.Dispose();
                            }
                            CurrentMediaType = MediaType.Audio;
                        }
                    }

                    Init_discord_RPC(CurrentMediaType);

                    var timeLine = currentSession.GetTimelineProperties();
                    var time_string = $"{FormatTime(timeLine.Position)}/{FormatTime(timeLine.EndTime)}";

                    if (title_temp != mediaProperties.Title)
                    {
                        title_temp = mediaProperties.Title;
                        needUpdateThumbnail = true;
                        needUpdateGUI = true;
                    }

                    if (needUpdateThumbnail)
                    {
                        needUpdateThumbnail = false;
                        try
                        {
                            using IRandomAccessStreamWithContentType stream = await mediaProperties.Thumbnail.OpenReadAsync();
                            using Stream netStream = stream.AsStreamForRead();
                            thumbnailImage.Dispose();
                            thumbnailImage = Image.FromStream(netStream);
                            if (thumbnailImage is null) throw new Exception();
                        }
                        catch
                        {
                            thumbnailImage = new Bitmap(100, 100);
                        }

                        if (config.EnableThumbnailExport)
                        {
                            CheckExportFolder();
                            _ = Task.Run(() =>
                            {
                                if (isThumbnailExportTaskRunning) return;
                                isThumbnailExportTaskRunning = true;
                                using Bitmap bitmapCopy = new(thumbnailImage);
                                bitmapCopy.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export\\thumbnail.png"), System.Drawing.Imaging.ImageFormat.Png);
                                isThumbnailExportTaskRunning = false;
                            }, token);
                        }
                    }

                    // export info
                    if (config.EnableInformationExport)
                    {
                        if (playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                        {
                            needUpdateExportedInfo = true;
                        }
                        else
                        {
                            needUpdateExportedInfo = false;
                        }

                        if (needUpdateExportedInfo)
                        {
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    if (isMediaInfoExportTaskRunning) return;
                                    isMediaInfoExportTaskRunning = true;
                                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export\\info.txt");
                                    using FileStream fs = new(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                                    using StreamWriter writer = new(fs);
                                    await writer.WriteAsync(BuildMediaInfoMsgForExport(mediaProperties, playbackInfo, timeLine));

                                    isMediaInfoExportTaskRunning = false;
                                }
                                catch (IOException)
                                {
                                    // file is already opened
                                }
                            }, token);
                        }
                    }

                    if (this.Visible && needUpdateGUI)
                    {
                        needUpdateGUI = false;
                        try
                        {
                            title_label.Invoke(new Action(() => title_label.Text = "Some information of the media:"));
                            media_title.Invoke(new Action(() => media_title.Text = AdjustLabelText($"Title: {mediaProperties.Title}")));
                            if (config.IgnoreSongsWithoutBothArtistAndAlbum)
                            {
                                media_title.Invoke(new Action(() => media_artist.Text = $""));
                                media_title.Invoke(new Action(() => media_album.Text = $""));
                            }
                            else
                            {
                                media_title.Invoke(new Action(() => media_artist.Text = AdjustLabelText($"Artist: {mediaProperties.Artist}")));
                                media_title.Invoke(new Action(() => media_album.Text = AdjustLabelText($"Album: {mediaProperties.AlbumTitle}")));
                            }
                            ThumbnailBox1.Image = thumbnailImage;
                        }
                        catch { }
                    }

                    if (PlayStatusLabel.Text != $"{playbackInfo.PlaybackStatus}")
                    {
                        statusStrip1.Invoke(new MethodInvoker(delegate { PlayStatusLabel.Text = $"{playbackInfo.PlaybackStatus}"; }));
                    }

                    if (playtime_label.Text != $"{time_string}")
                    {
                        statusStrip1.Invoke(new MethodInvoker(delegate { playtime_label.Text = $"{time_string}"; }));
                    }

                    if (config.EnableDiscord_Transfer)
                    {
                        if (Discord_Delay_Counter == 2)
                        {
                            Discord_Delay_Counter = 1;
                            SetMediaInfoMsgForDiscord(mediaProperties, playbackInfo, timeLine);
                        }
                        else
                        {
                            Discord_Delay_Counter++;
                        }

                    }

                    if (config.EnableVRC_Transfer)
                    {
                        if (VRC_OSC_Delay_Counter == config.VRC_OSC_Refresh_Delay * 4)
                        {
                            VRC_OSC_Delay_Counter = 1;
                            SendOSCMessage(BuildMediaInfoMsgForVRC(mediaProperties, playbackInfo, timeLine));
                        }
                        else
                        {
                            VRC_OSC_Delay_Counter++;
                        }
                    }
                }
                catch { }
                await Task.Delay(250, token);
            }
        }

        private string BuildMediaInfoMsgForVRC(GlobalSystemMediaTransportControlsSessionMediaProperties? mediaProperties, GlobalSystemMediaTransportControlsSessionPlaybackInfo? playbackinfo, GlobalSystemMediaTransportControlsSessionTimelineProperties? timeline)
        {
            if (mediaProperties is null || timeline is null)
            {
                return "No media playing.";
            }
            var rtn_msg =
                config.VRC_Msg_Format!
                    .Replace("{media.Title}", mediaProperties!.Title)
                    .Replace("{media.Artist}", mediaProperties!.Artist)
                    .Replace("{media.CurrentPosition}", FormatTime(timeline!.Position))
                    .Replace("{media.EndTime}", FormatTime(timeline!.EndTime))
                    .Replace("{media.StartTime}", FormatTime(timeline!.StartTime))
                    .Replace("{media.AlbumTitle}", mediaProperties!.AlbumTitle)
                    .Replace("{media.Subtitle}", mediaProperties!.Subtitle)
                    .Replace("{media.PlaybackStatus}", $"{playbackinfo!.PlaybackStatus}")
                    .Replace("{media.TrackNumber}", $"{mediaProperties!.TrackNumber}"
                    );
            return rtn_msg;
        }

        private string BuildMediaInfoMsgForExport(GlobalSystemMediaTransportControlsSessionMediaProperties? mediaProperties, GlobalSystemMediaTransportControlsSessionPlaybackInfo? playbackinfo, GlobalSystemMediaTransportControlsSessionTimelineProperties? timeline)
        {
            if (mediaProperties is null || timeline is null)
            {
                return "No media playing.";
            }
            var rtn_msg =
                config.Export_Msg_Format!
                    .Replace("{media.Title}", mediaProperties!.Title)
                    .Replace("{media.Artist}", mediaProperties!.Artist)
                    .Replace("{media.CurrentPosition}", FormatTime(timeline!.Position))
                    .Replace("{media.EndTime}", FormatTime(timeline!.EndTime))
                    .Replace("{media.StartTime}", FormatTime(timeline!.StartTime))
                    .Replace("{media.AlbumTitle}", mediaProperties!.AlbumTitle)
                    .Replace("{media.Subtitle}", mediaProperties!.Subtitle)
                    .Replace("{media.PlaybackStatus}", $"{playbackinfo!.PlaybackStatus}")
                    .Replace("{media.TrackNumber}", $"{mediaProperties!.TrackNumber}"
                    );
            return rtn_msg;
        }

        private void SetMediaInfoMsgForDiscord(GlobalSystemMediaTransportControlsSessionMediaProperties? mediaProperties, GlobalSystemMediaTransportControlsSessionPlaybackInfo? playbackinfo, GlobalSystemMediaTransportControlsSessionTimelineProperties? timeline)
        {
            if (_discordClient is null || _discordClient.IsDisposed)
            {
                return;
            }

            if (!discordTimestampsInited)
            {
                discordTimestampsInited = true;
                startTime = Timestamps.Now;
            }

            string state, details;

            if (playbackinfo!.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped ||
               playbackinfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed ||
               playbackinfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Opened)
            {
                state = "Idling";
                details = "";
            }
            else
            {
                if (CurrentMediaType == MediaType.Audio)
                {
                    state = timeline is null ? "0:00 / 0:00" : $"{playbackinfo.PlaybackStatus}: {FormatTime(timeline.Position)} / {FormatTime(timeline.EndTime)}";
                    details = $"{mediaProperties!.Artist} - {mediaProperties!.Title}";
                }
                else
                {
                    state = "Watching video";
                    details = $"{mediaProperties!.Title}";
                }

            }

            _discordClient!.SetPresence(new RichPresence()
            {
                State = state,
                Details = details,
                Timestamps = startTime,
                Assets = new Assets()
                {
                    LargeImageKey = CurrentMediaType == MediaType.Audio ? config.Discord_App_Music_Icon_URL : config.Discord_App_Video_Icon_URL,
                    LargeImageText = state
                    // SmallImageKey = "Play"
                    // SmallImageText = "Now playing"
                }
            });
        }

        public static string FormatTime(TimeSpan time)
        {
            int minutes = time.Minutes;
            int seconds = time.Seconds;

            string formattedMinutes = minutes.ToString().PadLeft(1, '0');
            string formattedSeconds = seconds.ToString().PadLeft(2, '0');

            return $"{formattedMinutes}:{formattedSeconds}";
        }

        private void SendOSCMessage(string songInfo)
        {
            byte[] oscMessage = BuildOscMessage("/chatbox/input", songInfo, true);
            _vrchatUdpClient!.Send(oscMessage, oscMessage.Length, _oscEndpoint);
        }

        private static byte[] BuildOscMessage(string address, string text, bool show)
        {
            using var stream = new System.IO.MemoryStream();
            void WritePaddedString(string str)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                stream.Write(bytes, 0, bytes.Length);
                stream.Write(new byte[4 - (bytes.Length % 4)], 0, 4 - (bytes.Length % 4));
            }

            WritePaddedString(address);
            WritePaddedString(",sT");
            WritePaddedString(text);
            stream.WriteByte((byte)(show ? 1 : 0));

            return stream.ToArray();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!isSettingFormOpen)
            {
                settingsForm settingForm = new(this);
                settingForm.FormClosed += SettingForm_FormClosed!;
                settingForm.Show();
                isSettingFormOpen = true;
            }
        }

        private void SettingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConfigManager.SaveConfig(config);

            // check run with system
            // for all users: %PROGRAMDATA%\Microsoft\Windows\Start Menu\Programs\Startup
            string startupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\Start Menu\Programs\Startup");
            string shortcutPath = Path.Combine(startupFolder, "Media info transmitter.lnk");
            if (config.StartWithSystem)
            {
                string arguments = "runningwithsystem";
                CreateShortcut(shortcutPath, Application.ExecutablePath, arguments);
            }
            else
            {
                File.Delete(shortcutPath);
            }

            if (!config.EnableDiscord_Transfer)
            {
                if (_discordClient is not null) _discordClient!.Dispose();
                discordTimestampsInited = false;
            }

            isSettingFormOpen = false;
        }

        public static void CreateShortcut(string shortcutPath, string targetPath, string arguments)
        {
            var shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell")!)! as dynamic;
            var shortcut = shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.Arguments = arguments;
            shortcut.Save();
        }

        private string AdjustLabelText(string fullText, int maxWidth = 340)
        {
            using Bitmap tempBitmap = new(1, 1);
            using Graphics g = Graphics.FromImage(tempBitmap);
            SizeF fullTextSize = g.MeasureString(fullText, title_label.Font);

            if (fullTextSize.Width <= maxWidth)
            {
                return fullText;
            }

            string truncatedText = fullText;
            while (truncatedText.Length > 0)
            {
                truncatedText = truncatedText.Substring(0, truncatedText.Length - 1);
                string tempText = truncatedText + "...";

                SizeF tempSize = g.MeasureString(tempText, title_label.Font);
                if (tempSize.Width <= maxWidth)
                {
                    return tempText;
                }
            }

            return "...";
        }

        private void Init_discord_RPC(MediaType mediatype)
        {
            if (_discordClient is null || _discordClient.IsDisposed)
            {
                Init_discord_Client(mediatype);
            }
        }

        private void Init_discord_Client(MediaType mediatype)
        {
            if (mediatype == MediaType.Video)
            {
                _discordClient = new DiscordRpcClient(config.Discord_App_Video_ID);
                _discordClient.Initialize();
            }
            else
            {
                _discordClient = new DiscordRpcClient(config.Discord_App_Music_ID);
                _discordClient.Initialize();
            }
        }

        private void CheckExportFolder()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export"));
            }
        }
    }

    public class AppConfig
    {
        public AppConfig()
        {
            StartWithSystem = false;
            EnableVRC_Transfer = false;
            EnableDiscord_Transfer = false;
            HideMinimizedNotification = false;
            VRC_OSC_Refresh_Delay = 5;
            IgnoreSongsWithoutBothArtistAndAlbum = false;
            EnableInformationExport = false;
            EnableThumbnailExport = false;
            // start with mediaProperties. end with propertie name
            VRC_Msg_Format =
                             "Now Playing: {media.Title}\n" +
                             "Artist: {media.Artist}\n" +
                             "Playtime: {media.CurrentPosition}/{media.EndTime}\n";
            Export_Msg_Format =
                             "Now Playing: {media.Title}\n" +
                             "Artist: {media.Artist}\n" +
                             "Playtime: {media.CurrentPosition}/{media.EndTime}\n";

            Discord_App_Music_ID = "0";
            Discord_App_Video_ID = "0";
            Discord_App_Music_Icon_URL = "https://assets.desu.life/Media_Info_Transmitter/Images/Music_Icon.png";
            Discord_App_Video_Icon_URL = "https://assets.desu.life/Media_Info_Transmitter/Images/Video_Icon.png";
        }
        public bool StartWithSystem { get; set; }
        public bool EnableVRC_Transfer { get; set; }
        public bool EnableDiscord_Transfer { get; set; }

        public int VRC_OSC_Refresh_Delay { get; set; }
        public bool HideMinimizedNotification { get; set; }
        public bool IgnoreSongsWithoutBothArtistAndAlbum { get; set; }
        public bool EnableInformationExport { get; set; }
        public bool EnableThumbnailExport { get; set; }

        public string? VRC_Msg_Format { get; set; }
        public string? Export_Msg_Format { get; set; }

        public string Discord_App_Music_ID { get; set; }
        public string Discord_App_Video_ID { get; set; }

        public string Discord_App_Music_Icon_URL { get; set; }
        public string Discord_App_Video_Icon_URL { get; set; }
    }


    public class ConfigManager()
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "media_info_transmitter_config.json");

        public static AppConfig LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                string jsonString = File.ReadAllText(ConfigFilePath);
                return JsonSerializer.Deserialize<AppConfig>(jsonString)!;
            }
            else
            {
                var rtn = new AppConfig();
                SaveConfig(rtn);
                return rtn;
            }
        }

        public static void SaveConfig(AppConfig config)
        {
            string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFilePath, jsonString);
        }
    }
}

