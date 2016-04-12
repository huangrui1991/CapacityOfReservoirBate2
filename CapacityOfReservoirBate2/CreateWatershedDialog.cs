using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace CapacityOfReservoirBate2
{
    public partial class CreateWatershedDialog : Form
    {
        private ICreater _Creater;

        public ICreater Creater
        {
            get { return _Creater; }
            set { _Creater = value; }
        }

        public CreateWatershedDialog()
        {
            InitializeComponent();
            BinaryIndexTextBox.DataBindings.Add("Text", BinaryIndexTrackBar, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
            ProgressBar.Hide();
        }

        private void CreateWatershedDialog_Load(object sender, EventArgs e)
        {
            IEnumLayer Lyrs = ArcMap.Document.FocusMap.Layers;
            ILayer Lyr = Lyrs.Next();
            while (Lyr != null)
            {
                if (Lyr is IRasterLayer)
                    this.InputDEMComboBox.Items.Add(Lyr.Name);
                Lyr = Lyrs.Next();
            }
        }

        private void WorkSpaceButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dialog = new FolderBrowserDialog();
            DialogResult Result = Dialog.ShowDialog();
            string path = Dialog.SelectedPath;
            
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Creater = new WatershedCreater(this.InputDEMComboBox.Text,this.BinaryIndexTextBox.Text);
            //ProgressBar.Show();
            Creater.Start();
            this.Dispose();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


    }
}
