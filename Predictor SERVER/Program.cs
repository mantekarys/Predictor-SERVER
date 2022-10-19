using Newtonsoft.Json;
using Predictor_SERVER.Character;
using Predictor_SERVER.Map;
using Predictor_SERVER.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using Timer = System.Timers.Timer;

namespace Predictor_SERVER
{
    public static class Variables
    {
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


        public static List<int> moveNpc = new List<int>();
        public static List<List<Npc>> npcs = new List<List<Npc>>();

        public static List<Obstacle> createObstacles()
        {
            List<Obstacle> matchObstacles = new List<Obstacle>();
            Random rnd = new Random(645);
            int obsCount = rnd.Next(1, 20);
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
            Random rnd = new Random(978);
            int trapCount = rnd.Next(5, 16);
            for (int i = 0; i < trapCount; i++)
            {
                Trap trap = new Trap(rnd.Next(5, 695), rnd.Next(5, 695), "Blue");
                matchTraps.Add(trap);
            }
            return matchTraps;
        }
    }
    public class Echo : WebSocketBehavior, ISubject
    {
        Random rnd = new Random(978);
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
                    Variables.matchIds.Add(new List<string>());
                    Variables.matchIds[matchId].Add(ID);
                    Variables.matches.Add(new Server.Match(matchId, text));
                    Variables.traps.Add(new List<Trap>());
                    Variables.obstacles.Add(new List<Obstacle>());
                    Variables.traps[matchId] = Variables.createTraps();
                    Variables.obstacles[matchId] = Variables.createObstacles();
                    Variables.pickables.Add(new List<PickUp>() { new DamagePotion((350, 350)) });
                    Variables.projectiles.Add(new List<Projectile>());

                    Variables.npcs.Add(new List<Npc>() { new Npc(15, 5, 5, 1, 30, 30), new Npc(15, 5, 5, 1, 655, 30), new Npc(15, 5, 5, 1, 30, 655), new Npc(15, 5, 5, 1, 655, 655) });
                    Variables.moveNpc.Add(0);
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
                    Variables.matches[matchId].players.Add(new Server.Player(ClassCreator.pickCreator(text, 15, which)));
                    if (Variables.matches[matchId].ready == Variables.matches[matchId].peopleAmount)
                    {
                        var thread = new Thread(
                                () => SendMessages(matchId));
                        thread.Start();
                        Variables.sesions = Sessions;
                    }

                }
                if (code == -1)////game movement
                {
                    var c = Variables.matches[matchId].players[which].playerClass;
                    var obstacles = Variables.obstacles[matchId];
                    int pad = 5;
                    foreach (var keyData in keys)
                    {
                        var tempC = c.coordinates;
                        if (keyData == Keys.Left)
                        {
                            if (c.coordinates.Item1 > c.speed + pad)
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
                            if (c.coordinates.Item2 > c.speed + pad)
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
                        foreach (var npc in Variables.npcs[matchId])
                        {
                            var k = npc.collision(tempC, c.coordinates, c.size);
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
                                c.takeDamage(npc.damage);
                            }
                        }

                        for (int i = 0; i < Variables.traps[matchId].Count; i++)
                        {
                            if (Variables.traps[matchId][i].collision(tempC, c.coordinates, c.size))
                            {
                                c.takeDamage(Variables.traps[matchId][i].damage);
                                Variables.traps[matchId].RemoveAt(i);
                            }
                        }
                        int num = 0;
                        foreach (var player in Variables.matches[matchId].players)
                        {
                            if (num++ == which) continue;
                            var k = player.playerClass.collision(tempC, c.coordinates, c.size);
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
                    Variables.matches[matchId].players[which].playerClass = c;
                }
            }
            if (e.Data == "159")//map start
            {
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

        public void SendMessages(int matchId)
        {
            Variables.started = true;
            Timer newTimer = new Timer();
            newTimer.Elapsed += delegate { Broadcast(matchId); };
            newTimer.Interval = 20;
            newTimer.Start();
        }
        public void Broadcast(int matchId)
        {
            BulletMovement(matchId);
            if (Variables.moveNpc[matchId]++ == 4)
            {
                NpcMovement(matchId);
                Variables.moveNpc[matchId] = 0;
            }
            var message = JsonConvert.SerializeObject((Variables.matches[matchId].players.ToList(), Variables.map,
                Variables.pickables[matchId].ToList(), Variables.projectiles[matchId].ToList(), Variables.traps[matchId].ToList(),
                Variables.obstacles[matchId].ToList(), Variables.npcs[matchId].ToList()));
            foreach (var item in Variables.matchIds[matchId])
            {
                Sessions.SendTo(message, item);
            }
        }
        private void NpcMovement(int matchId)
        {
            foreach (var npc in Variables.npcs[matchId])
            {           
                int dir = rnd.Next(0, 5);
                int pad = 5;
                int mSize = 700;

                var prev = npc.coordinates;
                if (dir == 0)
                {
                    if (npc.coordinates.Item1 > npc.speed + pad)
                    {
                        npc.coordinates.Item1 -= npc.speed;
                    }
                    else
                    {
                        npc.coordinates.Item1 = pad;
                    }
                }
                else if (dir == 1)
                {
                    if (npc.coordinates.Item1 + npc.speed + npc.size < mSize)
                    {
                        npc.coordinates.Item1 += npc.speed;
                    }
                    else
                    {
                        npc.coordinates.Item1 = mSize - npc.size + pad;
                    }
                }
                if (dir == 2)
                {
                    if (npc.coordinates.Item2 > npc.speed + pad)
                    {
                        npc.coordinates.Item2 -= npc.speed;
                    }
                    else
                    {
                        npc.coordinates.Item2 = pad;
                    }

                }
                else if (dir == 3)
                {
                    if (npc.coordinates.Item2 + npc.speed + npc.size < mSize)
                    {
                        npc.coordinates.Item2 += npc.speed;
                    }
                    else
                    {
                        npc.coordinates.Item2 = mSize - npc.size + pad;
                    }
                }

                foreach (var obs in Variables.obstacles[matchId])
                {
                    var k = obs.collision(prev, npc.coordinates, npc.size);
                    var diff = (prev.Item1 - npc.coordinates.Item1, prev.Item2 - npc.coordinates.Item2);
                    if (k != (-1, -1))
                    {
                        if (diff.Item1 == 0)
                        {
                            npc.coordinates.Item2 = k.Item2;
                        }
                        else
                        {
                            npc.coordinates.Item1 = k.Item1;
                        }
                    }
                }
                foreach (var player in Variables.matches[matchId].players)
                {
                    var k = player.playerClass.collision(prev, npc.coordinates, npc.size);
                    var diff = (prev.Item1 - npc.coordinates.Item1, prev.Item2 - npc.coordinates.Item2);
                    if (k != (-1, -1))
                    {
                        if (diff.Item1 == 0)
                        {
                            npc.coordinates.Item2 = k.Item2;
                        }
                        else
                        {
                            npc.coordinates.Item1 = k.Item1;
                        }
                        player.playerClass.takeDamage(npc.damage);
                    }
                }
            }

        }
        
        private static void BulletMovement(int matchId)
        {
            foreach (var projectile in Variables.projectiles[matchId].ToList())
            {
                if (projectile == null) continue;
                RectangleF r = new Rectangle();
                var last = projectile.coordinates;
                var current = projectile.move();
                if (current.Item1 > 700 || current.Item2 > 700 || current.Item1 < 5 || current.Item2 < 5)
                {
                    try
                    {
                        Variables.projectiles[matchId].Remove(projectile);
                    }
                    catch (Exception) { }
                }
                //var toRemovePlayer = new List<Player>();

                foreach (var player in Variables.matches[matchId].players)
                {
                    if (player.playerClass == projectile.attacker) continue;
                    var k = player.playerClass.collision(last, projectile.coordinates, projectile.size);
                    if (k != (-1, -1))
                    {
                        try
                        {
                            player.playerClass.takeDamage(projectile.attacker.damage);
                            //if (player.playerClass.takeDamage(projectile.attacker.damage))
                            //{
                            //    toRemovePlayer.Add(player);
                            //}
                            player.playerClass.takeDamage(projectile.attacker.damage);
                            Variables.projectiles[matchId].Remove(projectile);
                        }
                        catch (Exception) { }
                    }
                }
                //foreach (var player in toRemovePlayer)
                //{
                //    Variables.matches[matchId].players.Remove(player);
                //}
                var toRemoveNpc = new List<Npc>();
                foreach (var npc in Variables.npcs[matchId])
                {
                    var k = npc.collision(last, projectile.coordinates, projectile.size);
                    if (k != (-1, -1))
                    {
                        try
                        {
                            if (npc.takeDamage(projectile.attacker.damage))
                            {
                                toRemoveNpc.Add(npc);
                            }
                            Variables.projectiles[matchId].Remove(projectile);
                        }
                        catch (Exception) { }
                    }
                }
                foreach (var npc in toRemoveNpc)
                {
                    Variables.npcs[matchId].Remove(npc);
                    Variables.pickables[matchId].Add(npc.OnDeath());
                }
                foreach (var obs in Variables.obstacles[matchId])
                {
                    var k = obs.collision(last, projectile.coordinates, projectile.size);
                    if (k != (-1, -1))
                    {
                        try
                        {
                            Variables.projectiles[matchId].Remove(projectile);
                        }
                        catch (Exception) { }
                    }
                }
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
