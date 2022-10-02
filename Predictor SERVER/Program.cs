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
using System.Timers;
using Timer = System.Timers.Timer;
using System.Threading;
using Predictor_SERVER.Server;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Reflection;

namespace Predictor_SERVER
{
    public static class Variables
    {
        public static List<List<Class>> classes = new List<List<Class>>(); //{ new Class(15, 10, 5, 1, 343, 10), new Class(15, 10, 5, 1, 343, 685) };
        //public static Class c1 = new Class(15, 10, 5, 1, 343, 10);//pradines coord pakeist nes paskui kai su walls buna keistai gal i speed?
        //public static Class c2 = new Class(15, 10, 5, 1, 343, 685);//kaska su tom class daryt
        public static List<MapObject> mapO = new List<MapObject>();
        public static Map.Map map = new Map.Map("Map1", mapO);
        public static int howMany = 0;
        public static WebSocketSessionManager sesions;
        public static bool started = false;

        public static List<Server.Match> matches = new List<Server.Match>();
        public static List<List<string>> matchIds = new List<List<string>>();

        public static void SendMessages(int matchId)
        {
            Variables.started = true;
            Timer newTimer = new Timer();
            newTimer.Elapsed += delegate { Broadcast(matchId); };//new ElapsedEventHandler(Broadcast,5);
            newTimer.Interval = 100;
            newTimer.Start();
        }
        public static void Broadcast(int matchId)//object sender, EventArgs e
        {
            var message = JsonConvert.SerializeObject((Variables.classes[matchId].ToList(), Variables.map));
            //foreach (var item in Variables.matchIds[matchId])
            //{
            //    sesions.SendTo(message, item);
            //}
            sesions.Broadcast(message);
        }

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
            if (e.Data.Length > 3)
            {
                int which = -1;
                int code;
                string text;
                int matchId = -1;
                int ready = 0;
                int count = e.Data.Count(x => x == ':');
                if (count == 2)
                {
                    (code, text) = JsonConvert.DeserializeObject<(int, string)>(e.Data);
                }
                else//redies up
                {
                    (code, matchId, text, ready, which) = JsonConvert.DeserializeObject<(int, int, string, int, int)>(e.Data);
                }



                if (code == 752)//creates match
                {
                    matchId = Variables.matches.Count;
                    //Variables.matchIds.Add(new List<string>());
                    //Variables.matchIds[matchId].Add(ID);
                    Variables.matches.Add(new Server.Match(matchId, text));
                    Variables.classes.Add(new List<Class>());
                    var message = JsonConvert.SerializeObject((matchId, Variables.matches[matchId].peopleAmount-1));
                    Send(message);
                }
                if (code == 876)////joined match
                {
                    matchId= int.Parse(text);
                    //Variables.matchIds[matchId].Add(ID);
                    var message = JsonConvert.SerializeObject(Variables.matches[matchId].peopleAmount++);
                    Send(message);
                }
                if (code == 545)//readies up
                {
                    Variables.matches[matchId].ready += ready;
                    Variables.classes[matchId].Add(ClassCreator.pickCreator(text, 15,which));
                    if (Variables.matches[matchId].ready == Variables.matches[matchId].peopleAmount)
                    {
                        var message = JsonConvert.SerializeObject((Variables.classes[matchId].ToList(), Variables.map));

                        if (!Variables.started)//gal istrint ta if ir cia apskritai siuncia vissiems o turetu tik tiems kuriu matchas
                        {

                            //ThreadStart childref = new ThreadStart(Variables.SendMessages);
                            //Thread childThread = new Thread(childref);
                            //childThread.Start();

                            var thread = new Thread(
                                () => Variables.SendMessages(matchId));
                            thread.Start();
                            Variables.sesions = Sessions;
                        }
                        //Send(message);
                        Sessions.Broadcast(message);
                    }
                    //else
                    //{
                    //    var message = JsonConvert.SerializeObject(-1);
                    //    Send(message);
                    //}
                }

            }
            if (e.Data == "159")//map start
            {
                Variables.classes.Add(new List<Class>() { new Class(15, 10, 5, 1, 343, 10), new Class(15, 10, 5, 1, 343, 685) });
                var message = JsonConvert.SerializeObject((Variables.classes[0], Variables.map, Variables.howMany++));
                if (Variables.howMany > 2)
                {
                    Variables.howMany = 0;
                }
                if (!Variables.started)
                {

                    //ThreadStart childref = new ParameterizedThreadStart(Variables.SendMessages);
                    //Thread childThread = new Thread(childref);
                    //childThread.Start(matchId);
                    //Variables.sesions = Sessions;

                    var thread = new Thread(
                         () => Variables.SendMessages(0));
                    thread.Start();
                    Variables.sesions = Sessions;
                }
                //Send(message);
            }
            if (e.Data == "999")//match list
            {
                List<(int, string)> matchIds = new List<(int, string)>();
                foreach (var item in Variables.matches)
                {
                    matchIds.Add((item.id, item.name));
                }
                
                var message = JsonConvert.SerializeObject(matchIds);
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
            int matchId;
            (keys, mouseClick, which, matchId) = JsonConvert.DeserializeObject<(HashSet<Keys>, MouseEventArgs mouseClickp, int, int)>(e.Data);


            var c = Variables.classes[matchId][which];//sita reik susirast kuris siunte ir galbut iskart dirbt su listu
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
                if (keyData == Keys.Up)
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
            Variables.classes[matchId][which] = c;

            var message = JsonConvert.SerializeObject((Variables.classes[matchId].ToList(), Variables.map));
            Variables.sesions = Sessions;
            
            var z = this.Context;
            //Sessions.Broadcast(message);

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
