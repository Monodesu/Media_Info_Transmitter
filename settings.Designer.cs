namespace Media_Info_To_VRChat_Discord
{
    partial class settingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            checkBox_startWithSystem = new CheckBox();
            checkBox_enableVRC = new CheckBox();
            checkBox_enableDiscord = new CheckBox();
            textBox1 = new TextBox();
            label1 = new Label();
            VRC_OSC_REFRESH_INPUT_BOX = new TextBox();
            label2 = new Label();
            button1 = new Button();
            checkBox_hideNotify = new CheckBox();
            SuspendLayout();
            // 
            // checkBox_startWithSystem
            // 
            checkBox_startWithSystem.AutoSize = true;
            checkBox_startWithSystem.Location = new Point(12, 12);
            checkBox_startWithSystem.Name = "checkBox_startWithSystem";
            checkBox_startWithSystem.Size = new Size(115, 19);
            checkBox_startWithSystem.TabIndex = 0;
            checkBox_startWithSystem.Text = "Start with system";
            checkBox_startWithSystem.UseVisualStyleBackColor = true;
            checkBox_startWithSystem.CheckedChanged += checkBox_startWithSystem_CheckedChanged;
            // 
            // checkBox_enableVRC
            // 
            checkBox_enableVRC.AutoSize = true;
            checkBox_enableVRC.Location = new Point(12, 37);
            checkBox_enableVRC.Name = "checkBox_enableVRC";
            checkBox_enableVRC.Size = new Size(85, 19);
            checkBox_enableVRC.TabIndex = 1;
            checkBox_enableVRC.Text = "Enable VRC";
            checkBox_enableVRC.UseVisualStyleBackColor = true;
            checkBox_enableVRC.CheckedChanged += checkBox_enableVRC_CheckedChanged;
            // 
            // checkBox_enableDiscord
            // 
            checkBox_enableDiscord.AutoSize = true;
            checkBox_enableDiscord.Location = new Point(12, 62);
            checkBox_enableDiscord.Name = "checkBox_enableDiscord";
            checkBox_enableDiscord.Size = new Size(104, 19);
            checkBox_enableDiscord.TabIndex = 2;
            checkBox_enableDiscord.Text = "Enable Discord";
            checkBox_enableDiscord.UseVisualStyleBackColor = true;
            checkBox_enableDiscord.CheckedChanged += checkBox_enableDiscord_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 170);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(339, 133);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 152);
            label1.Name = "label1";
            label1.Size = new Size(162, 15);
            label1.TabIndex = 4;
            label1.Text = "VRChat OSC Message Format:";
            // 
            // VRC_OSC_REFRESH_INPUT_BOX
            // 
            VRC_OSC_REFRESH_INPUT_BOX.Location = new Point(192, 115);
            VRC_OSC_REFRESH_INPUT_BOX.Name = "VRC_OSC_REFRESH_INPUT_BOX";
            VRC_OSC_REFRESH_INPUT_BOX.Size = new Size(25, 23);
            VRC_OSC_REFRESH_INPUT_BOX.TabIndex = 5;
            VRC_OSC_REFRESH_INPUT_BOX.TextChanged += VRC_OSC_REFRESH_INPUT_BOX_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 118);
            label2.Name = "label2";
            label2.Size = new Size(174, 15);
            label2.TabIndex = 6;
            label2.Text = "VRChat message refresh time(s):";
            // 
            // button1
            // 
            button1.Location = new Point(313, 7);
            button1.Name = "button1";
            button1.Size = new Size(46, 27);
            button1.TabIndex = 7;
            button1.Text = "help";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox_hideNotify
            // 
            checkBox_hideNotify.AutoSize = true;
            checkBox_hideNotify.Location = new Point(12, 87);
            checkBox_hideNotify.Name = "checkBox_hideNotify";
            checkBox_hideNotify.Size = new Size(174, 19);
            checkBox_hideNotify.TabIndex = 8;
            checkBox_hideNotify.Text = "Hide Minimized Notification";
            checkBox_hideNotify.UseVisualStyleBackColor = true;
            checkBox_hideNotify.CheckedChanged += checkBox_hideNotify_CheckedChanged;
            // 
            // settingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(367, 315);
            Controls.Add(checkBox_hideNotify);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(VRC_OSC_REFRESH_INPUT_BOX);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(checkBox_enableDiscord);
            Controls.Add(checkBox_enableVRC);
            Controls.Add(checkBox_startWithSystem);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "settingsForm";
            Text = "settings - some setting will be applied when this window is closed";
            Load += SettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBox_startWithSystem;
        private CheckBox checkBox_enableVRC;
        private CheckBox checkBox_enableDiscord;
        private TextBox textBox1;
        private Label label1;
        private TextBox VRC_OSC_REFRESH_INPUT_BOX;
        private Label label2;
        private Button button1;
        private CheckBox checkBox_hideNotify;
    }
}