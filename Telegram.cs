using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexPortManagerPoC
{
    public enum State
    {
        None,
        Queue,
        Requesting,
        Timeout,
        Response,
        Done,
    }

    internal class Telegram
    {
        public int Timeout { get; set; }
        public int Force { get; set; }
        public long ExecuteTick { get; set; }  // Execute Time in Environment.TickCount64
        public long TimeoutTick { get; set; }  // Timeout Time in Environment.TickCount64
        public string RequestData { get; set; } = string.Empty;
        public string ResponseData { get; set; } = string.Empty;

        public State Status = State.None;
        public void Release()
        {
            this.Status = State.Done;
        }

    }
}
