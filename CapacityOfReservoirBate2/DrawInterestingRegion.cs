using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CapacityOfReservoirBate2
{
    public class DrawDAM : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public DrawDAM()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
