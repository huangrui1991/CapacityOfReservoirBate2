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
using ESRI.ArcGIS.ADF;

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
        private IFeature _DamPointFeature = null;
        private List<IFeature> _StreamList = new List<IFeature>();
        private HashSet<IFeature> _WatershedPolygonList = new HashSet<IFeature>();


        #region Properties

        public HashSet<IFeature> WatershedPolygonList
        {
            get { return _WatershedPolygonList; }
            set { _WatershedPolygonList = value; }
        }

        public List<IFeature> StreamList
        {
            get
            {
                return _StreamList;
            }
            set
            {
                _StreamList = value;
            }
        }

        public IFeature DamPointFeature
        {
            get
            {
                return _DamPointFeature;
            }
            set
            {
                _DamPointFeature = value;
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


        public VolumeComputer(String DamLyrName, String StreamNetLyrName, String WatershedLyrName,String WorkSpacePath)
        {
            _DamLayer = GetLayerByName(DamLyrName);
            _StreamNetLayer = GetLayerByName(StreamNetLyrName);
            _WatershedLayer = GetLayerByName(WatershedLyrName);
            _WorkSpacePath = WorkSpacePath;
        }

        private void GetDamPoint()
        {
            try
            {
                //init in_Feature
                if (StreamNetLayer == null)
                {
                    MessageBox.Show("河网图层为空！");
                    //DamPoint = null;
                    return;
                }
                if (DamLayer == null)
                {
                    MessageBox.Show("大坝图层为空！");
                    //DamPoint = null;
                    return;
                }


                IGeoProcessorResult results = Intersect(StreamNetLayer.Name, DamLayer.Name, WorkSpacePath + @"/DamPoint", "All", "POINT");
                if (results.Status != esriJobStatus.esriJobSucceeded)
                {
                    MessageBox.Show("获取大坝位置失败！");
                    return;
                }

                ILayer DamPointLayer = GetLayerByName("DamPoint");
                IFeatureLayer DamPointFeatureLyr = DamPointLayer as IFeatureLayer;
                IFeatureClass DamPointFeatureClass = DamPointFeatureLyr.FeatureClass;
                DamPointFeature = DamPointFeatureClass.GetFeature(0);
                //IGeometry DamPointGeometry = DamPointFeature.Shape;
                //IPoint DamPoint = new PointClass();
                //IGeometry tempCol = (IGeometry)DamPoint;
                //tempCol=(DamPointGeometry);
                //DamPoint. = (IPoint)tempCol;

                if (DamPointFeature == null)
                {
                    MessageBox.Show("获取大坝位置失败！");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private bool SelectStream()
        {
            //find first stream line
            IFeatureLayer StreamNetFeatureLayer = StreamNetLayer as IFeatureLayer;
            IFeatureClass StreamNetFeatureClass = StreamNetFeatureLayer.FeatureClass;
            ISpatialFilter SpatialFilter = new SpatialFilterClass();
            SpatialFilter.Geometry = DamPointFeature.Extent;
            SpatialFilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelEnvelopeIntersects;
            SpatialFilter.set_OutputSpatialReference("SpatialReference", ArcMap.Document.ActiveView.FocusMap.SpatialReference);

            IFeatureCursor Cursor = StreamNetFeatureClass.Search(SpatialFilter, false);
            IFeature FirstStreamFeature = Cursor.NextFeature();
            if (FirstStreamFeature == null)
            {
                MessageBox.Show("feature == null");
                return false;
            }

            //get FirstStreamFromNode
            GetStreamList(FirstStreamFeature);
             
            return true;


        }

        private void GetWatershedPolygon(IFeatureClass WatershedFeatureClass)
        {
            foreach (IFeature StreamFeature in StreamList)
            {
                ISpatialFilter Filter = new SpatialFilterClass();
                Filter.Geometry = StreamFeature.Shape;
                Filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                Filter.set_OutputSpatialReference("SpatialReference", ArcMap.Document.ActiveView.FocusMap.SpatialReference);

                IFeatureCursor Cursor = WatershedFeatureClass.Search(Filter, false);
                IFeature WatershedFeature = Cursor.NextFeature();
                while (WatershedFeature != null)
                {
                    WatershedPolygonList.Add(WatershedFeature);
                    WatershedFeature = Cursor.NextFeature();
                }
            }

            foreach (IFeature WatershedFeature in WatershedPolygonList)
            {
                ArcMap.Document.ActiveView.FocusMap.SelectFeature(WatershedLayer, WatershedFeature);
            }

        }

        private IGeoProcessorResult Intersect(String FirstLayerName, String SecondLayerName, String OutFeatureClass, String JoinAttribute, String OutputType)
        {
            using (ComReleaser ComReleaser = new ComReleaser())
            {
                Intersect IntersectTool = new Intersect();
                IntersectTool.in_features = FirstLayerName + ";" + SecondLayerName;
                IntersectTool.out_feature_class = OutFeatureClass;
                IntersectTool.join_attributes = JoinAttribute;
                IntersectTool.output_type = OutputType;

                Geoprocessor GP = new Geoprocessor();
                IGeoProcessorResult results = (IGeoProcessorResult)GP.Execute(IntersectTool, null);
                ComReleaser.ReleaseCOMObject(GP);
                return results;
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

        private void GetStreamList(IFeature StartFeature)
        {
            try
            {
                if (StartFeature == null)
                {
                    MessageBox.Show("StartFeature is null!");
                    return;
                }
                List<IFeature> PartialStreamList = new List<IFeature>();
                //StreamList.Add(StartFeature);

                IFields ToStreamFields = StartFeature.Fields;
                int from_node_index = ToStreamFields.FindField("FROM_NODE");
                string FirstStreamFromNode = StartFeature.get_Value(from_node_index).ToString();

                IFeatureLayer StreamNetFeatureLayer = StreamNetLayer as IFeatureLayer;
                IFeatureClass StreamNetFeatureClass = StreamNetFeatureLayer.FeatureClass;
                IQueryFilter Filter = new QueryFilterClass();
                Filter.WhereClause = "TO_NODE=" + long.Parse(FirstStreamFromNode);
                IFeatureCursor Cursor = StreamNetFeatureClass.Search(Filter, false);
                IFeature TargetFeature = Cursor.NextFeature();
                if (TargetFeature == null)
                    return;
                IFields Fields = TargetFeature.Fields;
                int to_node_index = Fields.FindField("TO_NODE");
                String ToNode = TargetFeature.get_Value(to_node_index).ToString();

                while (ToNode == FirstStreamFromNode)
                {
                    PartialStreamList.Add(TargetFeature);
                    TargetFeature = Cursor.NextFeature();
                    if (TargetFeature == null)
                        break;
                    Fields = TargetFeature.Fields;
                    to_node_index = Fields.FindField("TO_NODE");
                    ToNode = TargetFeature.get_Value(to_node_index).ToString();
                }
                foreach (IFeature Feature in PartialStreamList)
                {
                    StreamList.Add(Feature);
                }

                if (PartialStreamList.Count == 0)
                    return;
                else
                {
                    foreach (IFeature Feature in PartialStreamList)
                    {
                        GetStreamList(Feature);
                    }
                }

                foreach (IFeature StreamFeature in StreamList)
                {
                    ArcMap.Document.ActiveView.FocusMap.SelectFeature(WatershedLayer, StreamFeature);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
                
        public override bool Start()
        {
            GetDamPoint();
            SelectStream();
            GetWatershedPolygon((WatershedLayer as IFeatureLayer).FeatureClass);
            return true;
        }

        protected override bool Create()
        {
            throw new NotImplementedException();
        }
    }
}
