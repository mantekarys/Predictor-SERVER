using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal abstract class PickUpCreator
    {
        public abstract Item createItem((int,int)cord);

        public abstract PowerUp createPowerUp((int, int) cord);

        public static PickUpCreator PickUpCreate(int type)
        {
            switch (type)
            {
                case 0:
                    return new SpeedCreator();
                case 1:
                    return new AttackSpeedCreator();
                case 2:
                    return new DamageCreator();
                default:
                    return null;
            }
                
        }
       
    }
}
