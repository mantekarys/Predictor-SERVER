using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal class AttackSpeedCreator : PickUpCreator
    {
        public override Item createItem() 
        { 
            return new AttackSpeedPotion();
        }

        public override PowerUp createPowerUp() 
        { 
            return new AttackSpeedPowerUp();
        }
    }
}
