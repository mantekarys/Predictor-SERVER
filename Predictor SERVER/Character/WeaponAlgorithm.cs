using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public abstract class WeaponAlgorithm
    {
        public string name;
        public int attackSpeed;
        public abstract Projectile attack(Class attacker, int direction);

        internal abstract WeaponAlgorithm Clone();

    }
}
