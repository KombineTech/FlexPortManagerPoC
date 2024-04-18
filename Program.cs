namespace FlexPortManagerPoC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new Storage();
            var portManager = new PortManager();
            var units = new List<UnitXX>();

            Console.WriteLine("START");

            var key = ConsoleKey.None;
            do
            {
                if (!Console.KeyAvailable) { Task.Delay(50); continue; };
                key = Console.ReadKey().Key;
                switch (key)
                {
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
