using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinder
{
    public class CaveCompare : IComparer
    {
        // constructor
        public CaveCompare()
        {

        }

        // compares 2 objects by one of their values
        public int Compare(object x, object y)
        {
            return (int)((Cave)x).gScore - (int)((Cave)y).gScore;
        }
    }
}
