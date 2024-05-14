using Kombine.Flex;

namespace FlexPortManagerPoC
{
    internal class Unit01 : UnitXX
    {
        public Unit01(FlexStorage db, PortManager pm, byte unitId) : base(db, pm, unitId)
        {
            this.UserInterface = new UserInterface01();
            this.InstallerInteface = new SettingsInterface01();
            this.StatesInteface = new StateInterfase01();


        }

        protected override async Task MainLoop()
        {
            Debug($"Main");



            var telegram = PortManager.Request("COM3", UnitId, $"DATA{UnitId}", 10000, 0, 0, '\n');

            do
            {
                await Task.Delay(10);
                if (telegram.Status == State.Timeout) return;
            } while (telegram.Status != State.Response);

            // do work
            Debug($"Telegram {telegram.ResponseData}");

            telegram.Release();


            await Task.Delay(10);

            return;
        }
    }

    internal class UserInterface01() : IUserPay, IUserStop, IUserDuration
    {
        public int Duration { get; set; } = 0;
        public bool Pay { get; set; } = false;
        public bool Stop { get; set; } = false;
    }

    internal class SettingsInterface01() : ISettingsPrice, ISettingsIOMotorAndHeading, ISettingsMinutesStep
    {
        public int RelayHeating { get; set; } = 0;
        public int MinutesMinimum { get; set; } = 0;
        public int MinutesMaximum { get; set; } = 0;
        public int MinuteInit { get; set; } = 0;
        public int MinutesStep { get; set; } = 0;
        public int RelayMotor { get; set; } = 0;
        public int CoolingMinutes { get; set; } = 0;
        public int PriceMethod { get; set; } = 0;
        public int Price { get; set; } = 0;
        public int PriceMinutes { get; set; } = 0;
        public int PriceRound { get; set; } = 0;

    }

    internal class StateInterfase01() : IStateProtocol
    {
        public string ProtocolIn { get; set; } = string.Empty;
        public string ProtocolOut { get; set; } = string.Empty;
    }




}
