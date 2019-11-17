using System.Windows.Forms;
using HandyBoxApp.Properties;

namespace HandyBoxApp
{
    partial class MainForm
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
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.TransparencyKey = this.BackColor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "MainForm";
            this.Size = new System.Drawing.Size(0, 0);
            this.ShowIcon = false;
            this.AutoSize = true;
            this.Text = "Handy Box v1.0 by Halit Yurtsever";
            this.TopMost = Settings.Default.OnTop;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.Load += MainForm_Load;
        }

        #endregion
    }
}

