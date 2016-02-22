using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Analyst3DTools;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;

namespace CapacityOfReservoirBate2
{
    class VolumeComputer : ICreater
    {
        private SurfaceVolume _SurfaceVolume;
        private ILayer _WatershedLayer;
        private ILayer _StreamNetLayer;
        private IPolyline _Line;
        private ILayer _DamLayer;
        private String _WorkSpacePath;
        private IFeature _DamPoint = null;


        #region Properties

        public IFeature DamPoint
        {
            get
            {
                return _DamPoint;
            }
            set
            {
                _DamPoint = value;
            }
        }

        public String WorkSpacePath
        {
            get 
            {
                return _WorkSpacePath;
            }
            set
            {
                _WorkSpacePath = value;
            }
        }

        public ILayer DamLayer
        {
            get
            {
                return _DamLayer;
            }
            set
            {
                _DamLayer = value;
            }
        }

        public ILayer StreamNetLayer
        {
            get
            {
                return _StreamNetLayer;
            }
            set
            {
                _StreamNetLayer = value;
            }
        }

        public IPolyline Line
        {
            get
            {
                return _Line;
            }
            set
            {
                _Line = value;
            }
        }

        public ILayer WatershedLayer
        {
            get
            {
                return _WatershedLayer;
            }
            set
            {
                _WatershedLayer = value;
            }
        }

        public SurfaceVolume SurfaceVolume
        {
            get
            {
                return _SurfaceVolume;
            }
            set
            {
                _SurfaceVolume = value;
            }
        }

        #endregion


        public VolumeComputer(String DamLyrName,String StreamNetLyrName,String WorkSpacePath)
        {
            _DamLayer = GetLayerByName(DamLyrName) ;
            _StreamNetLayer = GetLayerByName(StreamNetLyrName);
            _WorkSpacePath = WorkSpacePath;
        }

        private void GetDamPoint()
        {
            try
            {
                Intersect IntersectTool = new Intersect();

                //init in_Feature
                if (StreamNetLayer == null)
                {
                    MessageBox.Show("河网图层为空！");
                    DamPoint = null;
                    return ;
                }
                if (DamLayer == null)
                {
                    MessageBox.Show("大坝图层为空！");
                    DamPoint = null;
                    return ;
                }
                IFeatureLayer StrNetFeatureLyr = StreamNetLayer as IFeatureLayer;
                //IFeatureClass StrNetFeatureCls = StrNetFeatureLyr.FeatureClass;
                List<ILayer> InFeatures = new List<ILayer>();
                InFeatures.Add(StreamNetLayer);
                InFeatures.Add(DamLayer);

                IntersectTool.in_features = StreamNetLayer.Name + ";" + DamLayer.Name;
                IntersectTool.out_feature_class = WorkSpacePath + @"/DamPoint";
                IntersectTool.join_attributes = "All";
                IntersectTool.output_type = "POINT";

                Geoprocessor GP = new Geoprocessor();
                IGeoProcessorResult results = (IGeoProcessorResult)GP.Execute(IntersectTool, null);
                if (results.Status != esriJobStatus.esriJobSucceeded)
                {
                    MessageBox.Show("获取大坝位置失败！");
                    return ;
                }
                ILayer DamPointLayer = GetLayerByName("DamPoint");
                IFeatureLayer DamPointFeatureLyr = DamPointLayer as IFeatureLayer;
                IFeatureClass DamPointFeatureClass = DamPointFeatureLyr.FeatureClass;
                DamPoint = DamPointFeatureClass.GetFeature(0);
                
                if (DamPoint == null)
                {
                    MessageBox.Show("获取大坝位置失败！");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            
        }

        private ILayer GetLayerByName(string Name)
        {
            IEnumLayer Lyrs = ArcMap.Document.FocusMap.get_Layers();
            Lyrs.Reset();
            ILayer Lyr = Lyrs.Next();
            while (Lyr != null)
            {
                if (Lyr.Name == Name)
                    break;
                Lyr = Lyrs.Next();
            }
            return Lyr;
        }

        public override bool Start()
        {
            GetDamPoint();
            return true;
        }

        protected override bool Create()
        {
            throw new NotImplementedException();
        }
    }
}
