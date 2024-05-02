namespace FlexPortManagerPoC
{
    internal class Unit02 : UnitXX
    {
        public Unit02(Storage db, PortManager pm, byte unitId) : base(db, pm, unitId)
        {
            this.UserInterface = new UserInterface02();
        }

        protected override async Task MainLoop()
        {

            Debug($"Main");

            var telegram = PortManager.Request("COM3", UnitId, $"DATA{UnitId}", 10000, 2000, 1000, '\0');

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


        internal class UserInterface02() : IUserPay
        {
            public bool Pay { get; set; }
        }

    }
}
