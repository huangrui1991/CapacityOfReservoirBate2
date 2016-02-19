using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CapacityOfReservoirBate2
{
    public abstract class ICreater
    {
         public abstract bool Start();
         protected abstract bool Create();
    }
}
