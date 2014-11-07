using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter width of map (make sure it will fit in the window!): ");
            int width = Int32.Parse(Console.ReadLine());
            Console.Write("Enter height of map (make sure it will fit in the window!): ");
            int height = Int32.Parse(Console.ReadLine());

            Map.drawPath(width, height);
        }
    }
}
