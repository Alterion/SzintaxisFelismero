using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzintaxisFelismero
{
    class Predikatum
    {
        public char szimb;
        public int parameterek;

        public Predikatum(char c, int p)
        {
            szimb = c;
            parameterek = p;
        }
    }
}
