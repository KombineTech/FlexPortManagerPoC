using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FlexPortManagerPoC
{
    internal class Unit02(Storage db, PortManager pm, byte unitId) : UnitXX(db, pm, unitId)
    {
        protected override async Task MainLoop()
        {
            Debug("Main");
            await Task.Delay(1666, cancel.Token);
        }
    }
}
