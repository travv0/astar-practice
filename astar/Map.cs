using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astar
{
    static class Map
    {
        public static int width, height;

        public static void doThings(int mapWidth, int mapHeight, bool showCalc = false)
        {
            width = mapWidth;
            height = mapHeight;

            Node[,] map = new Node[width, height];

            string draw = "";

            List<Node> wallList = new List<Node>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = new Node(i, j);
                }
            }
            List<Node> path = new List<Node>();

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            Random r = new Random();

            Node startNode = map[r.Next(width), r.Next(height)];
            Node endNode = map[r.Next(width), r.Next(height)];

            Node currentNode = startNode;

            foreach (Node node in map)
            {
                if (r.Next(100) < 25 && node != startNode && node != endNode)
                {
                    wallList.Add(node);
                }
            }

            foreach (Node wall in wallList)
            {
                closedList.Add(wall);
            }

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

                foreach (Node node in currentNode.getSuccessors(map))
                {
                    int g = currentNode.G + 1;
                    int h = Math.Abs(node.X - endNode.X) + Math.Abs(node.Y - endNode.Y);
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

                    if (node == endNode)
                    {
                        path = getPath(node);
                        goto end;
                    }

                    if (showCalc == true)
                    {
                        draw = "";
                        for (int j = 0; j < height; j++)
                        {
                            for (int i = 0; i < width; i++)
                            {
                                if (map[i, j] == startNode)
                                {
                                    draw += "S";
                                }
                                else if (map[i, j] == endNode)
                                {
                                    draw += "E";
                                }
                                else if (wallList.Contains(map[i, j]))
                                {
                                    draw += "|";
                                }
                                else if (openList.Contains(map[i, j]))
                                {
                                    draw += "o";
                                }
                                else if (closedList.Contains(map[i, j]))
                                {
                                    draw += ".";
                                }
                                else
                                {
                                    draw += "-";
                                }
                            }
                            draw += "\n";
                        }
                        Console.Clear();
                        Console.Write(draw);
                        System.Threading.Thread.Sleep(10);
                    }
                }


                if (!closedList.Contains(currentNode))
                    closedList.Add(currentNode);
            }

            end:

            draw = "";
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (map[i, j] == startNode)
                    {
                        draw += "S";
                    }
                    else if (map[i, j] == endNode)
                    {
                        draw += "E";
                    }
                    else if (wallList.Contains(map[i, j]))
                    {
                        draw += "|";
                    }
                    else if (path.Contains(map[i, j]))
                    {
                        draw += "O";
                    }
                    else
                    {
                        draw += "-";
                    }
                }
                draw += "\n";
            }
            Console.Clear();
            Console.Write(draw);
            Console.Write("Press R to restart without showing work, T to restart and show work, or anything else to exit...");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.R:
                    doThings(width, height);
                    break;
                case ConsoleKey.T:
                    doThings(width, height, true);
                    break;
            }
        }

        static List<Node> getPath(Node node)
        {
            List<Node> path = new List<Node>();
            Node tempNode = node;
            path.Add(tempNode);

            while (tempNode.Parent != null)
            {
                tempNode = tempNode.Parent;
                path.Add(tempNode);
            }

            return path;
        }
    }
}
