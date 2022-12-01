using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public abstract class HealthState
    {
        public int baseHealth;
        public double speedMultiplier;
        public double damageMultiplier;
        public abstract void CheckDamage(Class c);
    }
}
