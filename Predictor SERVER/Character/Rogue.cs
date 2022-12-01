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
            this.speed = 6;
            this.damage = 2;
            this.health = 10;
            this.size = size;
            this.coordinates = (x, y);
            this.weapon = new GunWeapon("gun", 200);
            this.state = new HealthFull(health);
        }
    }
}
