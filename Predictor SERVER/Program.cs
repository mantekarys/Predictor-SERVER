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
using System.Drawing;

namespace Predictor_SERVER
{
    public static class Variables
    {
        public static List<List<Class>> classes = new List<List<Class>>(); //{ new Class(15, 10, 5, 1, 343, 10), new Class(15, 10, 5, 1, 343, 685) };
        //public static Class c1 = new Class(15, 10, 5, 1, 343, 10);//pradines coord pakeist nes paskui kai su walls buna keistai gal i speed?
        //public static Class c2 = new Class(15, 10, 5, 1, 343, 685);//kaska su tom class daryt
        public static List<MapObject> mapO = new List<MapObject>();
        //public static List<List<MapObject>> mapObjects = new List<List<MapObject>>();
        public static List<List<Trap>> traps = new List<List<Trap>>();
        public static List<List<Obstacle>> obstacles = new List<List<Obstacle>>();
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
            newTimer.Interval = 20;
            newTimer.Start();
        }
        public static void Broadcast(int matchId)//object sender, EventArgs e
        {
            var message = JsonConvert.SerializeObject((Variables.classes[matchId].ToList(), Variables.map, Variables.traps[matchId].ToList(), Variables.obstacles[matchId].ToList()));
            foreach (var item in Variables.matchIds[matchId])
            {
                sesions.SendTo(message, item);
            }
            //sesions.Broadcast(message);
        }

        public static List<Obstacle> createObstacles()
        {
            List<Obstacle> matchObstacles = new List<Obstacle>();
            Random rnd = new Random(123);
            int obsCount = rnd.Next(1, 11);
            for (int i = 0; i < obsCount; i++)
            {
                Obstacle obstacle = new Obstacle(rnd.Next(5, 685), rnd.Next(5, 685), "Red");
                matchObstacles.Add(obstacle);   

            }
            return matchObstacles;
        }
        public static List<Trap> createTraps()
        {
            List<Trap> matchTraps = new List<Trap>(); 
            Random rnd = new Random(552);
            int trapCount = rnd.Next(5, 16);
            for (int i = 0; i < trapCount; i++)
            {
                Trap trap = new Trap(rnd.Next(5, 695), rnd.Next(5, 695), "Blue");
                matchTraps.Add(trap);   
            }
            return matchTraps;
        }

        //public static List<MapObject> combine(int matchId)
        //{
        //    List<MapObject> matchMapObjects = new List<MapObject>();
        //    matchMapObjects.AddRange(traps[matchId]);
        //    matchMapObjects.AddRange(obstacles[matchId]);
        //    return matchMapObjects;
        //}


    }
    public class Echo : WebSocketBehavior
    {

        //List<Class> classes = new List<Class>();
        //Class c1 = new Class(15, 10, 5, 1, 343, 10);//pradines coord pakeist nes paskui kai su walls buna keistai gal i speed?
        //Class c2 = new Class(15, 10, 5, 1, 343, 685);
        //public bool collision(int xCoord, int yCoord, List<Obstacle> obstacles)
        //{
        //    foreach (var obs in obstacles)
        //    {
        //        if (xCoord > obs.coordinates.Item1 && xCoord < obs.coordinates.Item1 + obs.size && yCoord > obs.coordinates.Item2 && yCoord < obs.coordinates.Item2 + obs.size)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public bool collision(int xCoord, int yCoord,int size, List<Obstacle> obstacles)
        //{
        //    RectangleF cRegion = new Rectangle(xCoord, yCoord, size, size);

        //    foreach (var obs in obstacles)
        //    {
        //        RectangleF obsRegion = new Rectangle(obs.coordinates.Item1, obs.coordinates.Item2, obs.size, obs.size);
        //        RectangleF intersectRectangleF = RectangleF.Intersect(cRegion, obsRegion);
        //        if(intersectRectangleF.Height != 0 || intersectRectangleF.Width != 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //public int collisionObstacle(int xCoord, int yCoord, int size, List<Obstacle> obstacles)
        //{
        //    RectangleF cRegion = new Rectangle(xCoord, yCoord, size, size);

        //    foreach (var obs in obstacles)
        //    {
        //        RectangleF obsRegion = new Rectangle(obs.coordinates.Item1, obs.coordinates.Item2, obs.size, obs.size);
        //        RectangleF intersectRectangleF = RectangleF.Intersect(cRegion, obsRegion);

        //        if (intersectRectangleF.Height != 0 || intersectRectangleF.Width != 0)
        //        {
        //            if (intersectRectangleF.Height > intersectRectangleF.Width)
        //            {
        //                return 1;
        //            }
        //            else if (intersectRectangleF.Width > intersectRectangleF.Height)
        //            {
        //                return 2;
        //            }
        //            else return 3;
        //        }
        //    }
        //    return 0;
        //}

        //public void collisionTrap(int xCoord, int yCoord, int size, List<Trap> trap, int matchId)
        //{
        //    RectangleF cRegion = new Rectangle(xCoord, yCoord, size, size);

        //    for (int i = 0; i < trap.Count; i++)

        //    {
        //        RectangleF trpRegion = new Rectangle(trap[i].coordinates.Item1, trap[i].coordinates.Item2, trap[i].size, trap[i].size);
        //        RectangleF intersectRectangleF = RectangleF.Intersect(cRegion, trpRegion);

        //        if (intersectRectangleF.Height != 0 || intersectRectangleF.Width != 0)
        //        {
        //            Variables.traps[matchId].RemoveAt(i);// add line to explode/deal damage;
        //            //c.takeDamage(trap[i].damage);
        //        }
        //    }
        //}
        public Echo()
        {

        }
        protected override void OnMessage(MessageEventArgs e)
        {
                
            if (e.Data.Length > 3)
            {
                int which = -1;
                int code = -1;
                string text = "";
                int matchId = -1;
                int ready = 0;

                HashSet<Keys> keys = new HashSet<Keys>();
                MouseEventArgs mouseClick;
                var map = Variables.map;

                int count = e.Data.Count(x => x == ':');
                if (count == 2)
                {
                    (code, text) = JsonConvert.DeserializeObject<(int, string)>(e.Data);
                }
                else if (count == 5)//redies up
                {
                    (code, matchId, text, ready, which) = JsonConvert.DeserializeObject<(int, int, string, int, int)>(e.Data);
                }
                else //gets game info
                {
                    (keys, mouseClick, which, matchId) = JsonConvert.DeserializeObject<(HashSet<Keys>, MouseEventArgs mouseClickp, int, int)>(e.Data);
                }



                if (code == 752)//creates match
                {
                    matchId = Variables.matches.Count;
                    Variables.matchIds.Add(new List<string>());//
                    Variables.matchIds[matchId].Add(ID);//
                    Variables.matches.Add(new Server.Match(matchId, text));
                    Variables.classes.Add(new List<Class>());
                    Variables.traps.Add(new List<Trap>());
                    Variables.obstacles.Add(new List<Obstacle>());
                    Variables.traps[matchId] = (Variables.createTraps());
                    Variables.obstacles[matchId] = (Variables.createObstacles());
                    //List<MapObject> temp = new List<MapObject>();
                    //temp.AddRange(Variables.traps[matchId]);
                    //temp.AddRange(Variables.obstacles[matchId]);

                    //Variables.mapObjects[matchId] = (Variables.combine(matchId));



                    var message = JsonConvert.SerializeObject((matchId, Variables.matches[matchId].peopleAmount-1));
                    Send(message);
                }
                if (code == 876)////joined match
                {
                    matchId= int.Parse(text);
                    Variables.matchIds[matchId].Add(ID);//
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

                        var thread = new Thread(
                                () => Variables.SendMessages(matchId));
                        thread.Start();
                        Variables.sesions = Sessions;
                        //Send(message);
                        Sessions.Broadcast(message);
                    }
                    
                }
                if (code == -1)////game movement
                {


                    var c = Variables.classes[matchId][which];//sita reik susirast kuris siunte ir galbut iskart dirbt su listu

                    var obstacles = Variables.obstacles[matchId];
                    int pad = 5;
                    foreach (var keyData in keys)
                    {
                        var tempC = c.coordinates;
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
                            if(c.coordinates.Item1 + c.speed + c.size < map.size)
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
                            if(c.coordinates.Item2 > c.speed)
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
                            if(c.coordinates.Item2 + c.speed + c.size < map.size)
                            {
                                c.coordinates.Item2 += c.speed;
                            }
                            else
                            {
                                c.coordinates.Item2 = map.size - c.size + 5;
                            }
                        }
                        foreach (var obs in obstacles)
                        {
                            var k = obs.collision(tempC, c.coordinates, c.size);
                            var diff = (tempC.Item1 - c.coordinates.Item1, tempC.Item2 - c.coordinates.Item2) ;
                            if (k != (-1, -1))
                            {
                                if (diff.Item1 == 0)
                                {
                                    c.coordinates.Item2 = k.Item2;
                                }
                                else
                                {
                                    c.coordinates.Item1 = k.Item1;
                                } 
                            }
                        }
                        for (int i = 0; i < Variables.traps[matchId].Count; i++)
                        {
                            if (Variables.traps[matchId][i].collision(tempC, c.coordinates, c.size))
                            {
                                Variables.traps[matchId].RemoveAt(i);
                            }
                        } 


                    }
                    //var cllsn = collisionObstacle(c.coordinates.Item1, c.coordinates.Item2, c.size, obstacles);
                    //collisionTrap(c.coordinates.Item1, c.coordinates.Item2, c.size, Variables.traps[matchId], matchId);
                    Variables.classes[matchId][which] = c;
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

            //var message = JsonConvert.SerializeObject((Variables.classes[matchId].ToList(), Variables.map));

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
