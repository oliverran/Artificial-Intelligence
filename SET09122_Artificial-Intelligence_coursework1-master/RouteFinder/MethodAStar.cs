using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinder
{   
    // method to use the cave list
    public class MethodAStar
    {
        ArrayList list_OfCaves;
        CaveCompare cave_Comparer;

        //constructor
        public MethodAStar()
        {
            list_OfCaves = new ArrayList();
            cave_Comparer = new CaveCompare();
        }

        // return a cave found by its ID or null if not found
        public Cave FindCave(int index)
        {
            foreach (Cave c in list_OfCaves)
            {
                if (c.CaveID == index)
                {
                    return c;
                }
            }
            return null;
        }

        // add cave and sort list
        public void AddCave(Cave c)
        {
            int k = list_OfCaves.BinarySearch(c, cave_Comparer);
            // if no element
            if (k == -1) 
            {
                list_OfCaves.Insert(0, c);
            }
            // find location by complement
            else if (k < 0) 
            {
                k = ~k;
                list_OfCaves.Insert(k, c);
            }
            else if (k >= 0)
            {
                list_OfCaves.Insert(k, c);
            }
        }

        // return the Cave with the lowest g score 
        // and remove it from the list
        public Cave Take_lowest()
        {
            Cave r = (Cave)list_OfCaves[0];
            list_OfCaves.RemoveAt(0);
            return r;
        }

        // return the number of caves in the list
        public int Count()
        {
            return list_OfCaves.Count;
        }
    }
}
