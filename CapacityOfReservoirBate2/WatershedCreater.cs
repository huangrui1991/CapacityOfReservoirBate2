using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace CapacityOfReservoirBate2
{
    class WatershedCreater : ICreater
    {
        String _StreamRasterPath;
        String _FlowDirRasterPath;
        IDataset StreamLinkDataset;
        String _WorkSpace;
        IDataset _StreamLine;


        public override bool Start()
        {
            throw new NotImplementedException();
        }

        protected override bool Create()
        {
            throw new NotImplementedException();
        }

        private bool CreateWatershed()
        {
            return true;
        }

        private bool CreateStreamLink()
        {
            return true;
        }

        private bool WatershedToPolygon()
        {
            return true;
        }

        private IGeometryCollection SelectWatershed()
        {
            IGeometryCollection WaterSheds = null;
            return WaterSheds;
        }
    }
}
