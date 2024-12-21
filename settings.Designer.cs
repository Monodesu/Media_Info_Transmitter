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
            button2 = new Button();
            groupBox1 = new GroupBox();
            radioButton_SwitchToVideoAction1 = new RadioButton();
            radioButton_IgnoreAction1 = new RadioButton();
            groupBox2 = new GroupBox();
            checkBox_informationExport = new CheckBox();
            groupBox3 = new GroupBox();
            textBox2 = new TextBox();
            label3 = new Label();
            checkBox_ThumbnailExport = new CheckBox();
            groupBox4 = new GroupBox();
            textBox_videoAppID = new TextBox();
            textBox_musicAppID = new TextBox();
            label7 = new Label();
            label6 = new Label();
            textBox_videoIconAddr = new TextBox();
            textBox_musicIconAddr = new TextBox();
            label5 = new Label();
            label4 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
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
            textBox1.Location = new Point(6, 71);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(330, 50);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 53);
            label1.Name = "label1";
            label1.Size = new Size(159, 15);
            label1.TabIndex = 4;
            label1.Text = "VRChat OSC message format:";
            // 
            // VRC_OSC_REFRESH_INPUT_BOX
            // 
            VRC_OSC_REFRESH_INPUT_BOX.Location = new Point(198, 22);
            VRC_OSC_REFRESH_INPUT_BOX.Name = "VRC_OSC_REFRESH_INPUT_BOX";
            VRC_OSC_REFRESH_INPUT_BOX.Size = new Size(25, 23);
            VRC_OSC_REFRESH_INPUT_BOX.TabIndex = 5;
            VRC_OSC_REFRESH_INPUT_BOX.TextChanged += VRC_OSC_REFRESH_INPUT_BOX_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 26);
            label2.Name = "label2";
            label2.Size = new Size(174, 15);
            label2.TabIndex = 6;
            label2.Text = "VRChat message refresh time(s):";
            // 
            // button1
            // 
            button1.Location = new Point(300, 7);
            button1.Name = "button1";
            button1.Size = new Size(59, 27);
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
            // button2
            // 
            button2.Location = new Point(300, 37);
            button2.Name = "button2";
            button2.Size = new Size(59, 27);
            button2.TabIndex = 10;
            button2.Text = "debug";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton_SwitchToVideoAction1);
            groupBox1.Controls.Add(radioButton_IgnoreAction1);
            groupBox1.Location = new Point(12, 162);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(347, 76);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Priority action for media without album";
            // 
            // radioButton_SwitchToVideoAction1
            // 
            radioButton_SwitchToVideoAction1.AutoSize = true;
            radioButton_SwitchToVideoAction1.Location = new Point(10, 47);
            radioButton_SwitchToVideoAction1.Name = "radioButton_SwitchToVideoAction1";
            radioButton_SwitchToVideoAction1.Size = new Size(167, 19);
            radioButton_SwitchToVideoAction1.TabIndex = 1;
            radioButton_SwitchToVideoAction1.TabStop = true;
            radioButton_SwitchToVideoAction1.Text = "Switch to video infomation";
            radioButton_SwitchToVideoAction1.UseVisualStyleBackColor = true;
            radioButton_SwitchToVideoAction1.CheckedChanged += radioButton_SwitchToVideoAction1_CheckedChanged;
            // 
            // radioButton_IgnoreAction1
            // 
            radioButton_IgnoreAction1.AutoSize = true;
            radioButton_IgnoreAction1.Location = new Point(10, 22);
            radioButton_IgnoreAction1.Name = "radioButton_IgnoreAction1";
            radioButton_IgnoreAction1.Size = new Size(59, 19);
            radioButton_IgnoreAction1.TabIndex = 0;
            radioButton_IgnoreAction1.TabStop = true;
            radioButton_IgnoreAction1.Text = "Ignore";
            radioButton_IgnoreAction1.UseVisualStyleBackColor = true;
            radioButton_IgnoreAction1.CheckedChanged += radioButton_IgnoreAction1_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(VRC_OSC_REFRESH_INPUT_BOX);
            groupBox2.Location = new Point(365, 7);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(347, 129);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "VRChat";
            // 
            // checkBox_informationExport
            // 
            checkBox_informationExport.AutoSize = true;
            checkBox_informationExport.Location = new Point(12, 112);
            checkBox_informationExport.Name = "checkBox_informationExport";
            checkBox_informationExport.Size = new Size(125, 19);
            checkBox_informationExport.TabIndex = 13;
            checkBox_informationExport.Text = "Information export";
            checkBox_informationExport.UseVisualStyleBackColor = true;
            checkBox_informationExport.CheckedChanged += checkBox_informationExport_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textBox2);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new Point(365, 142);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(347, 98);
            groupBox3.TabIndex = 14;
            groupBox3.TabStop = false;
            groupBox3.Text = "Export";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(6, 37);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(330, 50);
            textBox2.TabIndex = 1;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 19);
            label3.Name = "label3";
            label3.Size = new Size(82, 15);
            label3.TabIndex = 0;
            label3.Text = "Export format:";
            // 
            // checkBox_ThumbnailExport
            // 
            checkBox_ThumbnailExport.AutoSize = true;
            checkBox_ThumbnailExport.Location = new Point(12, 137);
            checkBox_ThumbnailExport.Name = "checkBox_ThumbnailExport";
            checkBox_ThumbnailExport.Size = new Size(119, 19);
            checkBox_ThumbnailExport.TabIndex = 15;
            checkBox_ThumbnailExport.Text = "Thumbnail Export";
            checkBox_ThumbnailExport.UseVisualStyleBackColor = true;
            checkBox_ThumbnailExport.CheckedChanged += checkBox_ThumbnailExport_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox_videoAppID);
            groupBox4.Controls.Add(textBox_musicAppID);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(textBox_videoIconAddr);
            groupBox4.Controls.Add(textBox_musicIconAddr);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(label4);
            groupBox4.Location = new Point(12, 244);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(700, 86);
            groupBox4.TabIndex = 16;
            groupBox4.TabStop = false;
            groupBox4.Text = "RPC ID and icon - for discord (ignore if you don't need it)";
            // 
            // textBox_videoAppID
            // 
            textBox_videoAppID.Location = new Point(447, 51);
            textBox_videoAppID.Name = "textBox_videoAppID";
            textBox_videoAppID.Size = new Size(245, 23);
            textBox_videoAppID.TabIndex = 7;
            textBox_videoAppID.TextChanged += textBox_videoAppKey_TextChanged;
            // 
            // textBox_musicAppID
            // 
            textBox_musicAppID.Location = new Point(447, 22);
            textBox_musicAppID.Name = "textBox_musicAppID";
            textBox_musicAppID.Size = new Size(245, 23);
            textBox_musicAppID.TabIndex = 6;
            textBox_musicAppID.TextChanged += textBox_musicAppKey_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(353, 54);
            label7.Name = "label7";
            label7.Size = new Size(79, 15);
            label7.TabIndex = 5;
            label7.Text = "Video App ID:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(353, 25);
            label6.Name = "label6";
            label6.Size = new Size(81, 15);
            label6.TabIndex = 4;
            label6.Text = "Music App ID:";
            // 
            // textBox_videoIconAddr
            // 
            textBox_videoIconAddr.Location = new Point(97, 51);
            textBox_videoIconAddr.Name = "textBox_videoIconAddr";
            textBox_videoIconAddr.Size = new Size(250, 23);
            textBox_videoIconAddr.TabIndex = 3;
            textBox_videoIconAddr.TextChanged += textBox_videoIconAddr_TextChanged;
            // 
            // textBox_musicIconAddr
            // 
            textBox_musicIconAddr.Location = new Point(97, 22);
            textBox_musicIconAddr.Name = "textBox_musicIconAddr";
            textBox_musicIconAddr.Size = new Size(250, 23);
            textBox_musicIconAddr.TabIndex = 2;
            textBox_musicIconAddr.TextChanged += textBox_musicIconAddr_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 54);
            label5.Name = "label5";
            label5.Size = new Size(83, 15);
            label5.TabIndex = 1;
            label5.Text = "Video Icon url:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 25);
            label4.Name = "label4";
            label4.Size = new Size(85, 15);
            label4.TabIndex = 0;
            label4.Text = "Music Icon url:";
            // 
            // settingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(722, 341);
            Controls.Add(groupBox4);
            Controls.Add(checkBox_ThumbnailExport);
            Controls.Add(groupBox3);
            Controls.Add(checkBox_informationExport);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button2);
            Controls.Add(checkBox_hideNotify);
            Controls.Add(button1);
            Controls.Add(checkBox_enableDiscord);
            Controls.Add(checkBox_enableVRC);
            Controls.Add(checkBox_startWithSystem);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "settingsForm";
            Text = "settings - some setting will be applied when this window is closed";
            Load += SettingsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
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
        private Button button2;
        private GroupBox groupBox1;
        private RadioButton radioButton_SwitchToVideoAction1;
        private RadioButton radioButton_IgnoreAction1;
        private GroupBox groupBox2;
        private CheckBox checkBox_informationExport;
        private GroupBox groupBox3;
        private Label label3;
        private CheckBox checkBox_ThumbnailExport;
        private TextBox textBox2;
        private GroupBox groupBox4;
        private TextBox textBox_videoIconAddr;
        private TextBox textBox_musicIconAddr;
        private Label label5;
        private Label label4;
        private TextBox textBox_videoAppID;
        private TextBox textBox_musicAppID;
        private Label label7;
        private Label label6;
    }
}