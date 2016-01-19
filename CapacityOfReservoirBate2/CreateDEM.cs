using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CapacityOfReservoirBate2
{
    public class CreateDEM : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private CreateDEMDialog dialog ;

        public CreateDEM()
        {
            
            
        }

        protected override void OnClick()
        {
            //
            //  TODO: Sample code showing how to access button host
            //
            ArcMap.Application.CurrentTool = null;
            dialog = new CreateDEMDialog();
            dialog.ShowDialog();
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
