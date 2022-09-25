using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal class Obstacle : MapObject
    {
        public bool breakable;
        public int health;
        public Obstacle(int x, int y)
        {
            size = 5;
            health = 5;
            coordinates = (x, y);
        }
    }
}
