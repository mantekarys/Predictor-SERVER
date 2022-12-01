using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Character
{
    public class HealthLow : HealthState
    {
        public HealthLow(int baseHealth)
        {
            this.baseHealth = baseHealth;
            Initialize();
        }
        public HealthLow(HealthState state)
        {
            this.baseHealth = state.baseHealth;
            this.speedMultiplier = state.speedMultiplier;
            this.damageMultiplier = state.damageMultiplier;
            Initialize();
        }
        private void Initialize()
        {
            damageMultiplier = 2;
            speedMultiplier = 1.6;
        }
        public override void CheckDamage(Class c)
        {
            
        }
    }
}
