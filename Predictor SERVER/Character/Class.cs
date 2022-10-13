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
        public void ApplyPowerUp(PowerUp powerUp)
        {
            int value = this.upgrades[powerUp.GetType().ToString()];
            if(this.upgrades.ContainsKey(powerUp.GetType().ToString()))
            {
                value = this.upgrades[powerUp.GetType().ToString()] +1;
                this.upgrades[powerUp.GetType().ToString()]=value;
            }
            else
            {
                this.upgrades.Add(powerUp.GetType().ToString(), 1); 
            }
        }
        /// <summary>
        /// Adds items to inventory
        /// </summary>
        /// <param name="item"> Item class's children </param>
        public void AddToInventory(Item item)
        {
            this.inventory.Add(item);
        }
        public void UseItem()
        {

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
