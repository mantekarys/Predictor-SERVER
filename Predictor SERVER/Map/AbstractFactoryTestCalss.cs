using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    internal class AbstractFactoryTestCalss
    {
        private Item item = null;
        private PowerUp powerUp = null;
        private readonly string pickuptype = "I"; // potion = P , item = I
        private PickUpCreator _pickUpCreator = PickUpCreator.PickUpCreate("S"); // speed = S , Attackspeed = AS , damage = D
        if(pickuptype == "I" && _pickUpCreator!=null){
            _pickUpCreator.createItem();
        }
        else if(pickuptype == "P" && _pickUpCreator!=null){
            _pickUpCreator.createPowerUp();
        }
        else
        {
             Console.WriteLine("error chosen pickup doesn't exist");
        }

    }
}
