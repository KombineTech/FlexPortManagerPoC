using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexPortManagerPoC
{
    internal class Unit01(Storage db, PortManager pm, byte unitId) : UnitXX(db, pm, unitId)
    {
        protected override async Task MainLoop()
        {
            var delay = 3333;
            Debug($"Main");

            var telegram = PortManager.Request("COM3", $"DATA{UnitId}", delay, 10000, 0);

            do
            {
                await Task.Delay(10);
                if (telegram.Status == State.Timeout) return;
            } while (telegram.Status != State.Response);

            // do work
            Debug($"Telegram {telegram.ResponseData}");

            telegram.Release();
 

            return;
        }
    }
}
