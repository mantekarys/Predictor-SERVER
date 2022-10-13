using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    internal class Tank : Class
    {
        public Tank(int size, int x, int y)
        {
            this.speed = 3;
            this.damage = 5;
            this.health = 15;
            this.size = size;
            this.coordinates = (x, y);
            this.weapon = new ExplosiveWeapon("gun", 500);
        }
    }
}
