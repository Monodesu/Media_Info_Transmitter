using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Control;

namespace Media_Info_Transmitter
{
    public partial class Udebug : Form
    {
        private CancellationTokenSource? _cancellationTokenSource;

        public Udebug()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(debug_FormClosing!);
        }

        private void debug_Load(object sender, EventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => BackgroundUpdateTask(_cancellationTokenSource.Token));
        }

        private void debug_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

        private async Task BackgroundUpdateTask(CancellationToken token)
        {
            var mediaManager = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    listBox1.Invoke(new Action(() => listBox1.Items.Clear()));

                    var currentSession = mediaManager.GetCurrentSession();
                    if (currentSession == null)
                    {
                        listBox1.Invoke(new Action(() => listBox1.Items.Add("No playing media detected.")  ));
                        await Task.Delay(1000, token);
                        continue;
                    }     

                    var playbackInfo = currentSession.GetPlaybackInfo();
                    var mediaProperties = await currentSession.TryGetMediaPropertiesAsync();

                    var timeLine = currentSession.GetTimelineProperties();

                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{mediaProperties!.Title}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{mediaProperties!.Subtitle}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{mediaProperties!.Artist}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{mediaProperties!.TrackNumber}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{mediaProperties!.AlbumTrackCount}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{mediaProperties!.AlbumTitle}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{mediaProperties!.AlbumArtist}")));
                    
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{playbackInfo!.PlaybackType}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{playbackInfo!.IsShuffleActive}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{playbackInfo!.AutoRepeatMode}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{playbackInfo!.PlaybackRate}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{playbackInfo!.PlaybackStatus}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{timeLine.StartTime}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{timeLine.Position}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{timeLine.EndTime}")));
                    listBox1.Invoke(new Action(() => listBox1.Items.Add($"{timeLine.LastUpdatedTime}")));
                }
                catch
                {
                    // do nothing
                }
                await Task.Delay(1000, token);
            }
        }

    }
}
