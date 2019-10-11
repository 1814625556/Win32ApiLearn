namespace Xforceplus.Plugin.AdobePrinter
{
    partial class AdobePanel
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
            this.panelAdobe = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelAdobe
            // 
            this.panelAdobe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdobe.Location = new System.Drawing.Point(0, 0);
            this.panelAdobe.Name = "panelAdobe";
            this.panelAdobe.Size = new System.Drawing.Size(428, 185);
            this.panelAdobe.TabIndex = 0;
            // 
            // AdobePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 185);
            this.Controls.Add(this.panelAdobe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "AdobePanel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "自定义打印容器";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAdobe;
    }
}