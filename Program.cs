using Kombine.Flex;

namespace FlexPortManagerPoC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Kombine.Logging.Singleton.SharedLogger.Logger = new Kombine.Logging.KombineLoggerWrapper();




            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            FlexStorage db = new FlexStorage(eUserId.FlexController, path, eTenantId.JAR, "92430495836 ", "85148999325", [1, 2, 3]);
            db.Restore();
            _ = db.StartAsync();



            db.SetUserId((int)eUserId.FlexController);


            var portManager = new PortManager();
            var units = new List<UnitXX>();

            Console.WriteLine("START");


            foreach (var unitid in new List<byte> { 1, 2, 3 })
            {



                db.SetSetting(unitid, eSetting.Booted, "Booted", true);
                db.SetSetting(unitid, eSetting.UnitType2, eUnitType.WashAlliancePump_16);
                db.SetSetting(unitid, eSetting.Name, $"Vaskemaskine {unitid}");

                //    db.SetState(unitid, eState.Door, 1);

                db.SetState(unitid, eState.Started, db.MS2000 - (long)eTime.Minute * unitid * 2);
                db.SetState(unitid, eState.Done, db.MS2000 + (long)eTime.Minute * unitid * 2);


                db.SetState(unitid, eState.OutOfOrder, eOutOfOrder.Enabled);
                //   db.SetState(unitid, eState.StartedBy, 1234);
                //   db.SetState(unitid, eState.BookedBy, 0);
                //  db.SetState(unitid, eState.Closed, 1);
                //   db.SetState(unitid, eState.Process, eProcess.Mainwash);
                //    db.SetState(unitid, eState.MachinStatus, eMachinStatus.BusyWithWash);

                db.SetCycle(unitid, eCycle.Idle);
            }




            Console.WriteLine(db.TestLink());


            var key = ConsoleKey.None;
            do
            {
                if (!Console.KeyAvailable) { Task.Delay(50); continue; };
                key = Console.ReadKey().Key;
                switch (key)
                {



                    case ConsoleKey.A:

                        break;

                    case ConsoleKey.T:
                        db.SetState(1, eState.Temperature, Environment.TickCount);
                        break;

                    case ConsoleKey.D1:
                        byte unitid = (byte)(units.Count + 1);
                        Console.WriteLine($"Add {unitid}");
                        units.Add(new Unit01(db, portManager, unitid));
                        break;

                    case ConsoleKey.D2:
                        unitid = (byte)(units.Count + 1);
                        Console.WriteLine($"Add {unitid}");
                        units.Add(new Unit02(db, portManager, unitid));
                        break;

                    case ConsoleKey.S:
                        units.ForEach(unit => { unit.Start(); });

                        break;

                    case ConsoleKey.E:
                        foreach (var unit in units) { unit.Stop(); }
                        break;

                    case ConsoleKey.U:

                        foreach (var unit in units)
                        {
                            if (unit.UserInterface is IUserPay) Console.WriteLine($" {nameof(unit)} Has IStart {unit.UnitId}");
                            if (unit.UserInterface is IUserStop) Console.WriteLine($" {nameof(unit)} Has IStop {unit.UnitId}");
                            if (unit.UserInterface is IUserDuration) Console.WriteLine($" {nameof(unit)} Has IDuration {unit.UnitId}");
                        }
                        break;


                    case ConsoleKey.J:

                        foreach (var unit in units)
                        {

                            Console.WriteLine($"{nameof(unit.UserInterface)}:{System.Text.Json.JsonSerializer.Serialize(unit.UserInterface)}");
                            Console.WriteLine($"{nameof(unit.InstallerInteface)}:{System.Text.Json.JsonSerializer.Serialize(unit.InstallerInteface)}");
                            Console.WriteLine($"{nameof(unit.StatesInteface)}:{System.Text.Json.JsonSerializer.Serialize(unit.StatesInteface)}");

                        }
                        break;




                    default:
                        break;
                }
            } while (key != ConsoleKey.Escape);


            portManager.Stop();

            Console.WriteLine("END");


            Console.ReadKey();

        }
    }
}
