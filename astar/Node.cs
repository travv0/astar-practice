using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar
{
    public class Node
    {
        public Node Parent { get; set; }
        public int F { get; set; }
        public int G { get; set; }

        public List<Node> GetPath()
        {
            List<Node> path = new List<Node>();
            Node tempNode = this;
            path.Add(tempNode);

            while (tempNode.Parent != null)
            {
                tempNode = tempNode.Parent;
                path.Add(tempNode);
            }

            return path;
        }
    }

    public class StartNode : Node { }

    public class EndNode : Node { }

    public class WallNode : Node { }
}
