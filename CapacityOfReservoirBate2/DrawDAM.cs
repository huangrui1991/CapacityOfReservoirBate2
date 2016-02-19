using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace CapacityOfReservoirBate2
{
    public class DrawDAM : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        CreateWatershedDialog _Dialog;
        private INewLineFeedback _LineFeedback = null;
        private bool _IsMouseDown = false;
        private IPoint _CurrentPoint = null;
        private IPolyline Dam = null;


        public CreateWatershedDialog Dialog
        {
            set { _Dialog = value; }
            get { return _Dialog; }
        }

        public DrawDAM()
        {
            Dialog = new CreateWatershedDialog();
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
                Dam = line;

            _LineFeedback = null;
            _IsMouseDown = false;
        }

    }

}
