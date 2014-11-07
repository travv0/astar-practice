using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar
{
    class Node
    {
        private int f, g, h;
        private int x, y;

        private Node parent;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Node Parent { get { return parent; } set { parent = value; } }
        public int F { get { return f; } set { f = value; } }
        public int G { get { return g; } set { g = value; } }
        public int H { get { return h; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }

        public List<Node> getSuccessors(Node[,] map)
        {
            List<Node> successorList = new List<Node>();

            if (x > 0)
                successorList.Add(map[x - 1, y]);
            if (x + 1 < Map.width)
                successorList.Add(map[x + 1, y]);
            if (y > 0)
                successorList.Add(map[x, y - 1]);
            if (y + 1 < Map.height)
                successorList.Add(map[x, y + 1]);

            return successorList;
        }
    }
}
