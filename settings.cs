using Media_Info_Transmitter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Media_Info_To_VRChat_Discord
{
    public partial class settingsForm : Form
    {
        private Form1? form1Instance;

        public settingsForm(Form1 form1)
        {
            InitializeComponent();
            form1Instance = form1;
        }

        public settingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (form1Instance == null)
            {
                Close();
            }

            textBox1.Text = form1Instance!.GlobalConfig.VRC_Msg_Format!.Replace("\n", "[n]");
            textBox2.Text = form1Instance!.GlobalConfig.Export_Msg_Format!.Replace("\n", "[n]");

            textBox_musicAppID.Text = form1Instance!.GlobalConfig.Discord_App_Music_ID;
            textBox_musicIconAddr.Text = form1Instance!.GlobalConfig.Discord_App_Music_Icon_URL;
            textBox_videoAppID.Text = form1Instance!.GlobalConfig.Discord_App_Video_ID;
            textBox_videoIconAddr.Text = form1Instance!.GlobalConfig.Discord_App_Video_Icon_URL;

            VRC_OSC_REFRESH_INPUT_BOX.Text = form1Instance.GlobalConfig.VRC_OSC_Refresh_Delay.ToString();


            if (form1Instance.GlobalConfig.EnableInformationExport)
            {
                checkBox_informationExport.Checked = true;
            }
            else
            {
                checkBox_informationExport.Checked = false;
            }

            if (form1Instance.GlobalConfig.EnableThumbnailExport)
            {
                checkBox_ThumbnailExport.Checked = true;
            }
            else
            {
                checkBox_ThumbnailExport.Checked = false;
            }
            if (form1Instance.GlobalConfig.IgnoreSongsWithoutAlbum)
            {
                radioButton_IgnoreAction1.Checked = true;
            }
            else
            {
                radioButton_SwitchToVideoAction1.Checked = true;
            }
            if (form1Instance.GlobalConfig.HideMinimizedNotification)
            {
                checkBox_hideNotify.Checked = true;
            }
            else
            {
                checkBox_hideNotify.Checked = false;
            }
            if (form1Instance.GlobalConfig.StartWithSystem)
            {
                checkBox_startWithSystem.Checked = true;
            }
            else
            {
                checkBox_startWithSystem.Checked = false;
            }

            if (form1Instance.GlobalConfig.EnableVRC_Transfer)
            {
                checkBox_enableVRC.Checked = true;
            }
            else
            {
                checkBox_enableVRC.Checked = false;
            }

            if (form1Instance.GlobalConfig.EnableDiscord_Transfer)
            {
                checkBox_enableDiscord.Checked = true;
            }
            else
            {
                checkBox_enableDiscord.Checked = false;
            }
        }

        private void checkBox_startWithSystem_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.StartWithSystem = checkBox_startWithSystem.Checked;
        }

        private void checkBox_enableVRC_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.EnableVRC_Transfer = checkBox_enableVRC.Checked;
        }

        private void checkBox_enableDiscord_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.EnableDiscord_Transfer = checkBox_enableDiscord.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.VRC_Msg_Format = textBox1.Text.Replace("[n]", "\n");
        }

        private void VRC_OSC_REFRESH_INPUT_BOX_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(VRC_OSC_REFRESH_INPUT_BOX.Text, out int result))
            {
                if (result < 0 || result > 10)
                {
                    VRC_OSC_REFRESH_INPUT_BOX.Text = "5";
                }
            }
            else
            {
                VRC_OSC_REFRESH_INPUT_BOX.Text = "5";
            }

            VRC_OSC_REFRESH_INPUT_BOX.SelectionStart = VRC_OSC_REFRESH_INPUT_BOX.Text.Length;

            form1Instance!.GlobalConfig.VRC_OSC_Refresh_Delay = result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                            $$"""
                            For format, currently supported:

                            {media.Title}
                            {media.Subtitle}
                            {media.TrackNumber}
                            {media.Artist}
                            {media.AlbumTitle}
                            {media.StartTime}
                            {media.CurrentPosition}
                            {media.EndTime}
                            {media.PlaybackStatus}

                            For a new line, insert [n] at the end.

                            Regarding Discord's App ID for RPC
                            you need to create two apps separately at
                            https://discord.com/developers/applications
                            and fill in the app ID into the input box so 
                            that the Discord RPC can work properly.

                            github:
                            https://github.com/Monodesu/Media_Info_Transmitter
                            """
                            , "help", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }

        private void checkBox_hideNotify_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.HideMinimizedNotification = checkBox_hideNotify.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Udebug debugForm = new();
            debugForm.Show();
        }

        private void radioButton_IgnoreAction1_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.IgnoreSongsWithoutAlbum = true;
        }

        private void radioButton_SwitchToVideoAction1_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.IgnoreSongsWithoutAlbum = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.Export_Msg_Format = textBox2.Text.Replace("[n]", "\n");
        }

        private void checkBox_informationExport_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.EnableInformationExport = checkBox_informationExport.Checked;
        }

        private void checkBox_ThumbnailExport_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.EnableThumbnailExport = checkBox_ThumbnailExport.Checked;
        }

        private void textBox_musicIconAddr_TextChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.Discord_App_Music_Icon_URL = textBox_musicIconAddr.Text;
        }

        private void textBox_videoIconAddr_TextChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.Discord_App_Video_Icon_URL = textBox_videoIconAddr.Text;
        }

        private void textBox_musicAppKey_TextChanged(object sender, EventArgs e)
        {
            if (textBox_musicAppID.Text.Length < 1) textBox_musicAppID.Text = "0";
            form1Instance!.GlobalConfig.Discord_App_Music_ID = textBox_musicAppID.Text;
        }

        private void textBox_videoAppKey_TextChanged(object sender, EventArgs e)
        {
            if (textBox_videoAppID.Text.Length < 1) textBox_videoAppID.Text = "0";
            form1Instance!.GlobalConfig.Discord_App_Video_ID = textBox_videoAppID.Text;
        }
    }
}
