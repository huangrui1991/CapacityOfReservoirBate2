using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Analyst3DTools;

namespace CapacityOfReservoirBate2
{
    class DEMCreater
    {
        private IFeatureClass _FeatureCls;
        private string _FieldName;
        private string _WorkSpacePath;
        private string _CellSize;
        

        public string CellSize
        {
            get { return _CellSize; }
            set { _CellSize = value; }
        }

        public String FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public IFeatureClass FeatureCls
        {
            get { return _FeatureCls; }
            set { _FeatureCls = value; }
        }

        public string WorkSpacePath
        {
            get { return _WorkSpacePath; }
            set { _WorkSpacePath = value; }
        }

        

        public DEMCreater(IFeatureClass SelectedFeatureCls,string HeightField,string WorkSpace,string CellSize)
        {
            _FeatureCls = SelectedFeatureCls;
            _FieldName = HeightField;
            _WorkSpacePath = WorkSpace;
            _CellSize = CellSize;
            
        }
        public bool Start()
        {
            try
            {
                if (_FeatureCls == null)
                {
                    MessageBox.Show("Oop！没有找到图层哟！");
                    return false;
                }

                if (CellSize == "")
                {
                    MessageBox.Show("DEM 栅格大小不能为空！");
                    return false;
                }
                if (WorkSpacePath == "")
                {
                    MessageBox.Show("输出DEM路径不能为空！");
                    return false;
                }
                
                //Create TIN
                IFields Fields = FeatureCls.Fields;
                int index = Fields.FindField(FieldName);
                IField Field = Fields.get_Field(index);
                ITinEdit TinEdit = new TinClass();
                TinEdit.StartEditing();
                TinEdit.InitNew(ArcMap.Document.ActiveView.Extent);
                object obj = true;
                TinEdit.AddFromFeatureClass(FeatureCls, null, Field, null, esriTinSurfaceType.esriTinHardLine, ref obj);
                TinEdit.SaveAs(WorkSpacePath + @"/TIN", ref obj);
                TinEdit.StopEditing(false);


                //TIN TO Raster
                Geoprocessor GP = new Geoprocessor();
                GP.OverwriteOutput = true;
                TinRaster TintoRaster = new TinRaster();
                TintoRaster.in_tin = TinEdit as TinClass;
                TintoRaster.out_raster = WorkSpacePath + @"\DEM";
                //TintoRaster.sample_distance = @"CELLSIZE 100" ;
                TintoRaster.sample_distance = @"CELLSIZE " + CellSize;
                TintoRaster.method = @"NATURAL_NEIGHBORS";
                TintoRaster.data_type = "FLOAT";
                GP.Execute(TintoRaster, null);
                
                return true;
                                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            
        }
    }
    
}
