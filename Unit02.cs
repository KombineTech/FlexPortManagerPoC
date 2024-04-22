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
           
            Debug($"Main");

            var telegram = PortManager.Request("COM3" , UnitId, $"DATA{UnitId}", 10000, 2000, 1000, '\0');

            do
            {
                await Task.Delay(10);
                if (telegram.Status == State.Timeout) return;
            } while (telegram.Status != State.Response);

            // do work
            Debug($"Telegram {telegram.ResponseData}");

            telegram.Release();

            await Task.Delay(1666, cancel.Token);
        }
    }
}
