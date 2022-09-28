using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

using Predictor_SERVER.Character;
using Predictor_SERVER.Map;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

namespace Predictor_SERVER
{
    public static class Variables
    {
        public static List<Class> classes = new List<Class>() { new Class(15, 10, 5, 1, 343, 10), new Class(15, 10, 5, 1, 343, 685) };
        public static Class c1 = new Class(15, 10, 5, 1, 343, 10);//pradines coord pakeist nes paskui kai su walls buna keistai gal i speed?
        public static Class c2 = new Class(15, 10, 5, 1, 343, 685);//kaska su tom class daryt
        public static List<MapObject> mapO = new List<MapObject>();
        public static Map.Map map = new Map.Map("Map1", mapO);
        public static int howMany = 0;

        //public static Class GetCharacter(int which) { return classes[which]; }

    }
    public class Echo : WebSocketBehavior
    {

        //List<Class> classes = new List<Class>();
        //Class c1 = new Class(15, 10, 5, 1, 343, 10);//pradines coord pakeist nes paskui kai su walls buna keistai gal i speed?
        //Class c2 = new Class(15, 10, 5, 1, 343, 685);


        public Echo()
        {

        }
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "159")
            {

                List<MapObject> mapO = new List<MapObject>();
                var message = JsonConvert.SerializeObject((Variables.classes, Variables.map, Variables.howMany++));
                if (Variables.howMany > 2)
                {
                    Variables.howMany = 0;
                }
                Send(message);
            }
     

        }
    }


    public class EchoAll : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            HashSet<Keys> keys = new HashSet<Keys>();
            MouseEventArgs mouseClick;
            int which;
            var map = Variables.map;
            (keys, mouseClick, which) = JsonConvert.DeserializeObject<(HashSet<Keys>, MouseEventArgs mouseClickp, int)>(e.Data);


            var c = Variables.classes[which];//sita reik susirast kuris siunte ir galbut iskart dirbt su listu
            int pad = 5;
            foreach (var keyData in keys)
            {
                if (keyData == Keys.Left)
                {
                    if (c.coordinates.Item1 > c.speed)
                    {
                        c.coordinates.Item1 -= c.speed;
                    }
                    else
                    {
                        c.coordinates.Item1 = pad;
                    }
                }
                else if (keyData == Keys.Right)
                {
                    if (c.coordinates.Item1 + c.speed + c.size < map.size)
                    {
                        c.coordinates.Item1 += c.speed;
                    }
                    else
                    {
                        c.coordinates.Item1 = map.size - c.size + 5;
                    }
                }
                else if (keyData == Keys.Up)
                {
                    if (c.coordinates.Item2 > c.speed)
                    {
                        c.coordinates.Item2 -= c.speed;
                    }
                    else
                    {
                        c.coordinates.Item2 = pad;
                    }

                }
                else if (keyData == Keys.Down)
                {
                    if (c.coordinates.Item2 + c.speed + c.size < map.size)
                    {
                        c.coordinates.Item2 += c.speed;
                    }
                    else
                    {
                        c.coordinates.Item2 = map.size - c.size + 5;
                    }
                }
            }
            Variables.classes[which] = c;

            List<MapObject> mapO = new List<MapObject>();
            map = new Map.Map("Map1", mapO);
            var message = JsonConvert.SerializeObject((Variables.classes.ToList(), map));
            Sessions.Broadcast(message);

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Class> classes = new List<Class>();
            Class c1 = new Class(15, 10, 5, 1, 343, 10);//pradines coord pakeist nes paskui kai su walls buna keistai gal i speed?
            Class c2 = new Class(15, 10, 5, 1, 343, 685);
            classes.Add(c1);
            classes.Add(c2);
            var message = JsonConvert.SerializeObject((classes, 0));


            WebSocketServer wssv = new WebSocketServer("ws://127.0.0.1:7890");

            wssv.AddWebSocketService<Echo>("/Echo");
            wssv.AddWebSocketService<EchoAll>("/EchoAll");
            

            wssv.Start();
            Console.WriteLine("WS server started on ws://127.0.0.1:7890/Echo");
            Console.WriteLine("WS server started on ws://127.0.0.1:7890/EchoAll");

            Console.ReadKey();
            wssv.Stop();
        }
    }
}
