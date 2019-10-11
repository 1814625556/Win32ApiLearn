using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xforceplus.Plugin.AdobePrinter
{
    internal partial class AdobePanel : Form
    {
        public AdobePanel()
        {
            InitializeComponent();
            FormClosing += AdobePanel_FormClosing;
        }

        public Panel GetAdobePanel
        {
            get { return this.panelAdobe; }
        }

        private void AdobePanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
