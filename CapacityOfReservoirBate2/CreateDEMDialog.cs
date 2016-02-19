using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

namespace CapacityOfReservoirBate2
{
    public partial class CreateDEMDialog : Form
    {


        private IMap _Map = ArcMap.Document.FocusMap;
        private IFeatureLayer _SelectedFeatureLyr;
        private CRProgressBar _ProgressBar;
        private DEMCreater _Creater;

        public IFeatureLayer SelectedFeatureLyr
        {
            set { _SelectedFeatureLyr = value; }
            get { return _SelectedFeatureLyr; }
        }

        public CRProgressBar ProgressBar
        {
            get { return _ProgressBar; }
            set { _ProgressBar = value; }
        }

        public String HeightField
        {
            get { return HeightFieldComboBox.Text; }
        }

        public String OutputPath
        {
            get { return OutputPathTextBox.Text; }
        }

        public String CellSize
        {
            get { return CellSizeTextBox.Text; }
        }


        public CreateDEMDialog()
        {
            InitializeComponent();

            //init Input Feature Combobox
            IEnumLayer Lyrs = _Map.Layers;
            ILayer Lyr = Lyrs.Next();
            while (Lyr != null)
            {
                if (Lyr is IFeatureLayer)
                    this.InputFeatureComboBox.Items.Add(Lyr.Name);
                Lyr = Lyrs.Next();
            }
            
        }

        private void InputFeatureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //binding items of HeightFieldCombobox
            string LyrName = InputFeatureComboBox.SelectedItem.ToString();
            IEnumLayer Lyrs = _Map.Layers;
            Lyrs.Reset();
            ILayer SelectedLyr = Lyrs.Next();
            while (SelectedLyr != null)
            {
                if (SelectedLyr.Name == LyrName)
                    break;
                SelectedLyr = Lyrs.Next();
            }
            if (SelectedLyr == null)
            {
                MessageBox.Show("啊！哦！获取图层失败咯！");
                return;
            }
            SelectedFeatureLyr = SelectedLyr as IFeatureLayer;
            IFeatureClass FeatureCls = SelectedFeatureLyr.FeatureClass;
            IFields Fields = FeatureCls.Fields;
            int Count = Fields.FieldCount;
            for (int index = 0; index < Count; index++)
            {
                this.HeightFieldComboBox.Items.Add(Fields.get_Field(index).Name);
            }

        }


        //For Combobox is not editable
        private void InputFeatureComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void HeightFieldComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void OutputPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dialog = new FolderBrowserDialog();
            DialogResult Result = Dialog.ShowDialog();
            string path = Dialog.SelectedPath;
            this.OutputPathTextBox.Text = path;
        }

        private void OutputPathTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
        }

        private void CellSizeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            try
            {

                _Creater = new DEMCreater(SelectedFeatureLyr.FeatureClass, HeightField, OutputPath, CellSize);
                System.Threading.Thread CreateDEMThread = new System.Threading.Thread(new System.Threading.ThreadStart(ShowProgressbar));
                CreateDEMThread.Start();
                ProgressBar = new CRProgressBar("Compute Dem, Please wait!",CreateDEMThread);
                ProgressBar.Show();                             
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }

            //Start();

        }

        private void ShowProgressbar()
        {
            //
            try
            {
                bool IsCreated = _Creater.Start();

                //ProgressBar.ProgressBar.Value = 1000;
                ProgressBar.Dispose();
                //Dispose(true);

                //ProgressBar.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            //ProgressBar.Dispose();
        }

    }
}
