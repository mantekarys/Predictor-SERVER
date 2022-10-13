using Newtonsoft.Json;
using Predictor_SERVER.Character;
using Predictor_SERVER.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using Timer = System.Timers.Timer;

namespace Predictor_SERVER
{
    public static class Variables
    {
        public static List<List<Class>> classes = new List<List<Class>>(); //{ new Class(15, 10, 5, 1, 343, 10), new Class(15, 10, 5, 1, 343, 685) };
        public static List<MapObject> mapO = new List<MapObject>();
        public static List<List<Trap>> traps = new List<List<Trap>>();
        public static List<List<Obstacle>> obstacles = new List<List<Obstacle>>();
        public static Map.Map map = new Map.Map("Map1", mapO);
        public static int howMany = 0;
        public static WebSocketSessionManager sesions;
        public static bool started = false;
        public static List<List<PickUp>> pickables = new List<List<PickUp>>();
        public static List<List<Projectile>> projectiles = new List<List<Projectile>>();
        public static List<Server.Match> matches = new List<Server.Match>();
        public static List<List<string>> matchIds = new List<List<string>>();

        public static void SendMessages(int matchId)
        {
            Variables.started = true;
            Timer newTimer = new Timer();
            newTimer.Elapsed += delegate { Broadcast(matchId); };
            newTimer.Interval = 20;
            newTimer.Start();
        }
        public static void Broadcast(int matchId)
        {
            foreach (var projectile in projectiles[matchId].ToList())
            {
                if (projectile == null) continue;
                RectangleF r = new Rectangle();
                var last = projectile.coordinates;
                var current = projectile.move();
                var dif = (last.Item1 - current.Item1, last.Item2 - current.Item2);
                if (current.Item1 > 700 || current.Item2 > 700 || current.Item1 < 5 || current.Item2 < 5)
                {
                    try
                    {
                        projectiles[matchId].Remove(projectile);
                    }
                    catch (Exception) { }
                }

            }
            var message = JsonConvert.SerializeObject((Variables.classes[matchId].ToList(), Variables.map, Variables.pickables[matchId].ToList(), Variables.projectiles[matchId].ToList(), Variables.traps[matchId].ToList(), Variables.obstacles[matchId].ToList()));
            foreach (var item in Variables.matchIds[matchId])
            {
                sesions.SendTo(message, item);
            }
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
    }
    public class Echo : WebSocketBehavior
    {
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
                (int, int) mouse = (0, 0);
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
                    (keys, mouse, which, matchId) = JsonConvert.DeserializeObject<(HashSet<Keys>, (int, int), int, int)>(e.Data);
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
                    Variables.traps[matchId] = Variables.createTraps();
                    Variables.obstacles[matchId] = Variables.createObstacles();
                    Variables.pickables.Add(new List<PickUp>() { new DamagePotion((350, 350)) });
                    Variables.projectiles.Add(new List<Projectile>());
                    var message = JsonConvert.SerializeObject((matchId, Variables.matches[matchId].peopleAmount - 1));
                    Send(message);
                }
                if (code == 876)////joined match
                {
                    matchId = int.Parse(text);
                    Variables.matchIds[matchId].Add(ID);
                    var message = JsonConvert.SerializeObject(Variables.matches[matchId].peopleAmount++);
                    Send(message);
                }
                if (code == 545)//readies up
                {
                    Variables.matches[matchId].ready += ready;
                    Variables.classes[matchId].Add(ClassCreator.pickCreator(text, 15, which));
                    if (Variables.matches[matchId].ready == Variables.matches[matchId].peopleAmount)
                    {
                        var thread = new Thread(
                                () => Variables.SendMessages(matchId));
                        thread.Start();
                        Variables.sesions = Sessions;
                    }

                }
                if (code == -1)////game movement
                {
                    var c = Variables.classes[matchId][which];
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
                        foreach (var obs in obstacles)
                        {
                            var k = obs.collision(tempC, c.coordinates, c.size);
                            var diff = (tempC.Item1 - c.coordinates.Item1, tempC.Item2 - c.coordinates.Item2);
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


                        if (keyData == Keys.LButton)
                        {
                            if (DateTime.Now.Ticks / 10000 - c.lastAttack.Ticks / 10000 > c.weapon.attackSpeed)
                            {
                                int direction;
                                int x = mouse.Item1 - c.coordinates.Item1;
                                int y = mouse.Item2 - c.coordinates.Item2;
                                if (Math.Abs(x) > Math.Abs(y))
                                {
                                    if (x > 0)
                                    {
                                        direction = 1;
                                    }
                                    else
                                    {
                                        direction = 3;
                                    }
                                }
                                else
                                {
                                    if (y > 0)
                                    {
                                        direction = 2;
                                    }
                                    else
                                    {
                                        direction = 0;
                                    }
                                }
                                Variables.projectiles[matchId].Add(c.attack(direction));
                            }
                        }
                    }
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
                    var thread = new Thread(
                         () => Variables.SendMessages(0));
                    thread.Start();
                    Variables.sesions = Sessions;
                }
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
    class Program
    {
        static void Main(string[] args)
        {
            WebSocketServer wssv = new WebSocketServer("ws://127.0.0.1:7890");
            wssv.AddWebSocketService<Echo>("/Echo");
            wssv.Start();
            Console.WriteLine("WS server started on ws://127.0.0.1:7890/Echo");

            Console.ReadKey();
            wssv.Stop();
        }
    }
}
