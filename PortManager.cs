using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FlexPortManagerPoC
{
    internal class PortManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Telegram>> _telegrams = new();
        private Dictionary<string, Port> Ports { get; set; } = [];
        protected CancellationTokenSource cancel = new();
        private Task? loopTask;

        public PortManager()
        {
            Ports.Add("COM3", new Port() { PortName = "COM3" });

            Start();
        }

        public void Start()
        {
            if (loopTask is not null) return;
            Console.WriteLine($"Starting PortManager");
            cancel = new CancellationTokenSource();
            loopTask = RunLoop(cancel.Token);
        }

        public async void Stop()
        {
            if (loopTask is null) return;
            Console.WriteLine($"Stopping PortManager");
            cancel.Cancel();
            try { await loopTask; }
            catch (Exception) { }
            Console.WriteLine($"Stopped PortManager");
        }

        async Task RunLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                foreach (KeyValuePair<string, ConcurrentDictionary<string, Telegram>> kvp in _telegrams)
                {
                    var portname = kvp.Key;
                    var telegrams = kvp.Value;

                    if (!Ports[portname].IsPending)
                    {
                        //clean up done telegrams
                        foreach (var telegram in telegrams)
                        {
                            if (telegram.Value.Status == State.Done)
                            {
                                telegrams.TryRemove(telegram.Key, out Telegram? _);
                                Console.WriteLine($"Telegram {telegram.Key} removed from {portname}");
                            }
                        }

                        var found = new Telegram();
                        var wait =  false ;

                        //find forced telegrams
                        foreach (var telegram in telegrams)
                        {
                            if (telegram.Value.Status != State.Queue) continue;
                            if (telegram.Value.Force <= 0) continue;
                            if (telegram.Value.ForceTick > Environment.TickCount64) continue;
                            if (found.ForceTick < telegram.Value.ForceTick) continue;
                            wait = true;
                            Console.WriteLine($"force found in  {portname}");                         
                            if (found.ExecuteTick < telegram.Value.ExecuteTick) continue;
                            found = telegram.Value;
                            wait = false;
                        }

                        // if waiting for foced telegram
                        if (wait) continue;

                        // find smalles ExecuteTick greater oq eq  than Environment.TickCount64
                        if (found.Status == State.None)
                        {
                            foreach (var telegram in telegrams)
                            {
                                if (telegram.Value.Status != State.Queue) continue;
                                if (telegram.Value.ExecuteTick  > Environment.TickCount64) continue;
                                if (found.ExecuteTick < telegram.Value.ExecuteTick) continue;
                                found = telegram.Value;
                            }
                        };

                        // if no telegram found 
                        if (found.Status == State.None) continue;

                        Console.WriteLine($"Telegram found in  {portname} {found.Status}");

                        Ports[portname].Write(found);
                    }

                }

                await Task.Delay(100, token);
            }
        }

        public Telegram Request(string portName, string data, int timeout, int delay, int force, char stopchar)
        {
            var output = new Telegram
            {
                ExecuteTick = Environment.TickCount64 + delay,
                Force = force,
                Timeout = timeout,
                RequestData = data,
                Status = State.Queue,
                StopChar = stopchar,
            };

            _telegrams.TryAdd(portName, []);
            _telegrams[portName][System.Guid.NewGuid().ToString()] = output;

            return output;
        }


    }
}
