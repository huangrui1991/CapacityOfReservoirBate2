using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CapacityOfReservoirBate2
{
    public class DrawInterestingRegion : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public DrawInterestingRegion()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
