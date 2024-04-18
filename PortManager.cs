using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FlexPortManagerPoC
{
    internal class PortManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Telegram>> _telegrams = new();
        private Dictionary<string, Port> Ports { get; set; } = new();
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

                    //  Console.WriteLine($"RunLoop  {portname} {telegrams.Count }");


                    if (Ports[portname].IsPending   )
                    {



                    }
                    else 
                    {
                        // find smalles ExecuteTick greater than Environment.TickCount64
                        var found = new Telegram() { ExecuteTick = long.MaxValue };
                        foreach (var telegram in telegrams)
                        {
                            if (telegram.Value.Status == State.Done)
                            {
                                telegrams.TryRemove(telegram.Key, out Telegram? deleted);
                                Console.WriteLine($"Telegram {telegram.Key} removed from {portname}");
                                continue;
                            }
                            if (telegram.Value.Status != State.Queue) continue;
                            if (telegram.Value.ExecuteTick> Environment.TickCount64) continue;
                            if (found.ExecuteTick < telegram.Value.ExecuteTick) continue;
                            found = telegram.Value;
                        }
                        if (found.ExecuteTick == long.MaxValue) continue;

                        Console.WriteLine($"Telegram found in  {portname} {found.Status}");

                        //Write

                        Ports[portname].Write(found );


                    }

                }

                await Task.Delay(100, token);
            }
        }

        public Telegram Request(string portName, string data, int delay, int timeout, int force)
        {
            var output = new Telegram
            {
                ExecuteTick = Environment.TickCount64 + delay,
                TimeoutTick = 0,
                Force = force,
                Timeout = timeout,
                RequestData = data,
                Status = State.Queue,
            };

            _telegrams.TryAdd(portName, []);
            _telegrams[portName][System.Guid.NewGuid().ToString()] = output;

            return output;
        }


    }
}
