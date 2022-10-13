using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public class Trap : MapObject
    {
        int damage;
        string description;
        string name;

        public Trap(int x, int y, string color)
        {
            size = 5;
            coordinates = (x, y);

            damage = 5;
            description = "";
            name = "";
            this.color = color;
        }
    }
}
