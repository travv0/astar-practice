﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace astar
{
    static class Map
    {
        public static int width, height;

        public static void drawPath(int mapWidth, int mapHeight, bool showCalc = false, bool showCurrPath = false, Node[,] mapToUse = null, List<Node> walls = null, Node start = null, Node end = null)
        {
            width = mapWidth;
            height = mapHeight;
            Node[,] map;
            List<Node> wallList = walls;
            Node startNode = start, endNode = end;

            Random r = new Random();

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            if (mapToUse == null)
            {
                map = new Node[width, height];

                wallList = new List<Node>();

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        map[i, j] = new Node(i, j);
                    }
                }

                startNode = map[r.Next(width), r.Next(height)];
                do
                {
                    endNode = map[r.Next(width), r.Next(height)];
                } while (endNode == startNode);

                foreach (Node node in map)
                {
                    if (r.Next(100) < 25 && node != startNode && node != endNode)
                    {
                        wallList.Add(node);
                    }
                }
            }
            else
            {
                map = mapToUse;
            }

            foreach (Node wall in wallList)
            {
                closedList.Add(wall);
            }

            List<Node> path = new List<Node>();

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
                    else if (showCalc == true)
                    {
                        path = getPath(node);
                        draw(showCalc, showCurrPath, map, wallList, startNode, endNode, openList, closedList, path);
                        Thread.Sleep(100);
                    }
                }


                if (!closedList.Contains(currentNode))
                    closedList.Add(currentNode);
            }

        end:

            draw(showCalc, true, map, wallList, startNode, endNode, openList, closedList, path);
            Console.Write("Press R to show new map, T to show A* calculation, or anything else to exit...");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.R:
                    drawPath(width, height);
                    break;
                case ConsoleKey.T:
                    drawPath(width, height, true, true, map, wallList, startNode, endNode);
                    break;
                case ConsoleKey.Y:
                    drawPath(width, height, true, false, map, wallList, startNode, endNode);
                    break;
            }
        }

        private static void draw(bool showCalc, bool showCurrPath, Node[,] map, List<Node> wallList, Node startNode, Node endNode, List<Node> openList, List<Node> closedList, List<Node> path)
        {
            string drawStr = "";
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (map[i, j] == startNode)
                    {
                        drawStr += "S";
                    }
                    else if (map[i, j] == endNode)
                    {
                        drawStr += "E";
                    }
                    else if (wallList.Contains(map[i, j]))
                    {
                        drawStr += "|";
                    }
                    else if (showCurrPath && path.Contains(map[i, j]))
                    {
                        drawStr += "O";
                    }
                    else if (openList.Contains(map[i, j]) && showCalc)
                    {
                        drawStr += ",";
                    }
                    else if (closedList.Contains(map[i, j]) && showCalc)
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

        private static List<Node> getPath(Node node)
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