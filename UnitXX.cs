using Kombine.Flex;

namespace FlexPortManagerPoC
{
    internal class UnitXX(FlexStorage db, PortManager pm, byte unitId)
    {
        public byte UnitId { get; } = unitId;
        protected readonly Guid guid = System.Guid.NewGuid();
        protected CancellationTokenSource cancel = new();
        private Task? loopTask;

        protected PortManager PortManager = pm;
        protected FlexStorage Database = db;


        public object UserInterface;
        public object InstallerInteface;
        public object StatesInteface;

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


    internal class UserInterface()
    {
        public int UserId { get; set; }
    }

    internal class SettingsInterface()
    {

    }

    internal class StatesInteface()
    {

    }














    public class StorageLinkAttribute : Attribute
    {
        public eSetting Setting { get; private set; }

        public StorageLinkAttribute(eSetting setting)
        {
            Setting = setting;
        }

        public eState State { get; private set; }

        public StorageLinkAttribute(eState state)
        {
            State = state;
        }


    }






}
