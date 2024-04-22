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
        public long ExecuteTick { get; set; } = long.MaxValue;  // Execute Time in Environment.TickCount64
        public long TimeoutTick { get; set; } = long.MaxValue;
        public string RequestData { get; set; } = string.Empty;
        public string ResponseData { get; set; } = string.Empty;

        public char StopChar { get; set; }

        public State Status = State.None;
        public void Release()
        {
            this.Status = State.Done;
        }

        public long ForceTick
        {
            get
            {
                if (this.Force <= 0) return long.MaxValue; ;
                return ExecuteTick - Force;
            }
        }
    }
}
