using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal abstract  class ClassCreator
    {
        public abstract Class CreateClass(int size, int x, int y);

        public static Class pickCreator(string creator,int size, int which)
        {
            int x = -1; 
            int y = -1;
            //new Class(15, 10, 5, 1, 343, 10), new Class(15, 10, 5, 1, 343, 685)
            if (which == 0)
            {
                x = 343;
                y = 10;
            }
            else if (which == 1)
            {
                x = 343;
                y = 685;
            }
            if (which == 3)
            {
                x = 10;
                y = 343;
            }
            else if (which == 4)
            {
                x = 685;
                y = 343;
            }
            if (creator == "Rogue")
            {
                return new RogueCreator().CreateClass(size, x, y);
            }
            else if (creator == "Tank")
            {
                return new TankCreator().CreateClass(size, x, y);
            }
            else// if (creator == "Gunner")
            {
                return new GunnerCreator().CreateClass(size, x, y);
            }
        }
    }
}
