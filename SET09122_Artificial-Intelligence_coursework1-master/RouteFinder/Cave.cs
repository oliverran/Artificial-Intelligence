using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinder
{   
    // defines Cave Object 
    public class Cave
    {
        //properties of cave objects
        private int xCord;
        private int yCord;
        private int caveID;
        private Cave previousCave;
        private ArrayList neighboursList = new ArrayList();
        private double g_Score;

        // constructor
        public Cave(int x, int y, int caveID)
        {
            xCord = x;
            yCord = y;
            this.caveID = caveID;
        }

        // CaveID getter and setter
        public int CaveID
        {
            get { return caveID; }
            set { caveID = value; }
        }

        // G score value
        public double gScore
        {
            get { return g_Score; }
            set { g_Score = value; }
        }

        // Previous cave 
        public Cave FromPreviousCave
        {
            set { previousCave = value; }
            get { return previousCave; }
        }

        // add cave to neighbourcave list
        public void AddCavesToNeighbourList(Cave c)
        {
            this.neighboursList.Add(c);
        }

        // return list of neighbour the cave can go to
        public ArrayList NeighboursList
        {
            get { return neighboursList; }
        }

        // Calculate eucledian distance between 2 caves
        public double Eucledian(Cave c)
        {
            return (Math.Sqrt(Math.Pow(this.xCord - c.xCord, 2) + Math.Pow(this.yCord - c.yCord, 2)));
        }
    }
}
