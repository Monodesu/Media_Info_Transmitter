namespace Media_Info_To_VRChat_Discord
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            title_label = new Label();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            PlayStatusLabel = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            playtime_label = new ToolStripStatusLabel();
            media_title = new Label();
            media_artist = new Label();
            media_album = new Label();
            button1 = new Button();
            notifyIcon1 = new NotifyIcon(components);
            ThumbnailBox1 = new PictureBox();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ThumbnailBox1).BeginInit();
            SuspendLayout();
            // 
            // title_label
            // 
            title_label.AutoSize = true;
            title_label.Location = new Point(9, 6);
            title_label.Name = "title_label";
            title_label.Size = new Size(57, 15);
            title_label.TabIndex = 0;
            title_label.Text = "Waiting...";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, PlayStatusLabel, toolStripStatusLabel2, playtime_label });
            statusStrip1.Location = new Point(0, 188);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(351, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(42, 17);
            toolStripStatusLabel1.Text = "Status:";
            // 
            // PlayStatusLabel
            // 
            PlayStatusLabel.Name = "PlayStatusLabel";
            PlayStatusLabel.Size = new Size(29, 17);
            PlayStatusLabel.Text = "Null";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(57, 17);
            toolStripStatusLabel2.Text = "PlayTime:";
            // 
            // playtime_label
            // 
            playtime_label.Name = "playtime_label";
            playtime_label.Size = new Size(29, 17);
            playtime_label.Text = "Null";
            // 
            // media_title
            // 
            media_title.AutoSize = true;
            media_title.Location = new Point(9, 21);
            media_title.Name = "media_title";
            media_title.Size = new Size(29, 15);
            media_title.TabIndex = 2;
            media_title.Text = "Title";
            // 
            // media_artist
            // 
            media_artist.AutoSize = true;
            media_artist.Location = new Point(9, 36);
            media_artist.Name = "media_artist";
            media_artist.Size = new Size(35, 15);
            media_artist.TabIndex = 3;
            media_artist.Text = "Artist";
            // 
            // media_album
            // 
            media_album.AutoSize = true;
            media_album.Location = new Point(9, 51);
            media_album.Name = "media_album";
            media_album.Size = new Size(42, 15);
            media_album.TabIndex = 4;
            media_album.Text = "Album";
            // 
            // button1
            // 
            button1.Location = new Point(272, 154);
            button1.Name = "button1";
            button1.Size = new Size(69, 25);
            button1.TabIndex = 5;
            button1.Text = "Settings";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            // 
            // ThumbnailBox1
            // 
            ThumbnailBox1.Location = new Point(11, 75);
            ThumbnailBox1.Name = "ThumbnailBox1";
            ThumbnailBox1.Size = new Size(100, 100);
            ThumbnailBox1.SizeMode = PictureBoxSizeMode.Zoom;
            ThumbnailBox1.TabIndex = 6;
            ThumbnailBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(351, 210);
            Controls.Add(ThumbnailBox1);
            Controls.Add(button1);
            Controls.Add(media_album);
            Controls.Add(media_artist);
            Controls.Add(media_title);
            Controls.Add(statusStrip1);
            Controls.Add(title_label);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Media Info Transmitter v1.1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ThumbnailBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label title_label;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel PlayStatusLabel;
        private Label media_title;
        private Label media_artist;
        private Label media_album;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel playtime_label;
        private Button button1;
        private NotifyIcon notifyIcon1;
        private PictureBox ThumbnailBox1;
    }
}
