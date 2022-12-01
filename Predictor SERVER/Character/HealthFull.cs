using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class HealthFull : HealthState
    {

        public HealthFull(int baseHealth)
        {
            this.baseHealth = baseHealth;
            Initialize();
        }

        private void Initialize()
        {
            damageMultiplier = 1;
            speedMultiplier = 1;
        }
        public override void CheckDamage(Class c)
        {
            if (c.health / (float)baseHealth <= 0.75)
            {
                c.state = new HealthDamaged(this);
            }
        }
    }
}
