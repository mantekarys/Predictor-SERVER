using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class GunWeapon : WeaponAlgorithm
    {
        public GunWeapon(string name, int attackSpeed)
        {
            this.name = name;
            this.attackSpeed = attackSpeed;
        }
        public override Projectile attack(Class attacker, int direction)
        {
            return new ProjectileLeaf(20, 8, attacker.coordinates, direction, attacker);
        }

        internal override WeaponAlgorithm Clone()
        {
            GunWeapon clone = (GunWeapon)this.MemberwiseClone();
            return clone;
        }
    }
}
