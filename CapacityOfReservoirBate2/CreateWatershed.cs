using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace CapacityOfReservoirBate2
{
    public class CreateWatershed : ESRI.ArcGIS.Desktop.AddIns.Button
    {

        private CreateWatershedDialog _Dialog;

        public CreateWatershedDialog Dialog
        {
            get { return _Dialog; }
            set { _Dialog = value; }
        }

        public CreateWatershed()
        {
        }

        protected override void OnClick()
        {
            ArcMap.Application.CurrentTool = null;
            Dialog = new CreateWatershedDialog();
            Dialog.Show();
        }

        protected override void OnUpdate()
        {
        }
    }
}
