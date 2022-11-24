using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class ExplosiveWeapon : WeaponAlgorithm
    {
        public ExplosiveWeapon(string name, int attackSpeed)
        {
            this.name = name;
            this.attackSpeed = attackSpeed;
        }
        public override Projectile attack(Class attacker, int direction)
        {
            return new ProjectileComposite(10, 6, attacker.coordinates, direction, attacker,3,2);
        }
    }
}
