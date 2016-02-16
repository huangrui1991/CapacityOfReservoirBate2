using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CapacityOfReservoirBate2
{
    public partial class CRProgressBar : Form
    {
        public CRProgressBar()
        {
            InitializeComponent();
        }

        public CRProgressBar(String LabelText)
        {
            InitializeComponent();
            
            this.ProgressBarLabel.Text = LabelText;
            ProgressBar.Value = 0;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 1000;
            Timer.Enabled = true;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (ProgressBar.Value < ProgressBar.Maximum)
            {
                ProgressBar.Value++;
            }
            else
            {
                Timer.Enabled = false;
                ProgressBar.Value = 0;
                Dispose(true);
            }
        }

        private void CRProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
        }
    }
}
