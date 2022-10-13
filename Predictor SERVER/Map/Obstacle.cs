using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public class Obstacle : MapObject
    {
        public bool destructable;
        public int health;
        public Obstacle(int x, int y, string color)
        {
            size = 20;
            coordinates = (x, y);

            destructable = true;
            health = 5;
            this.color = color;
        }

        public void takeDamage()
        {
        }
    }
}
