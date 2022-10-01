using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal class Gunner : Class
    {
        public Gunner(int size, int x, int y)
        {
            this.speed = 5;
            this.damage = 1;
            this.health = 5;
            this.size = size;
            this.coordinates = (x, y);
        }
    }
}
