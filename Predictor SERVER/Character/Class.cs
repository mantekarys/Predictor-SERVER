using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp;
using Newtonsoft.Json;

namespace Predictor_SERVER.Character
{
    public class Class : Character
    {
        public WeaponAlgorithm weapon;
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

            //send other parameters on start or when took an upgrade
        }
        public Class() { }

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
    }
}
