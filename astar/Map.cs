using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace astar
{
    public class Map
    {
        public Node[,] Nodes { get; private set; }
        public int Width { get { return this.Nodes.GetLength(0); } }
        public int Height { get { return this.Nodes.GetLength(1); } }

        public Map(int width, int height)
        {
            this.Nodes = new Node[width, height];

            // initialize with empty nodes
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    this.Nodes[x, y] = new Node();
                }
            }

            // random number generator
            Random random = new Random();

            // random start and end nodes
            int startX, startY, endX, endY;
            startX = random.Next(width);
            startY = random.Next(height);
            do
            {
                endX = random.Next(width);
                endY = random.Next(height);
            } while ((startX == endX) && (startY == endY));
            this.Nodes[startX, startY] = new StartNode();
            this.Nodes[endX, endY] = new EndNode();

            // random walls
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if ((random.Next(100) < 25) && !(this.Nodes[x, y] is StartNode) && !(this.Nodes[x, y] is EndNode))
                    {
                        this.Nodes[x, y] = new WallNode();
                    }
                }
            }
        }

        public void FindPath(bool showCalc = false, bool showCurrPath = false)
        {
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            // find start and end nodes
            Node startNode = this.Nodes.Cast<Node>().First(node => node is StartNode);
            Node endNode = this.Nodes.Cast<Node>().First(node => node is EndNode);
            int endX, endY; this.FindPosition(endNode, out endX, out endY);

            // add walls to closed list
            var walls = from Node node in this.Nodes
                        where node is WallNode
                        select node;
            closedList.AddRange(walls);

            // start with start node
            Node currentNode = startNode;

            if (!openList.Contains(startNode))
                openList.Add(startNode);

            while (openList.Count > 0)
            {
                int smallestF = Int32.MaxValue;
                foreach (Node node in openList)
                {
                    if (node.F < smallestF)
                    {
                        smallestF = node.F;
                        currentNode = node;
                    }
                }

                openList.Remove(currentNode);

                foreach (Node node in this.GetSuccessors(currentNode))
                {
                    int nodeX, nodeY;
                    this.FindPosition(currentNode, out nodeX, out nodeY);

                    int g = currentNode.G + 1;
                    int h = Math.Abs(nodeX - endX) + Math.Abs(nodeY - endY);
                    int f = g + h;

                    if (closedList.Contains(node))
                    {
                        continue;
                    }
                    if (!openList.Contains(node))
                    {
                        node.F = f;
                        node.G = g;
                        node.Parent = currentNode;
                        openList.Add(node);
                    }
                    else
                    {
                        if (f < node.F)
                        {
                            node.F = f;
                            node.G = g;
                            node.Parent = currentNode;
                        }
                    }

                    if (Node.ReferenceEquals(node, endNode))
                    {
                        this.RenderMap(openList, closedList, currentNode, false, true);
                        return;
                    }
                    else if (showCalc)
                    {
                        this.RenderMap(openList, closedList, currentNode, showCalc, showCurrPath);
                        Thread.Sleep(100);
                    }
                }

                if (!closedList.Contains(currentNode))
                    closedList.Add(currentNode);
            }
        }

        private void RenderMap(List<Node> openList, List<Node> closedList, Node currentNode, bool showCalc, bool showCurrPath)
        {
            List<Node> path = currentNode.GetPath();

            string drawStr = "";
            for (int j = 0; j < this.Height; j++)
            {
                for (int i = 0; i < this.Width; i++)
                {
                    if (this.Nodes[i, j] is StartNode)
                    {
                        drawStr += "S";
                    }
                    else if (this.Nodes[i, j] is EndNode)
                    {
                        drawStr += "E";
                    }
                    else if (this.Nodes[i, j] is WallNode)
                    {
                        drawStr += "|";
                    }
                    else if (showCurrPath && path.Contains(this.Nodes[i, j]))
                    {
                        drawStr += "O";
                    }
                    else if (openList.Contains(this.Nodes[i, j]) && showCalc)
                    {
                        drawStr += ",";
                    }
                    else if (closedList.Contains(this.Nodes[i, j]) && showCalc)
                    {
                        drawStr += ".";
                    }
                    else
                    {
                        drawStr += "-";
                    }
                }
                drawStr += "\n";
            }
            Console.Clear();
            Console.Write(drawStr);
        }

        private List<Node> GetSuccessors(Node node)
        {
            int x, y;
            this.FindPosition(node, out x, out y);

            List<Node> successorList = new List<Node>();

            if (x > 0)
                successorList.Add(this.Nodes[x - 1, y]);
            if (x + 1 < this.Width)
                successorList.Add(this.Nodes[x + 1, y]);
            if (y > 0)
                successorList.Add(this.Nodes[x, y - 1]);
            if (y + 1 < this.Height)
                successorList.Add(this.Nodes[x, y + 1]);

            return successorList;
        }

        private void FindPosition(Node node, out int x, out int y)
        {
            for (x = 0; x < this.Width; ++x)
            {
                for (y = 0; y < this.Height; ++y)
                {
                    if (Node.ReferenceEquals(this.Nodes[x,y], node))
                        return;
                }
            }
            throw new Exception("Node not in map.");
        }
    }
}
