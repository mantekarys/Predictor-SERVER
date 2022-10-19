using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Predictor_SERVER.Map;

namespace Predictor_SERVER.Character
{
    public class Npc : Character
    {
        public Npc(int size, int speed, int health, int damage, int x, int y)
        {
            this.size = size;
            this.speed = speed;
            this.health = health;
            this.damage = damage;
            this.coordinates.Item1 = x;
            this.coordinates.Item2 = y;
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
