namespace Media_Info_Transmitter
{
    partial class Udebug
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
            listBox1 = new ListBox();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 18);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(323, 424);
            listBox1.TabIndex = 0;
            // 
            // debug
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(347, 450);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "debug";
            Text = "debug";
            Load += debug_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBox1;
    }
}