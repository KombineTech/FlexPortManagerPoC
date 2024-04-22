using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexPortManagerPoC
{
    internal class UnitXX(Storage db, PortManager pm, byte unitId)
    {
        public byte UnitId { get; } = unitId;
        protected readonly Guid guid = System.Guid.NewGuid();
        protected CancellationTokenSource cancel = new();
        private Task? loopTask;

        protected PortManager PortManager = pm;
        protected Storage Database = db;

        public void Start()
        {
            if (loopTask is not null) return;
            Console.WriteLine($"Starting {UnitId} {guid}");
            cancel = new CancellationTokenSource();
            loopTask = RunLoop(cancel.Token);
        }

        public async void Stop()
        {
            if (loopTask is null) return;
            Console.WriteLine($"Stopping {UnitId} {guid}");
            cancel.Cancel();
            try { await loopTask; }
            catch (Exception) { }
            Console.WriteLine($"Stopped {UnitId} {guid}");
            loopTask = null;
        }

        async Task RunLoop(CancellationToken token)
        {
        while (!token.IsCancellationRequested)
            {
                await MainLoop();
            }
        }

        virtual protected async Task MainLoop()
        {
            Debug("MainLoop");
            await Task.Delay(1000, cancel.Token);
        }

        protected void Debug(string text)
        {
            Console.WriteLine($"{UnitId}\t{text}\t{typeof(Unit01).Name}\t{guid}");
        }
    }
}
