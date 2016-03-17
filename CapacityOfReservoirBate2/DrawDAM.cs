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
using System.Windows.Forms;
using System.Drawing;

namespace CapacityOfReservoirBate2
{
    public class DrawDAM : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        VolumeComputerDialog _Dialog;
        private INewLineFeedback _LineFeedback = null;
        private bool _IsMouseDown = false;
        private IPoint _CurrentPoint = null;
        private IPolyline _Dam = null;
        private int _Count;


        public VolumeComputerDialog Dialog
        {
            set { _Dialog = value; }
            get { return _Dialog; }
        }

        public DrawDAM()
        {
            
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

        protected override void OnActivate()
        {
            _Count = 0;
            base.OnActivate();
        }

        protected override bool OnDeactivate()
        {
            _Count = 0;
            return base.OnDeactivate();
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
            try
            {
                if (_Count > 0)
                {
                    ArcMap.Document.ActiveView.Refresh();
                    _LineFeedback = null;
                    return;
                }
                    

                IPolyline line = null;
                if (_LineFeedback != null)
                    line = _LineFeedback.Stop();

                for (int i = 0; i < ArcMap.Document.FocusMap.LayerCount; ++i)
                {
                    ILayer Layer = ArcMap.Document.FocusMap.get_Layer(i);
                    if (Layer.Name == "Dam")
                    {
                        ArcMap.Document.FocusMap.DeleteLayer(Layer);
                    }
                }


                if (line != null)
                {
                    _Dam = line;
                    IWorkspaceFactory WorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.InMemoryWorkspaceFactoryClass();
                    IWorkspaceName WorkspaceName = WorkspaceFactory.Create(null, "DamWorkspace", null, 0);
                    IName Name = (IName)WorkspaceName;

                    IFeatureWorkspace Workspace = (IFeatureWorkspace)Name.Open();
                    UID CLSID = new ESRI.ArcGIS.esriSystem.UIDClass();
                    CLSID.Value = "esriGeoDatabase.Feature";

                    IFields Fields = new FieldsClass();
                    IFieldsEdit FieldsEdit = (IFieldsEdit)Fields;

                    IField OjIDField = new FieldClass();
                    IFieldEdit OjIDFieldEdit = OjIDField as IFieldEdit;
                    OjIDFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                    OjIDFieldEdit.Name_2 = "OBJECTID";
                    OjIDFieldEdit.AliasName_2 = "OBJECTID";
                    FieldsEdit.AddField(OjIDField);

                    IField ShapeField = new FieldClass();

                    IFieldEdit ShapeFieldEdit = ShapeField as IFieldEdit;
                    ShapeFieldEdit.Name_2 = "SHAPE";
                    ShapeFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                    IGeometryDef GeometryDef = new GeometryDefClass();
                    IGeometryDefEdit GeometryDefEdit = GeometryDef as IGeometryDefEdit;
                    GeometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
                    GeometryDefEdit.GridCount_2 = 1;
                    GeometryDefEdit.SpatialReference_2 = ArcMap.Document.FocusMap.SpatialReference;
                    ShapeFieldEdit.GeometryDef_2 = GeometryDef;
                    FieldsEdit.AddField(ShapeField);

                    IFieldChecker FieldChecker = new FieldCheckerClass();
                    IEnumFieldError EnumFieldError = null;
                    IFields ValidatedFields = null;
                    FieldChecker.ValidateWorkspace = (IWorkspace)Workspace;
                    FieldChecker.Validate(Fields, out EnumFieldError, out ValidatedFields);

                    UID EXCLSID = new UIDClass();

                    IFeatureClass DamFeatureClass = Workspace.CreateFeatureClass("Dam", ValidatedFields, CLSID, EXCLSID, esriFeatureType.esriFTSimple, "SHAPE", null);
                    IFeature Feature = DamFeatureClass.CreateFeature();
                    Feature.Shape = _Dam as IGeometry;
                    Feature.Store();

                    ESRI.ArcGIS.Carto.IFeatureLayer FeatureLayer = new ESRI.ArcGIS.Carto.FeatureLayerClass();
                    FeatureLayer.FeatureClass = DamFeatureClass;
                    FeatureLayer.Name = DamFeatureClass.AliasName;
                    FeatureLayer.Visible = true;
                    ArcMap.Document.ActiveView.FocusMap.AddLayer(FeatureLayer);
                    //AddTemporalLayer(DamFeatureClass, "EVENTID", "TA_DATE");
                }

                _LineFeedback = null;
                _IsMouseDown = false;

                Dialog = new VolumeComputerDialog();
                Dialog.Show();
                
                _Count++;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }


    }

}
