using DiscordRPC;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Windows.Media.Control;

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
        private bool needUpdate = false;

        private CancellationTokenSource? _cancellationTokenSource;
        private readonly IPEndPoint _oscEndpoint = new(IPAddress.Parse("127.0.0.1"), 9000);
        private static UdpClient? _vrchatUdpClient;
        private static DiscordRpcClient? _discordClient;
        private static bool discordTimestampsInited = false;
        private static Timestamps? startTime;
        private int VRC_OSC_Delay_Counter = 0;
        private int Discord_Delay_Counter = 0;

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public Form1(bool isRunningWithSystem)
        {
            InitializeComponent();
            _vrchatUdpClient = new UdpClient();
            _discordClient = new DiscordRpcClient("1318495466085941259"); // you can replace this to your own
            _discordClient.Initialize();

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
        }

        private void StartBackgroundTask()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => BackgroundUpdateTask(_cancellationTokenSource.Token));
        }

        private async Task BackgroundUpdateTask(CancellationToken token)
        {
            // get config
            config = ConfigManager.LoadConfig();

            var mediaManager = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var currentSession = mediaManager.GetCurrentSession();
                    if (currentSession == null)
                    {
                        title_label.Invoke(new Action(() => title_label.Text = "No playing media detected."));
                        if (needUpdate)
                        {
                            SendOSCMessage(BuildSongInfoMsgForVRC(null, null, null));
                            _discordClient!.ClearPresence();
                            needUpdate = false;
                        }
                        await Task.Delay(2000, token);
                        continue;
                    }

                    needUpdate = true;

                    var playbackInfo = currentSession.GetPlaybackInfo();
                    var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();

                    if (config.IgnoreSongsWithoutBothArtistAndAlbum)
                    {
                        if (mediaProperties!.Artist == "" && mediaProperties!.AlbumTitle == "")
                        {
                            title_label.Invoke(new Action(() => title_label.Text = "No playing media detected."));
                            media_title.Invoke(new Action(() => media_title.Text = $""));
                            media_title.Invoke(new Action(() => media_artist.Text = $""));
                            media_title.Invoke(new Action(() => media_album.Text = $""));
                            statusStrip1.Invoke(new MethodInvoker(delegate { PlayStatusLabel.Text = $"Null"; }));
                            statusStrip1.Invoke(new MethodInvoker(delegate { playtime_label.Text = $"Null"; }));
                            await Task.Delay(2000, token);
                            continue;
                        }
                    }

                    var timeLine = currentSession.GetTimelineProperties();
                    var time_string = $"{FormatTime(timeLine.Position)}/{FormatTime(timeLine.EndTime)}";

                    title_label.Invoke(new Action(() => title_label.Text = "Some information of the media:"));
                    media_title.Invoke(new Action(() => media_title.Text = $"Title: {mediaProperties.Title}"));
                    media_title.Invoke(new Action(() => media_artist.Text = $"Artist: {mediaProperties.Artist}"));
                    media_title.Invoke(new Action(() => media_album.Text = $"Album: {mediaProperties.AlbumTitle}"));
                    statusStrip1.Invoke(new MethodInvoker(delegate { PlayStatusLabel.Text = $"{playbackInfo.PlaybackStatus}"; }));
                    statusStrip1.Invoke(new MethodInvoker(delegate { playtime_label.Text = $"{time_string}"; }));


                    if (config.EnableDiscord_Transfer)
                    {
                        if (Discord_Delay_Counter == 2)
                        {
                            Discord_Delay_Counter = 1;
                            BuildSongInfoMsgForDiscord(mediaProperties, playbackInfo, timeLine);
                        }
                        else
                        {
                            Discord_Delay_Counter++;
                        }

                    }

                    if (config.EnableVRC_Transfer)
                    {
                        if (VRC_OSC_Delay_Counter == config.VRC_OSC_Refresh_Delay * 2)
                        {
                            VRC_OSC_Delay_Counter = 1;
                            SendOSCMessage(BuildSongInfoMsgForVRC(mediaProperties, playbackInfo, timeLine));
                        }
                        else
                        {
                            VRC_OSC_Delay_Counter++;
                        }
                    }
                }
                catch
                {
                    // do nothing
                }
                await Task.Delay(500, token);
            }
        }

        private string BuildSongInfoMsgForVRC(GlobalSystemMediaTransportControlsSessionMediaProperties? mediaProperties, GlobalSystemMediaTransportControlsSessionPlaybackInfo? playbackinfo, GlobalSystemMediaTransportControlsSessionTimelineProperties? timeline)
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
                    .Replace("{media.TrackNumber}", mediaProperties!.TrackNumber.ToString()
                    .Replace("{media.PlaybackStatus}", $"{playbackinfo!.PlaybackStatus}")
                    );
            return rtn_msg;
        }

        private static void BuildSongInfoMsgForDiscord(GlobalSystemMediaTransportControlsSessionMediaProperties? mediaProperties, GlobalSystemMediaTransportControlsSessionPlaybackInfo? playbackinfo, GlobalSystemMediaTransportControlsSessionTimelineProperties? timeline)
        {
            if (!discordTimestampsInited)
            {
                discordTimestampsInited = true;
                startTime = Timestamps.Now;
            }

            string state, details;

            if (playbackinfo!.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped ||
               playbackinfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Closed)
            {
                state = "Idling";
            }
            else
            {
                state = timeline is null ? "0:00 / 0:00" : $"{playbackinfo.PlaybackStatus}: {FormatTime(timeline.Position)} / {FormatTime(timeline.EndTime)}";
            }

            details = $"{mediaProperties!.Artist} - {mediaProperties!.Title}";

            _discordClient!.SetPresence(new RichPresence()
            {
                State = state,
                Details = details,
                Timestamps = startTime,
                Assets = new Assets()
                {
                    LargeImageKey = "https://assets.desu.life/discord_bot/12f4f041-be3a-4d50-abed-3174cc8d05a8.png",
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
                _discordClient!.ClearPresence();
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

            // start with mediaProperties. end with propertie name
            VRC_Msg_Format =
                             "Now Playing: {media.Title}\n" +
                             "Artist: {media.Artist}\n" +
                             "Playtime: {media.CurrentPosition}/{media.EndTime}\n";
        }
        public bool StartWithSystem { get; set; }
        public bool EnableVRC_Transfer { get; set; }
        public bool EnableDiscord_Transfer { get; set; }
        public string? VRC_Msg_Format { get; set; }
        public int VRC_OSC_Refresh_Delay { get; set; }
        public bool HideMinimizedNotification { get; set; }
        public bool IgnoreSongsWithoutBothArtistAndAlbum { get; set; }
    }


    public class ConfigManager()
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "media_info_transfer_config.json");

        public static AppConfig LoadConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                string jsonString = File.ReadAllText(ConfigFilePath);
                return JsonSerializer.Deserialize<AppConfig>(jsonString)!;
            }
            else
            {
                var rtn = new AppConfig()
                {
                    StartWithSystem = false,
                    EnableVRC_Transfer = false,
                    EnableDiscord_Transfer = false,
                    VRC_OSC_Refresh_Delay = 5,
                    HideMinimizedNotification = false,
                    IgnoreSongsWithoutBothArtistAndAlbum = false,
                    VRC_Msg_Format =
                        "Now Playing: {media.Title}\n" +
                        "Artist: {media.Artist}\n" +
                        "Playtime: {media.CurrentPosition}/{media.EndTime}\n"
                };
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

