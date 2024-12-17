using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            VRC_OSC_REFRESH_INPUT_BOX.Text = form1Instance.GlobalConfig.VRC_OSC_Refresh_Delay.ToString();

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
                            Currently supported:

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
                            """
                            , "help", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }

        private void checkBox_hideNotify_CheckedChanged(object sender, EventArgs e)
        {
            form1Instance!.GlobalConfig.HideMinimizedNotification = checkBox_hideNotify.Checked;
        }
    }
}
