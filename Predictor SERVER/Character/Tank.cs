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
            this.speed = 4;
            this.damage = 1;
            this.health = 100;
            this.size = size;
            this.coordinates = (x, y);
            this.weapon = new GunWeapon("gun", 500);
            this.state = new HealthFull(health);
        }
    }
}
