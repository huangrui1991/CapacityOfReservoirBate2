﻿using System;
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
    public partial class VolumeComputerDialog : Form
    {

        private IMap _Map = ArcMap.Document.FocusMap;
        
        private ICreater Creater;
        public VolumeComputerDialog()
        {
            try
            {
                InitializeComponent();
                IEnumLayer Lyrs = _Map.Layers;
                ILayer Lyr = Lyrs.Next();
                while (Lyr != null)
                {
                    if (Lyr is IFeatureLayer)
                    {
                        this.DamLyrComboBox.Items.Add(Lyr.Name);
                        this.StreamNetLyrComboBox.Items.Add(Lyr.Name);
                    }

                    Lyr = Lyrs.Next();
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void VolumeComputerDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Creater = new VolumeComputer(this.DamLyrComboBox.Text,this.StreamNetLyrComboBox.Text,this.WorkSpacePathTextBox.Text);
            Creater.Start();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dialog = new FolderBrowserDialog();
            DialogResult Result = Dialog.ShowDialog();
            string path = Dialog.SelectedPath;
            this.WorkSpacePathTextBox.Text = path;
        }

       
    }
}
