using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class HealthDamaged : HealthState
    {
        public HealthDamaged(int baseHealth)
        {
            this.baseHealth = baseHealth;
            Initialize();
        }
        public HealthDamaged(HealthState state)
        {
            this.baseHealth = state.baseHealth;
            Initialize();
        }
        private void Initialize()
        {
            damageMultiplier = 1;
            speedMultiplier = 1.2;
        }
        public override void CheckDamage(Class c)
        {
            if (c.health / (float)baseHealth <= 0.5)
            {
                c.state = new HealthHalf(this);
            }
        }
    }
}
