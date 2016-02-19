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
        System.Threading.Thread _Thread;

        public System.Threading.Thread Thread
        {
            get { return _Thread; }
            set { _Thread = value; }
        }


        public CRProgressBar()
        {
            InitializeComponent();
        }

        public CRProgressBar(String LabelText, System.Threading.Thread Th)
        {
            InitializeComponent();

            Thread = Th;

            this.ProgressBarLabel.Text = LabelText;
            ProgressBar.Value = 0;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 1000;
            Timer.Enabled = true;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            //while (Thread.IsAlive)
            //{
            if (ProgressBar.Value < ProgressBar.Maximum)
            {
                ProgressBar.Value++;
            }
            else
            {
                //Timer.Enabled = false;
                ProgressBar.Value = 0;
                //Dispose(true);
            }
            //}


        }

        private void CRProgressBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            MessageBoxButtons MessButton = MessageBoxButtons.OKCancel;
            DialogResult Result = MessageBox.Show("确定要退出创建DEM进程吗？", "退出创建DEM进程", MessButton);
            if (Result == DialogResult.OK)
            {
                Thread.Abort();
                Thread.Join();
               // Thread = null;
                this.Dispose();
            }
        }
    }
}
