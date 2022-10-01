using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal class Rogue : Class
    {
        public Rogue(int size, int x, int y)
        {
            this.speed = 10;
            this.damage = 3;
            this.health = 5;
            this.size = size;
            this.coordinates = (x, y);
        }
    }
}
