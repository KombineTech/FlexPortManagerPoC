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
            var delay = 1000;

            var telegram = PortManager.Request("COM3", "data", delay, 1000, 0);
            await Task.Delay(delay);

            do
            {
                await Task.Delay(10);
                if (telegram.Status == PortManager.State.Timedout) return;
            } while (telegram.Status != PortManager.State.Response);

            telegram.Status = PortManager.State.Done;

            Debug($"Telegram {telegram.Response}");

            await Task.Delay(3333);

            return;
        }
    }
}
