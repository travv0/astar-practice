using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // get map size
            Console.Write("Enter width of map (make sure it will fit in the window!): ");
            int width = Int32.Parse(Console.ReadLine());
            Console.Write("Enter height of map (make sure it will fit in the window!): ");
            int height = Int32.Parse(Console.ReadLine());

            // draw first map
            Map map = new Map(width, height);
            map.FindPath();

            // interaction loop
            while (true)
            {
                Console.Write("Press N to show new map, C to show A* calculation, or anything else to exit...");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.N:
                        map = new Map(width, height);
                        map.FindPath();
                        break;
                    case ConsoleKey.C:
                        map.FindPath(showCalc: true, showCurrPath: true);
                        break;
                    case ConsoleKey.Y:
                        map.FindPath(showCalc: true, showCurrPath: false);
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
