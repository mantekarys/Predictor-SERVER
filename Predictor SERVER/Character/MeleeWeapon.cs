using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class MeleeWeapon : WeaponAlgorithm
    {
        public int range;
        public MeleeWeapon(string name, int attackSpeed, int range)
        {
            this.name = name;
            this.attackSpeed = attackSpeed;
            this.range = range;
        }
        public override Projectile attack(Class attacker, int direction)
        {
            return new Projectile(0, range, attacker.coordinates, direction, attacker);
        }
    }
}
