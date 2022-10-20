using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Predictor_SERVER.Map;

namespace Predictor_SERVER.Character
{
    public class Npc : Character, IPrototype
    {
        public Npc(int size, int speed, int health, int damage, int x, int y)
        {
            this.size = size;
            this.speed = speed;
            this.health = health;
            this.damage = damage;
            this.coordinates.Item1 = x;
            this.coordinates.Item2 = y;
            this.ability = new Ability(100, "Makes character faster", 50, "Speed");
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
        public Npc shallowCopy()
        {
            return (Npc)this.MemberwiseClone();
        }
        public Npc deepCopy()
        {
            Npc clone = (Npc)this.MemberwiseClone();
            clone.ability = new Ability(100,"Makes character faster",50,"Speed");
            return clone;
        }
        public void calculateAction(Random rnd, int matchId)
        {
            int dir = rnd.Next(0, 5);
            int pad = 5;
            int mSize = 700;

            var prev = this.coordinates;
            if (dir == 0)
            {
                if (this.coordinates.Item1 > this.speed + pad)
                {
                    this.coordinates.Item1 -= this.speed;
                }
                else
                {
                    this.coordinates.Item1 = pad;
                }
            }
            else if (dir == 1)
            {
                if (this.coordinates.Item1 + this.speed + this.size < mSize)
                {
                    this.coordinates.Item1 += this.speed;
                }
                else
                {
                    this.coordinates.Item1 = mSize - this.size + pad;
                }
            }
            if (dir == 2)
            {
                if (this.coordinates.Item2 > this.speed + pad)
                {
                    this.coordinates.Item2 -= this.speed;
                }
                else
                {
                    this.coordinates.Item2 = pad;
                }

            }
            else if (dir == 3)
            {
                if (this.coordinates.Item2 + this.speed + this.size < mSize)
                {
                    this.coordinates.Item2 += this.speed;
                }
                else
                {
                    this.coordinates.Item2 = mSize - this.size + pad;
                }
            }
            foreach (var obs in Variables.obstacles[matchId])
            {
                var k = obs.collision(prev, this.coordinates, this.size);
                var diff = (prev.Item1 - this.coordinates.Item1, prev.Item2 - this.coordinates.Item2);
                if (k != (-1, -1))
                {
                    if (diff.Item1 == 0)
                    {
                        this.coordinates.Item2 = k.Item2;
                    }
                    else
                    {
                        this.coordinates.Item1 = k.Item1;
                    }
                }
            }
            foreach (var player in Variables.matches[matchId].players)
            {
                var k = player.playerClass.collision(prev, this.coordinates, this.size);
                var diff = (prev.Item1 - this.coordinates.Item1, prev.Item2 - this.coordinates.Item2);
                if (k != (-1, -1))
                {
                    if (diff.Item1 == 0)
                    {
                        this.coordinates.Item2 = k.Item2;
                    }
                    else
                    {
                        this.coordinates.Item1 = k.Item1;
                    }
                    player.playerClass.takeDamage(this.damage);
                }
            }
        }
    }
}
