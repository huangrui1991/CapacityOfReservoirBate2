using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.TrackingAnalyst;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;

namespace CapacityOfReservoirBate2
{
    public class DrawDAM : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        VolumeComputerDialog _Dialog;
        private INewLineFeedback _LineFeedback = null;
        private bool _IsMouseDown = false;
        private IPoint _CurrentPoint = null;
        private IPolyline Dam = null;


        public VolumeComputerDialog Dialog
        {
            set { _Dialog = value; }
            get { return _Dialog; }
        }

        public DrawDAM()
        {
            Dialog = new VolumeComputerDialog();
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

        protected override void OnActivate()
        {

            base.OnActivate();
        }

        protected override void OnMouseDown(ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs arg)
        {
            if (_LineFeedback == null)
            {
                _LineFeedback = new NewLineFeedback();
                _LineFeedback.Display = ArcMap.Document.ActiveView.ScreenDisplay;
            }
            if (_IsMouseDown == false)
            {
                _IsMouseDown = true;
                _LineFeedback.Start(_CurrentPoint);
            }
            else
            {
                _LineFeedback.AddPoint(_CurrentPoint);
            }
        }

        protected override void OnMouseMove(ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs arg)
        {
            _CurrentPoint = ArcMap.Document.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(arg.X, arg.Y);
            if (_LineFeedback == null)
                return;
            _LineFeedback.MoveTo(_CurrentPoint);
        }

        protected override void OnDoubleClick()
        {
            IPolyline line = null;
            if (_LineFeedback != null)
                line = _LineFeedback.Stop();

            if (line != null)
            {
                Dam = line;
                IWorkspaceFactory WorkspaceFactory = new InMemoryWorkspaceFactory();
                IWorkspaceName workspaceName = WorkspaceFactory.Create(null, "DamWorkspace", null, 0);
                IName name = (IName)workspaceName;

                // Open the workspace through the name object.
                IFeatureWorkspace Workspace = (IFeatureWorkspace)name.Open();
                UID CLSID = new ESRI.ArcGIS.esriSystem.UIDClass();
                CLSID.Value = "esriGeoDatabase.Feature";

                //IFeatureClass DamFeatureClass = Workspace.CreateFeatureClass("Dam",null,CLSID)
            }

            _LineFeedback = null;
            _IsMouseDown = false;

            

            Dialog.Show();
        }

        private void AddTemporalLayer(IFeatureClass featureClass, string eventFieldName, string temporalFieldName)
        {
            ITemporalLayer temporalFeatureLayer = new TemporalFeatureLayerClass();
            IFeatureLayer2 featureLayer = temporalFeatureLayer as IFeatureLayer2;
            ILayer layer = temporalFeatureLayer as ILayer;
            ITemporalRenderer temporalRenderer = new CoTrackSymbologyRendererClass();
            ITemporalRenderer2 temporalRenderer2 = (ITemporalRenderer2)temporalRenderer;
            IFeatureRenderer featureRenderer = temporalRenderer as IFeatureRenderer;
            ITrackSymbologyRenderer trackRenderer = temporalRenderer as ITrackSymbologyRenderer;

            if (featureLayer != null)
            {
                featureLayer.FeatureClass = featureClass;
            }

            if (featureRenderer != null)
            {
                temporalRenderer.TemporalObjectColumnName = eventFieldName;
                temporalRenderer.TemporalFieldName = temporalFieldName;
                temporalFeatureLayer.Renderer = featureRenderer;
            }

            if (trackRenderer != null)
            {
                //Create green color value
                IRgbColor rgbColor = new RgbColorClass();
                rgbColor.RGB = 0x00FF00;

                //Create simple thin green line 
                ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                simpleLineSymbol.Color = (IColor)rgbColor;
                simpleLineSymbol.Width = 1.0;

                //Create simple renderer using line symbol
                ISimpleRenderer simpleRenderer = new SimpleRendererClass();
                simpleRenderer.Symbol = (ISymbol)simpleLineSymbol;

                //Apply line renderer as track symbol and enable track rendering
                trackRenderer.TrackSymbologyRenderer = (IFeatureRenderer)simpleRenderer;
                trackRenderer.ShowTrackSymbologyLegendGroup = true;
                temporalRenderer2.TrackRendererEnabled = true;
            }

            if (layer != null)
            {
                ArcMap.Document.FocusMap.AddLayer(layer);
            }
        }
    }

}
