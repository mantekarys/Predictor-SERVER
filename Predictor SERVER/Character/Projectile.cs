using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class Projectile
    {
        public int speed;
        public int size;
        public (int, int) coordinates;
        public int direction;
        public Class attacker;
        public Projectile(int speed, int size, (int, int) coordinates, int direction, Class attacker)
        {
            this.speed = speed;
            this.size = size;
            this.coordinates = coordinates;
            this.direction = direction;
            this.attacker = attacker;
        }
        public (int,int) move()
        {
            switch (direction)
            {
                case 0:
                    coordinates.Item2 -= speed;
                    break;
                case 1:
                    coordinates.Item1 += speed;
                    break;
                case 2:
                    coordinates.Item2 += speed;
                    break;
                case 3:
                    coordinates.Item1 -= speed;
                    break;
            }
            return coordinates;
        }

        
    }
}
