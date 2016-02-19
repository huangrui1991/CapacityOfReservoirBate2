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
    public partial class CreateWatershedDialog : Form
    {
        private ICreater _Creater;

        public ICreater Creater
        { get; set; }

        public CreateWatershedDialog()
        {
            InitializeComponent();
            ProgressBar.Hide();
        }

        

        

    }
}
