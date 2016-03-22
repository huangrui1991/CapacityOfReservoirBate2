using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SpatialAnalyst;
using System.Windows.Forms;
using ESRI.ArcGIS.SpatialAnalystTools;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.esriSystem;


namespace CapacityOfReservoirBate2
{
    class WatershedCreater : ICreater
    {
        private String _DEMLayerName;
        private IGeoDataset _FillDEMDataset;
        private IGeoDataset _FlowDirDataset;
        private IGeoDataset _FlowAccDataset;
        private CreateWatershedDialog _Dialog;
        private String _WorkSpacePath;
        private IGeoDataset _StreamNetDataset;

        private IHydrologyOp _HydrologyOp = new RasterHydrologyOpClass();

        public String WorkSpacePath
        {
            get { return _WorkSpacePath; }
            set { _WorkSpacePath = value; }
        }

        public String DEMLayerName
        {
            get { return _DEMLayerName; }
            set { _DEMLayerName = value; }
        }


        public WatershedCreater(CreateWatershedDialog D, String DEMLyrName, String WSP)
        {
            DEMLayerName = DEMLyrName;
            WorkSpacePath = WSP;
            _Dialog = D;
        }
        public override bool Start()
        {
            FillDEM();
            FlowDirAcc();
            StreamNet();
            Watershed();
            return true;
        }

        protected override bool Create()
        {
            throw new NotImplementedException();
        }

        private bool FillDEM()
        {
            try
            {
                IEnumLayer Lyrs = ArcMap.Document.FocusMap.get_Layers();
                Lyrs.Reset();
                ILayer Layer = Lyrs.Next();
                ILayer DEMLayer = null;
                while (Layer != null)
                {
                    if (Layer.Name == DEMLayerName)
                    {
                        DEMLayer = Layer;
                        break;
                    }
                    Layer = Lyrs.Next();
                }
                IRasterLayer DEMRasterLyr = DEMLayer as IRasterLayer;
                IGeoDataset DEMGeoDataset = DEMRasterLyr as IGeoDataset;

                _FillDEMDataset = _HydrologyOp.Fill(DEMGeoDataset);

                //IRasterLayer FillDEMLayer = new RasterLayerClass();
                //IRaster FillDEMRst = _FillDEMDataset as IRaster;
                //FillDEMLayer.CreateFromRaster(FillDEMRst);
                //FillDEMLayer.Name = "FillDEM";
                //ArcMap.Document.AddLayer(FillDEMLayer as ILayer);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

        }

        private bool FlowDirAcc()
        {
            try
            {
                _FlowDirDataset = _HydrologyOp.FlowDirection(_FillDEMDataset, false, true);
                _FlowAccDataset = _HydrologyOp.FlowAccumulation(_FlowDirDataset);
                IRasterLayer FlowDirLayer = new RasterLayerClass();
                IRaster FlowDirRst = _FlowDirDataset as IRaster;
                FlowDirLayer.CreateFromRaster(FlowDirRst);
                FlowDirLayer.Name = "FlowDirDataset";
                ArcMap.Document.AddLayer(FlowDirLayer as ILayer);

                //MessageBox.Show(FlowAccMax.ToString() + " " + FlowAccMin.ToString());
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

        }

        private bool StreamNet()
        {
            try
            {
                //get binary thredhold
                IRaster FlowAccRst = _FlowAccDataset as IRaster;
                IRasterBandCollection FlowAccBandCol = FlowAccRst as IRasterBandCollection;
                IRasterBand FlowAccBand = FlowAccBandCol.Item(0);
                IRasterStatistics Statistics = FlowAccBand.Statistics;
                Double FlowAccMax = Statistics.Maximum;
                Double FlowAccMin = Statistics.Minimum;
                int Theldhold = Convert.ToInt16((FlowAccMax + FlowAccMin) * 0.0005);

                //usong IMapALgebraOp to binary map
                IMapAlgebraOp MapAlgebraOp = new RasterMapAlgebraOpClass();
                MapAlgebraOp.BindRaster(_FlowAccDataset, "FlowAccDataset");
                _StreamNetDataset = MapAlgebraOp.Execute("[FlowAccDataset] > " + Theldhold.ToString());
                //IRasterLayer StreamNetLayer = new RasterLayer();
                //IRaster StreamNetRaster = _StreamNetDataset as IRaster;
                //StreamNetLayer.CreateFromRaster(StreamNetRaster);
                //StreamNetLayer.Name = "BinaryFlowAccLayer";
                //ArcMap.Document.AddLayer(StreamNetLayer);

                
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private IGeoDataset CreateWatershed()
        {
            try
            {
                IGeoDataset StreamLinkDataset = CreateStreamLink();
                IGeoDataset WatershedDataset = _HydrologyOp.Watershed(_FlowDirDataset, StreamLinkDataset);
                return WatershedDataset;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        private IGeoDataset CreateStreamLink()
        {
            try
            {
                IGeoDataset StreamLinkDataset = _HydrologyOp.StreamLink(_StreamNetDataset, _FlowDirDataset);

                IGeoDataset StreamNetLineDataset = _HydrologyOp.StreamToFeature(StreamLinkDataset, _FlowDirDataset, false);
                IFeatureLayer StreamNetLineFeatLyr = new FeatureLayer();
                IFeatureClass StreamNetFeatureClass = StreamNetLineDataset as IFeatureClass;
                StreamNetLineFeatLyr.FeatureClass = StreamNetFeatureClass;
                StreamNetLineFeatLyr.Name = "StreamFeatLyr";
                ArcMap.Document.AddLayer(StreamNetLineFeatLyr);
                return StreamLinkDataset;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null ;
            }
        }

        private bool Watershed()
        {
            try
            {
                IGeoDataset WatershedDataset = CreateWatershed();
                if (WatershedDataset == null)
                {
                    MessageBox.Show("创建汇水区域失败！");
                    return false;
                }
                IConversionOp ConversionOp = new RasterConversionOpClass();
                IWorkspaceFactory WorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.InMemoryWorkspaceFactoryClass();
                IWorkspaceName WorkspaceName = WorkspaceFactory.Create(null, "DamWorkspace", null, 0);
                IName Name = (IName)WorkspaceName;

                IWorkspace Workspace = (IWorkspace)Name.Open();
                IGeoDataset WatershedFeatureDataset = ConversionOp.RasterDataToPolygonFeatureData(WatershedDataset, Workspace, "Watershed", false);
                IFeatureLayer WatershedFeatLyr = new FeatureLayer();
                IFeatureClass StreamNetFeatureClass = WatershedFeatureDataset as IFeatureClass;
                WatershedFeatLyr.FeatureClass = StreamNetFeatureClass;
                WatershedFeatLyr.Name = "WatershedFeature";
                ArcMap.Document.AddLayer(WatershedFeatLyr);
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
