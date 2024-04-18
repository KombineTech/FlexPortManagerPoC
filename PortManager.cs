using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FlexPortManagerPoC
{
    internal class PortManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Telegram>> _telegrams = new();
        private List<Port> Ports { get; set; } = [];
        protected CancellationTokenSource cancel = new();
        private Task? loopTask;

        public PortManager()
        {
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

                    var found = new Telegram() { Execute = long.MaxValue };
                    foreach (var telegram in telegrams)
                    {
                        if (telegram.Value.Status == State.Done)
                        {
                            telegrams.TryRemove(telegram.Key, out Telegram? deleted);
                            Console.WriteLine($"Telegram {telegram.Key} removed from {portname}");
                            continue;

                        }

                        if (found.Execute < Environment.TickCount64) continue;
                        if (found.Execute < telegram.Value.Execute) continue;
                        found = telegram.Value;
                    }
                    if (found.Execute == long.MaxValue) continue;

                    Console.WriteLine($"Telegram found in  {portname}");

                    found.Response = $"ECHO {found.Write}";
                    found.Status = State.Response;

                }

                await Task.Delay(100, token);
            }
        }

        public Telegram Request(string portName, string data, int delay, int timeout, int force)
        {
            var output = new Telegram
            {
                Execute = Environment.TickCount64 + delay,
                Force = force,
                Timeout = timeout,
                Write = data,
                Status = State.Request,
            };

            _telegrams.TryAdd(portName, []);
            _telegrams[portName][System.Guid.NewGuid().ToString()] = output;

            return output;
        }

        public enum State
        {
            None,
            Request,
            Timedout,
            Response,
            Done,
        }

        public class Telegram
        {
            public long Execute { get; set; }  // Execute Time in Environment.TickCount64
            public long Force { get; set; }
            public long Timeout { get; set; }
            public string Write { get; set; } = string.Empty;
            public string Response { get; set; } = string.Empty;

            public State Status = State.None;
        }
    }
}
