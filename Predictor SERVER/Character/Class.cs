using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;
using System.Globalization;
using Predictor_SERVER.Map;

namespace Predictor_SERVER.Character
{
    public class Class : Character
    {
        private List<Item> inventory = new List<Item>();
        private List<Item> activeItems = new List<Item>();
        private Dictionary<string, int> upgrades = new Dictionary<string, int>();
        public WeaponAlgorithm weapon;
        public DateTime lastAttack; 
        class Message
        {
            List<string> buttons;
        }
        public Class(int size, int speed, int health, int damage, int x,int y)
        {
            this.size = size;
            this.speed = speed;
            this.health = health;
            this.damage = damage;
            this.coordinates = (x, y);
            lastAttack = DateTime.MinValue;
            //send other parameters on start or when took an upgrade
        }
        public Class() { }
        /// <summary>
        /// Applies power up to playable character
        /// </summary>
        /// <param name="powerUp">Provided will be children of PowerUP class</param>
        public void applyPowerUp(PowerUp powerUp)
        {
            Console.WriteLine("Current damage {0}",this.damage);
            if(this.upgrades.ContainsKey(powerUp.GetType().Name))
            {
                int value = this.upgrades[powerUp.GetType().ToString()];
                value = this.upgrades[powerUp.GetType().ToString()] +1;
                this.upgrades[powerUp.GetType().ToString()]=value;
            }
            else
            {
                this.upgrades.Add(powerUp.GetType().ToString(), 1); 
            }
            switch (powerUp.GetType().Name)
            {
                case "SpeedPowerUp":
                    this.speed += 20;
                    break;
                case "DamagePowerUp":
                    this.damage += 20;
                    break;
/*              case "AttackSpeedPowerUp":
                    this.attackspeed += 20;
                    break;
*/
                default:
                    break;
            }
            Console.WriteLine("After damage {0}", this.damage);

        }
        /// <summary>
        /// Adds items to inventory
        /// </summary>
        /// <param name="item"> Item class's children </param>
        public void addToInventory(Item item)
        {
            this.inventory.Add(item);
        }
        /// <summary>
        /// Method for apllying item for temporarly stat boost
        /// </summary>
        public void useItem(int position)
        {
            if (inventory[position] != null)
            {
                Item tempItem = inventory[position];
                tempItem.remainingTime = DateTime.Now.AddSeconds(tempItem.experationTime);
                this.activeItems.Add(tempItem);
                inventory.RemoveAt(position);
                switch (tempItem.GetType().Name)
                {
                    case "SpeedPotion":
                        this.speed += 50;
                        break;
                    case "DamagePotion":
                        this.damage += 50;
                        break;
/*                  case "AttackSpeedPotion":
                        this.attackspeed += 50;
                        break;
*/
                    default:
                        break;

                }
            }
        }
        /// <summary>
        /// Method for returning latest activated item back to inventory and removing their buff
        /// </summary>
        public void undoActiveItem()
        {
            if (activeItems.Count != 0)
            {
                var lastItem = activeItems.Count - 1;
                Item activeItem = activeItems[lastItem];
                activeItem.remainingTime = DateTime.MinValue;
                activeItems.RemoveAt(lastItem);
                switch (activeItem.GetType().Name)
                {
                    case "SpeedPotion":
                        this.speed -= 50;
                        break;
                    case "DamagePotion":
                        this.damage -= 50;
                        break;
/*                  case "AttackSpeedPotion":
                        this.attackspeed -= 50;
                        break;
*/
                    default:
                        break;

                }
                this.addToInventory(activeItem);
            }
        }
        /// <summary>
        /// Method to get the amount of intems held in inventory
        /// </summary>
        /// <returns></returns>
        public int inventoryCheck()
        {
            return this.inventory.Count();
        }
        /// <summary>
        /// Method for checking whether the item has expired and removes buffs from stats 
        /// </summary>
        public void activeItemTimeExperationCheck()
        {
            foreach(var item in activeItems)
            {
                if(item.remainingTime > DateTime.Now)
                {
                    string type = item.GetType().Name;
                    switch (type)
                    {
                        case "SpeedPotion":
                            this.speed -= 50;
                            break;
                        case "DamagePotion":
                            this.damage -= 50;
                            break ;
/*                      case "AttackSpeedPotion":
                            this.attackspeed -= 50;
                            break;
*/                      default:
                            break;
                            
                    }
                    activeItems.Remove(item);
                }
            }
        }
        public override void move()//event
        {
            using (WebSocket ws = new WebSocket("ws://127.0.0.1:7890/EchoAll"))
            {

                ws.Connect();

                //var mes = JsonConvert.SerializeObject<Message>(e.Data);
                //ws.Send(mes);
            }
        }

        public void setWeaponAlgorithm(WeaponAlgorithm weapon)
        {
            this.weapon = weapon;
        }

        public Projectile attack(int direction)
        {
            lastAttack = DateTime.Now;
            return weapon.attack(this, direction);
        }
    }
}
