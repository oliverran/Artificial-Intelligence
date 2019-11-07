using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Collections;

namespace RouteFinder
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {  
                // add .cav to command line arguments 
                string filename = args[0] + ".cav";

                // check if the file exist in the directory where .exe is
                if (File.Exists(filename))
                {
                    // read file into a string
                    string input = File.ReadAllText(filename);

                    // declare arraylist to store all the caves
                    ArrayList caves = new ArrayList();

                    // declare caveID
                    int caveID = 1;

                    // if input string is not empty
                    if (input != null)
                    {
                        // write input file to an array and split by ','
                        string[] inputArray = input.Split(',');

                        // number of caves is the first int of the array
                        int cavesNumber = Int16.Parse(inputArray[0]);

                        // number of caves coords (2 * number of caves)
                        int cavCoordsNumber = cavesNumber * 2;

                        // for loop increment by 2 each time to get the pairs from array which holds the input file 
                        for (int c = 1; c < cavCoordsNumber; c += 2)
                        {
                            int x = Int16.Parse(inputArray[c]);
                            int y = Int16.Parse(inputArray[c + 1]);
                            //add cave objects to list 
                            Cave cave = new Cave(x, y, caveID);
                            caves.Add(cave);
                            caveID++;
                        }

                        // connection matrix 2d array 7x7
                        Boolean[][] connections = new Boolean[cavesNumber][];

                        for (int rows = 0; rows < cavesNumber; rows++)
                        {
                            connections[rows] = new Boolean[cavesNumber];
                        }

                        // Read the connections from the 2D array, starting point is after the coordinates
                        int col = 0; int row = 0;
                        for (int point = cavCoordsNumber + 1; point < inputArray.Length; point++)
                        {
                            // find which caves are connected
                            if (inputArray[point].Equals("1"))
                            {
                                connections[row][col] = true;
                            }
                            else
                            {
                                connections[row][col] = false;
                            }
                            row++;

                            if (row == cavesNumber)
                            {
                                row = 0;
                                col++;
                            }
                        }
                        // Generate a list with the caves to go 
                        for (int i = 1; i < caves.Count + 1; i++)
                        {
                            Cave parentCave = null;
                            Cave possibleChildCave = null;

                            foreach (Cave pc in caves)
                            {
                                if (pc.CaveID == i)
                                {
                                    parentCave = pc;
                                }
                            }

                            for (int j = 1; j < caves.Count + 1; j++)
                            {
                                foreach (Cave pcd in caves)
                                {
                                    if (pcd.CaveID == j)
                                    {
                                        possibleChildCave = pcd;
                                    }
                                }

                                int CaveOneID = parentCave.CaveID;
                                int CaveTwoID = possibleChildCave.CaveID;

                                Boolean check_connection = connections[CaveOneID - 1][CaveTwoID - 1];

                                if (!parentCave.Equals(possibleChildCave) && check_connection)
                                {
                                    parentCave.AddCavesToNeighbourList(possibleChildCave);
                                }
                            }
                        }
                        // Declare first and last cave
                        Cave firstCave = (Cave)caves[0];
                        Cave lastCave = (Cave)caves[caves.Count - 1];
                        //call A* function
                        ArrayList aStarCaves = AStarSearch(firstCave, lastCave, args);
                    }
                    else
                    {
                        Console.WriteLine("Input file is empty!");
                        Console.ReadKey();
                    }

                }
                else
                {
                    Console.WriteLine("File not found!");
                }
            }
        }
        //A* algorithm
        static ArrayList AStarSearch(Cave firstCave, Cave lastCave, string[] args)
        {
            Cave currentNode;

            // declare an open list and add the first cave to it 
            MethodAStar openList = new MethodAStar();
            openList.AddCave(firstCave);

            // declare an empty closed list
            MethodAStar closedList = new MethodAStar();

            // while open list is not empty
            while (openList.Count() != 0)
            {
                // bind current node the lowest gscore
                currentNode = openList.Take_lowest();
                // add current node in the closed list 
                closedList.AddCave(currentNode);
                // neighbours of current node list
                ArrayList neighbours_list = currentNode.NeighboursList;

                //for each neighbour cave in the current cave neighbour list
                foreach (Cave neighbour in neighbours_list)
                {
                    // while the neighbour cave is not in the closed list and it is in the open list
                    while ((closedList.FindCave(neighbour.CaveID) == null) && (openList.FindCave(neighbour.CaveID) == null))
                    {
                        // calculate current node neighbour gscore
                        neighbour.gScore = currentNode.gScore + neighbour.Eucledian(currentNode);

                        // change the parent of the previous cave to the current node
                        neighbour.FromPreviousCave = currentNode;

                        // add current node neighbour to the open list
                        openList.AddCave(neighbour);
                    }
                    // if neighbour has lower gscore than current node
                    if (neighbour.gScore > (currentNode.gScore + neighbour.Eucledian(currentNode)))
                    {
                        // replace the neighbour with the new, lower, g score
                        neighbour.gScore = currentNode.gScore + neighbour.Eucledian(currentNode);
                        // current node is now the neighbour parent cave
                        neighbour.FromPreviousCave = currentNode;
                    }
                }

                // if the current cave is the last cave
                if (currentNode == lastCave)
                {
                    // assign current cave to last cave
                    Cave current = lastCave;

                    //declare a list to contain the answer path
                    ArrayList answerPath = new ArrayList();

                    while (current != null)
                    {
                        // add from last cave to first cave
                        answerPath.Add(current);
                        current = current.FromPreviousCave;
                    }
                    // reverse answer path (first cave to last cave)
                    answerPath.Reverse();

                    //Print solution path
                    Console.Write("Path : ");
                    foreach (Cave c in answerPath)
                    {
                        Console.Write(c.CaveID);
                        Console.Write(" ");
                        // write file path to a .csn file with the name of the arguments input
                        File.AppendAllText( args[0] + ".csn", c.CaveID + " ");
                    }
                    // write the length of the path to console
                    Console.WriteLine(Environment.NewLine + "Length: " + String.Format("{0:0.00}", lastCave.gScore));
                }
            }
            // print 0 if no path found (last cave not found in closed list)
            if (closedList.FindCave(lastCave.CaveID) == null)
            {
                Console.WriteLine("0");
                File.AppendAllText(args[0] + ".csn", "0");
            }
            return null;
        }
    }
}