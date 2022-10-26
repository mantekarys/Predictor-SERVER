using Newtonsoft.Json;
using Predictor_SERVER.Character;
using Predictor_SERVER.Map;
using Predictor_SERVER.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
        internal static UseItem useItem = new UseItem();

        public static List<int> moveNpc = new List<int>();
        public static List<List<Npc>> npcs = new List<List<Npc>>();

        public static List<Obstacle> createObstacles()
        {
            List<Obstacle> matchObstacles = new List<Obstacle>();
            var builder = new ObstacleBuilder();
            Random rnd = new Random(645);
            int obsCount = rnd.Next(1, 20);
            for (int i = 0; i < obsCount; i++)
            {
                builder.build(rnd.Next(5, 685), rnd.Next(5, 685));
                matchObstacles.Add(builder.GetObstacle());
            }
            return matchObstacles;
        }
        public static List<Trap> createTraps()
        {
            List<Trap> matchTraps = new List<Trap>();
            var builder = new TrapBuilder();
            Random rnd = new Random(552);
            int trapCount = rnd.Next(5, 16);
            for (int i = 0; i < trapCount; i++)
            {
                builder.build(rnd.Next(5, 695), rnd.Next(5, 695));
                matchTraps.Add(builder.GetTrap());
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
                    Variables.pickables.Add(new List<PickUp>() { new DamagePowerUp((350, 350)) });
                    Variables.projectiles.Add(new List<Projectile>());

                    Npc n1 = new Npc(15, 5, 5, 1, 30, 30);
                    n1.selectDeathType("item");
                    Npc n2 = n1.deepCopy();
                    n2.coordinates = (655, 30);
                    n2.selectDeathType("powerUp");
                    Npc n3 = n1.deepCopy();
                    n3.coordinates = (30, 655);
                    n3.selectDeathType("item");
                    Npc n4 = n1.deepCopy();
                    n4.coordinates = (655, 655);
                    n4.selectDeathType("powerUp");
                    Variables.npcs.Add(new List<Npc>() { n1, n2, n3, n4});

                    GCHandle handle = GCHandle.Alloc(n1.ability, GCHandleType.WeakTrackResurrection);
                    int address = GCHandle.ToIntPtr(handle).ToInt32();
                    Console.WriteLine(address);

                    handle = GCHandle.Alloc(n2.ability, GCHandleType.WeakTrackResurrection);
                    address = GCHandle.ToIntPtr(handle).ToInt32();
                    Console.WriteLine(address);

                    handle = GCHandle.Alloc(n3.ability, GCHandleType.WeakTrackResurrection);
                    address = GCHandle.ToIntPtr(handle).ToInt32();
                    Console.WriteLine(address);

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
                    var pickups = Variables.pickables[matchId];
                    int pad = 5;
                    foreach (var keyData in keys)
                    {
                        c.move(keyData, map);
                        var tempC = c.coordinates;
                        #region collision calc
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
                            if (num++ == which || player.playerClass.health<=0) continue;
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

                        for(int i =0; i< Variables.pickables[matchId].Count;i++)// checks whether a pickup has been touched and adds it to respected player
                        {
                            
                            if(Variables.pickables[matchId][i] is Item)
                            {
                                var tempPickup = Variables.pickables[matchId][i] as Item;
                                if (tempPickup.collision(tempC, c.coordinates, c.size) && c.inventoryCheck()<3)
                                {
                                    c.addToInventory(tempPickup);
                                    Variables.pickables[matchId].Remove(Variables.pickables[matchId][i]);
                                }
                            }
                            else if(Variables.pickables[matchId][i] is PowerUp)
                            {
                                var tempPickup = Variables.pickables[matchId][i] as PowerUp;
                                if (tempPickup.collision(tempC, c.coordinates, c.size))
                                {
                                    c.applyPowerUp(tempPickup);
                                    Variables.pickables[matchId].Remove(Variables.pickables[matchId][i]);
                                }
                            } 
                        }
                        #endregion collision calc
                        #region Other key read

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
                        if (keyData == Keys.D1)
                        {
                            Variables.useItem.Execute(c, 0);
                        }
                        if (keyData == Keys.D2)
                        {
                            Variables.useItem.Execute(c, 1);
                        }
                        if (keyData == Keys.D3)
                        {
                            Variables.useItem.Execute(c, 2);
                        }
                        #endregion Other key read end

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
            foreach(Player player in Variables.matches[matchId].players) 
            {
                player.playerClass.activeItemTimeExperationCheck();
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
            int which = 0;
            string[] moveTypes = new string[] { "random", "circle" };
            string[] deathTypes = new string[] { "item", "powerUp" };
            foreach (var npc in Variables.npcs[matchId])
            {
                ActivateNpcAbility(npc, matchId);
                npc.calculateAction(moveTypes[which], matchId);
                which = (which + 1) % 2;
            }

        }
        private void ActivateNpcAbility(Npc npc, int matchId)
        {
            Ability ability = npc.ability;
            ability.cooldownLeft--;
            ability.durationLeft--;
            if (ability.cooldownLeft == 0)
            {
                if (ability.name == "Speed")
                {
                    npc.speed += 20;
                    npc.ability.durationLeft = npc.ability.duration;
                    npc.ability.cooldownLeft = npc.ability.cooldown;
                    npc.ability.activated = true;
                }
            }
            else
            {
                if (ability.activated && ability.durationLeft == 0)
                {
                    if(ability.name == "Speed")npc.speed -= 20;
                    ability.activated = false;
                }
            }
            npc.ability = ability;

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
                    if (player.playerClass == projectile.attacker || player.playerClass.health <= 0) continue;
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
