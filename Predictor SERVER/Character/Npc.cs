using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Predictor_SERVER.Map;

namespace Predictor_SERVER.Character
{
    public class Npc : Character
    {
        Dictionary<string, NpcMovement> movementTypes = new Dictionary<string, NpcMovement>();
        Dictionary<string, NpcDeath> deathTypes = new Dictionary<string, NpcDeath>();
        public Npc(int size, int speed, int health, int damage, int x, int y)
        {
            this.size = size;
            this.speed = speed;
            this.health = health;
            this.damage = damage;
            this.coordinates.Item1 = x;
            this.coordinates.Item2 = y;
            this.ability = new Ability(100, "Makes character faster", 50, "Speed");

            this.movementTypes["random"] = new NpcRandomType();
            this.movementTypes["circle"] = new NpcCircleType();

            this.deathTypes["item"] = new NpcDeathItemType();
            this.deathTypes["powerUp"] = new NpcDeathPowerUpType();
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
            clone.movementTypes = new Dictionary<string, NpcMovement>() { { "random", new NpcRandomType() },{ "circle" , new NpcCircleType() } };
            clone.deathTypes = new Dictionary<string, NpcDeath>() { { "item", new NpcDeathItemType() }, { "powerUp", new NpcDeathPowerUpType() } };
            return clone;
        }
        public void calculateAction(string moveType, int matchId)
        {
            this.movementTypes[moveType].move(matchId, this);
        }
        public void selectDeathType(string deathType)
        {
            this.deathTypes[deathType].onDeath(this.coordinates);
        }
    }
}
