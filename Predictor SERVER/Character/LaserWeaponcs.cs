using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class LaserWeapon : WeaponAlgorithm
    {
        public LaserWeapon(string name, int attackSpeed)
        {
            this.name = name;
            this.attackSpeed = attackSpeed;
        }
        public override Projectile attack(Class attacker, int direction)
        {
            return new Projectile(0, 5, attacker.coordinates, direction, attacker);
        }
    }
}
