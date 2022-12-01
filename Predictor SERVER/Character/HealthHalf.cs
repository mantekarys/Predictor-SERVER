using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class HealthHalf : HealthState
    {
        public HealthHalf(int baseHealth)
        {
            this.baseHealth = baseHealth;
            Initialize();
        }
        public HealthHalf(HealthState state)
        {
            this.baseHealth = state.baseHealth;
            Initialize();
        }
        private void Initialize()
        {
            damageMultiplier = 1.5;
            speedMultiplier = 1.4;
        }
        public override void CheckDamage(Class c)
        {
            if (c.health / (float)baseHealth <= 0.25)
            {
                c.state = new HealthLow(this);
            }
        }
    }
}
