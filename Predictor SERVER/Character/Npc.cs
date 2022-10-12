using System;
using System.Collections.Generic;
using System.Linq;
using Predictor_SERVER.Map;

namespace Predictor_SERVER.Character
{
    internal class Npc : Character
    {
        public Npc()
        {
        }

        public override void move()
        {
            throw new NotImplementedException();
        }
        public PickUp OnDeath()
        {
            Random random = new Random();   
            PickUp drop = null;
            PickUpCreator _pickUpCreator = PickUpCreator.PickUpCreate(random.Next(3));
            switch (random.Next(2))
            {
                case (0):
                    drop = _pickUpCreator.createItem(this.coordinates);
                    break;
                case (1):
                    drop = _pickUpCreator.createPowerUp(this.coordinates);
                    break;
                default:
                    Console.WriteLine("error message needed");
                    break;

            }
            return drop;
        }
    }
}
