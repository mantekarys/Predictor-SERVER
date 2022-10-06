using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal abstract class PickUpCreator
    {
        public abstract Item createItem();

        public abstract PowerUp createPowerUp();

        public static PickUpCreator PickUpCreate(string type)
        {
            switch (type)
            {
                case "S":
                    return new SpeedCreator();
                case "AS":
                    return new AttackSpeedCreator();
                case "D":
                    return new DamageCreator();
                default:
                    return null;
            }
                
        }
       
    }
}
