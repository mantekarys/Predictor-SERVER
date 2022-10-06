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
        private string pickuptype = "";
        private string creatortype= "";
        private PickUpCreator _pickUpCreator = null;
        public void Tester()
        {
            _pickUpCreator = PickUpCreator.PickUpCreate(creatortype); 
            switch (pickuptype)
            {
                case ("I"):
                    item = _pickUpCreator.createItem();
                    break;
                case ("P"):
                    powerUp = _pickUpCreator.createPowerUp();
                    break;
                default:
                    Console.WriteLine("error chosen pickup doesn't exist");
                    break;

            }
        }

        public AbstractFactoryTestCalss(string creator, string type)
        {
            pickuptype = type;
            creatortype = creator;

        }
    }
}
