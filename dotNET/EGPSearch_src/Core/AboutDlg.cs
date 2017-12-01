using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EGPSearch
{
    public partial class AboutDlg : Form
    {
        public AboutDlg(string taskName)
        {
            InitializeComponent();

            lblTaskname.Text = taskName;
            string file = System.Reflection.Assembly.GetExecutingAssembly().Location;
            lblVersionNum.Text = System.Diagnostics.FileVersionInfo.GetVersionInfo(file).FileVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://blogs.sas.com/sasdummy");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
