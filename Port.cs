using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FlexPortManagerPoC
{
    internal class Port
    {
        public string PortName { get; set; } = string.Empty;

        private Telegram _telegram = new();

        public bool Write(Telegram telegram)
        {
            _telegram = telegram;
            _telegram.TimeoutTick = Environment.TickCount64 + _telegram.Timeout;
            _telegram.Status = State.Requesting;

            Console.WriteLine($"Write {PortName} {_telegram.RequestData}");
            _ = Dummy();
            return true;
        }

        async Task Dummy()
        {
            await Task.Delay(1111);
            if (_telegram.Status != State.Requesting) return;
            _telegram.ResponseData = $"echo {_telegram.RequestData}";
            _telegram.Status = State.Response;
            Console.WriteLine($"Read {PortName} {_telegram.RequestData}");
        }

        public bool IsPending
        {
            get
            {
                if (_telegram.Status == State.Requesting)
                {
                        if (_telegram.TimeoutTick <= Environment.TickCount64)
                        {
                            _telegram.Status = State.Timeout;
                            Console.WriteLine($"Timeout " + _telegram.TimeoutTick + ">" + Environment.TickCount64);
                            return false;
                        }
                    return true;
                }
                return false;
            }
        }

    }
}
