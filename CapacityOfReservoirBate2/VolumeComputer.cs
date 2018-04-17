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
using ESRI.ArcGIS.NetworkAnalysis;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.DataSourcesRaster;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

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
        private String _DEMLayerName;
        private String _Cellsize;
        private String _Base_Z;
        private String _Separation;
        private Double _MinHeight;


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


        public VolumeComputer(String DamLyrName, String StreamNetLyrName, String WatershedLyrName, String WorkSpacePath, String DEMLyrName, String Cellsize, String Z, String Separation)
        {
            _DamLayer = GetLayerByName(DamLyrName);
            _StreamNetLayer = GetLayerByName(StreamNetLyrName);
            _WatershedLayer = GetLayerByName(WatershedLyrName);
            _WorkSpacePath = WorkSpacePath;
            _DEMLayerName = DEMLyrName;
            _Cellsize = Cellsize;
            _Base_Z = Z;
            _Separation = Separation;
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
            foreach (IFeature StreamFeature in StreamList)
            {
                ArcMap.Document.ActiveView.FocusMap.SelectFeature(StreamNetLayer, StreamFeature);
            }
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


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void ComputeVolume()
        {
            try
            {
                IFeature MergeFeature = WatershedPolygonList.First<IFeature>();
                WatershedPolygonList.Remove(MergeFeature);
                IGeometry MergeGeo = MergeFeature.Shape;
                ITopologicalOperator4 MergeTopo = MergeGeo as ITopologicalOperator4;
                MergeTopo.IsKnownSimple_2 = false;
                MergeTopo.Simplify();
                foreach (IFeature Feature in WatershedPolygonList)
                {
                    MergeGeo = MergeTopo.Union(Feature.Shape);
                    MergeTopo = MergeGeo as ITopologicalOperator4;
                }
                IPolygon MergePolygon = MergeGeo as IPolygon;

                using (ComReleaser ComReleaser = new ComReleaser())
                {
                    //MessageBox.Show("TIN TO Raster");
                    Geoprocessor GP = new Geoprocessor();
                    //ComReleaser.ManageLifetime(GP);
                    GP.OverwriteOutput = true;
                    PolygonToRaster PolygonToRaster = new ESRI.ArcGIS.ConversionTools.PolygonToRaster();
                    PolygonToRaster.cellsize = _Cellsize;
                    PolygonToRaster.cell_assignment = "CELL_CENTER";
                    PolygonToRaster.in_features = WatershedLayer as IFeatureLayer;
                    PolygonToRaster.out_rasterdataset = WorkSpacePath + @"/Raster";
                    PolygonToRaster.priority_field = "NONE";
                    PolygonToRaster.value_field = "ID";

                    GP.Execute(PolygonToRaster, null);
                    ComReleaser.ReleaseCOMObject(GP);
                }

                ILayer RasterLyr = GetLayerByName("Raster");
                ILayer FillDEMLyr = GetLayerByName(_DEMLayerName);
                IMapAlgebraOp MapAlgebraOp = new RasterMapAlgebraOpClass();
                IRasterLayer FillDemRstLayer = FillDEMLyr as IRasterLayer;
                IRasterLayer RasterRstLayer = RasterLyr as IRasterLayer;


                MapAlgebraOp.BindRaster(FillDemRstLayer as IGeoDataset, "FillDemRstLayer");
                MapAlgebraOp.BindRaster(RasterRstLayer as IGeoDataset, "RasterRstLayer");

                IGeoDataset GeoDataset = MapAlgebraOp.Execute("([RasterRstLayer] / [RasterRstLayer]) * [FillDemRstLayer]");
                IRaster Raster = GeoDataset as IRaster;
                IRasterLayer RasterLayer = new RasterLayer();
                RasterLayer.CreateFromRaster(Raster);

                IRasterBandCollection RasterBandCol = Raster as IRasterBandCollection;
                IRasterBand RasterBand = RasterBandCol.Item(0);
                IRasterStatistics Statistics = RasterBand.Statistics;
                _MinHeight = Statistics.Minimum;
                Double Separation = Convert.ToDouble(_Separation);
                Double Base_Z = Convert.ToDouble(_Base_Z);

                while (Base_Z > _MinHeight)
                {
                    using (ComReleaser ComReleaser = new ComReleaser())
                    {
                        Geoprocessor GP = new Geoprocessor();
                        //ComReleaser.ManageLifetime(GP);
                        GP.OverwriteOutput = true;
                        SurfaceVolume SurfaceVolume = new SurfaceVolume();
                        SurfaceVolume.base_z = Convert.ToDouble(Base_Z);
                        SurfaceVolume.in_surface = RasterLayer;
                        SurfaceVolume.out_text_file = WorkSpacePath + @"/" + Base_Z.ToString() + ".txt";
                        SurfaceVolume.reference_plane = "BELOW";
                        SurfaceVolume.z_factor = 1;
                        SurfaceVolume.pyramid_level_resolution = 0;

                        GP.Execute(SurfaceVolume, null);
                        ComReleaser.ReleaseCOMObject(GP);
                    }
                    Base_Z = Base_Z - Separation;
                    if (Base_Z < _MinHeight)
                    {
                        using (ComReleaser ComReleaser = new ComReleaser())
                        {
                            Geoprocessor GP = new Geoprocessor();
                            //ComReleaser.ManageLifetime(GP);
                            GP.OverwriteOutput = true;
                            SurfaceVolume SurfaceVolume = new SurfaceVolume();
                            SurfaceVolume.base_z = _MinHeight;
                            SurfaceVolume.in_surface = RasterLayer;
                            SurfaceVolume.out_text_file = WorkSpacePath + @"/" + _MinHeight.ToString() + ".txt";
                            SurfaceVolume.reference_plane = "BELOW";
                            SurfaceVolume.z_factor = 1;
                            SurfaceVolume.pyramid_level_resolution = 0;

                            GP.Execute(SurfaceVolume, null);
                            ComReleaser.ReleaseCOMObject(GP);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        private void ResultOutput()
        {
            try
            {
                HSSFWorkbook HSSFWB = new HSSFWorkbook();
                ISheet Table = HSSFWB.CreateSheet("库容");
                NPOI.SS.UserModel.IRow RowHead = Table.CreateRow(0);

                ICell Cell_0 = RowHead.CreateCell(0);
                Cell_0.SetCellValue("高度");

                ICell Cell_1 = RowHead.CreateCell(1);
                Cell_1.SetCellValue("二维面积");

                ICell Cell_2 = RowHead.CreateCell(2);
                Cell_2.SetCellValue("三维面积");

                ICell Cell_3 = RowHead.CreateCell(3);
                Cell_3.SetCellValue("库容");

                Double RasterMin = _MinHeight;
                Double Separation = Convert.ToDouble(_Separation);
                Double Base_Z = Convert.ToDouble(_Base_Z);
                int LineIndex = 1;
                while (Base_Z > RasterMin)
                {
                     
                    String FilePath = WorkSpacePath + @"/" + Base_Z.ToString() + ".txt";
                    DataRead(FilePath, Table, LineIndex);

                    Base_Z = Base_Z - Separation;
                    if (Base_Z < RasterMin)
                    {
                        DataRead(WorkSpacePath + @"/" + _MinHeight.ToString() + ".txt", Table, LineIndex);
                    }
                    ++LineIndex;
                }
                using (System.IO.FileStream Fs = File.OpenWrite(WorkSpacePath + @"/库容.xls"))
                {
                    HSSFWB.Write(Fs);   //向打开的这个xls文件中写入保存。
                }
                
            }

                
            
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            


        }


        private void DataRead(string FilePath, ISheet Table, int LineIndex)
        {
            if (!File.Exists(FilePath))
            {
                MessageBox.Show("File " + FilePath + " is not exist!");
            }
            else
            {
                System.IO.FileStream FileStream = new System.IO.FileStream(FilePath, FileMode.Open, FileAccess.Read);
                StreamReader StreamReader = new StreamReader(FileStream, Encoding.Default);
                FileStream.Seek(0, SeekOrigin.Begin);
                string ContentLine1 = StreamReader.ReadLine();
                string ContentLine2 = StreamReader.ReadLine();

                if (ContentLine2 != null)
                {

                    string Dataset = ContentLine2.Substring(0, ContentLine2.IndexOf(",")).Trim();
                    ContentLine2 = ContentLine2.Remove(0, ContentLine2.IndexOf(",") + 1);

                    string Plane_Height = ContentLine2.Substring(0, ContentLine2.IndexOf(",")).Trim();
                    ContentLine2 = ContentLine2.Remove(0, ContentLine2.IndexOf(",") + 1);

                    string Reference = ContentLine2.Substring(0, ContentLine2.IndexOf(",")).Trim();
                    ContentLine2 = ContentLine2.Remove(0, ContentLine2.IndexOf(",") + 1);

                    string Z_Factor = ContentLine2.Substring(0, ContentLine2.IndexOf(",")).Trim();
                    ContentLine2 = ContentLine2.Remove(0, ContentLine2.IndexOf(",") + 1);

                    string Area_2D = ContentLine2.Substring(0, ContentLine2.IndexOf(",")).Trim();
                    ContentLine2 = ContentLine2.Remove(0, ContentLine2.IndexOf(",") + 1);

                    string Area_3D = ContentLine2.Substring(0, ContentLine2.IndexOf(",")).Trim();
                    ContentLine2 = ContentLine2.Remove(0, ContentLine2.IndexOf(",") + 1);

                    string Volume = ContentLine2.Trim();



                    NPOI.SS.UserModel.IRow DataRow = Table.CreateRow(LineIndex);
                    ICell HeightCell = DataRow.CreateCell(0);
                    ICell Area_2DCell = DataRow.CreateCell(1);
                    ICell Area_3DCell = DataRow.CreateCell(2);
                    ICell VolumeCell = DataRow.CreateCell(3);

                    HeightCell.SetCellValue(Plane_Height);
                    Area_2DCell.SetCellValue(Area_2D);
                    Area_3DCell.SetCellValue(Area_3D);
                    VolumeCell.SetCellValue(Volume);

                }

            }
        }

        public override bool Start()
        {
            GetDamPoint();
            SelectStream();
            GetWatershedPolygon((WatershedLayer as IFeatureLayer).FeatureClass);
            ComputeVolume();
            ResultOutput();
            return true;
        }

        protected override bool Create()
        {
            throw new NotImplementedException();
        }
    }
}
