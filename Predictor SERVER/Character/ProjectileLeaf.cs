using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class ProjectileLeaf : Projectile
    {
        public ProjectileLeaf(int speed, int size, (int, int) coordinates, int direction, Class attacker)
        {
            this.speed = speed;
            this.size = size;
            this.coordinates = coordinates;
            this.direction = direction;
            this.attacker = attacker;
        }

        public override List<ProjectileLeaf> getNotHitList()
        {
            List<ProjectileLeaf> list = new List<ProjectileLeaf>();
            if (hit == false)
            {
                list.Add(new ProjectileLeaf(speed, size, coordinates, direction, attacker));
            }
            return list;
        }
        public override bool existsNotHit()
        {
            return !hit;
        }
    }
}
