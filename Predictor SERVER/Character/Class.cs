using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;
using System.Globalization;
using Predictor_SERVER.Map;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq.Expressions;

namespace Predictor_SERVER.Character
{
    public class Class : Character
    {
        private List<Item> inventory = new List<Item>();
        private List<Item> activeItems = new List<Item>();
        private Dictionary<string, int> upgrades = new Dictionary<string, int>();
        public WeaponAlgorithm weapon;
        public DateTime lastAttack;
        public HealthState state;

        internal Class Clone()
        {
            Class clone = (Class)this.MemberwiseClone();
            clone.weapon = weapon.Clone();
            clone.inventory=new List<Item>();
            clone.activeItems = new List<Item>();
            clone.upgrades=new Dictionary<string, int>();
            clone.state = new HealthFull(this.health);
            return clone;

        }

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
            state = new HealthFull(this.health);
        }
        public Class() { }
        #region PowerUps and Items
        /// <summary>
        /// Applies power up to playable character
        /// </summary>
        /// <param name="powerUp">Provided will be children of PowerUP class</param>
        public void applyPowerUp(PowerUp powerUp)
        {
            if(this.upgrades.ContainsKey(powerUp.GetType().Name))
            {
                int value = this.upgrades[powerUp.GetType().Name];
                value = this.upgrades[powerUp.GetType().Name] +1;
                this.upgrades[powerUp.GetType().Name]=value;
            }
            else
            {
                this.upgrades.Add(powerUp.GetType().Name, 1); 
            }
            switch (powerUp.GetType().Name)
            {
                case "SpeedPowerUp":
                    this.speed += 20;
                    break;
                case "DamagePowerUp":
                    this.damage += 20;
                    break;
             case "AttackSpeedPowerUp":
                    this.weapon.attackSpeed = (int)(this.weapon.attackSpeed * 0.8);
                    if (this.weapon.attackSpeed < 1) this.weapon.attackSpeed=1;
                    break;

                default:
                    break;
            }

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
            if (this.inventoryCheck()>0) {
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
                            Console.WriteLine("Current damage {0}", this.damage);
                            this.damage += 50;
                            Console.WriteLine("After damage {0}", this.damage);
                            break;
                        case "AttackSpeedPotion":
                            this.weapon.attackSpeed = (int)(this.weapon.attackSpeed * 0.8);
                            if (this.weapon.attackSpeed < 1) this.weapon.attackSpeed = 1;
                            break;

                        default:
                            break;

                    }
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
                    case "AttackSpeedPotion":
                        this.weapon.attackSpeed = (int)(this.weapon.attackSpeed * 1.25);
                        break;

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
                        case "AttackSpeedPotion":
                            this.weapon.attackSpeed = (int)(this.weapon.attackSpeed * 1.25);
                            break;
                        default:
                            break;
                            
                    }
                    activeItems.Remove(item);
                }
            }
        }

        #endregion PowerUps and Items
        public void move(Keys keyData, Map.Map map)//event
        {
            int pad = 5;
            if (keyData == Keys.Left)
            {
                if (this.coordinates.Item1 > getSpeed() + pad)
                {
                    this.coordinates.Item1 -= getSpeed();
                }
                else
                {
                    this.coordinates.Item1 = pad;
                }
            }
            else if (keyData == Keys.Right)
            {
                if (this.coordinates.Item1 + getSpeed() + this.size < map.size)
                {
                    this.coordinates.Item1 += getSpeed();
                }
                else
                {
                    this.coordinates.Item1 = map.size - this.size + 5;
                }
            }
            if (keyData == Keys.Up)
            {
                if (this.coordinates.Item2 > getSpeed() + pad)
                {
                    this.coordinates.Item2 -= getSpeed();
                }
                else
                {
                    this.coordinates.Item2 = pad;
                }

            }
            else if (keyData == Keys.Down)
            {
                if (this.coordinates.Item2 + getSpeed() + this.size < map.size)
                {
                    this.coordinates.Item2 += getSpeed();
                }
                else
                {
                    this.coordinates.Item2 = map.size - this.size + 5;
                }
            }
        }
        public override void move() { }

        public void setWeaponAlgorithm(WeaponAlgorithm weapon)
        {
            this.weapon = weapon;
        }

        public Projectile attack(int direction)
        {
            lastAttack = DateTime.Now;
            return weapon.attack(this, direction);
        }

        public new bool takeDamage(int damage)
        {
            this.health -= damage;
            state.CheckDamage(this);
            Console.WriteLine("Health: {0}", health);
            Console.WriteLine("State: {0}", state.GetType());
            return this.health <= 0;
        }
        public int getDamage()
        {
            return (int)Math.Round(damage * state.damageMultiplier);
        }
        public int getSpeed()
        {
            return (int)Math.Round(speed * state.speedMultiplier);
        }
        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
