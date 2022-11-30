using Newtonsoft.Json;
using Predictor_SERVER.Character;
using Predictor_SERVER.Map;
using Predictor_SERVER.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using Logger = Predictor_SERVER.Server.Logger;
using Timer = System.Timers.Timer;

namespace Predictor_SERVER
{
    public static class Variables
    {
        public static List<MapObject> mapO = new List<MapObject>();
        public static List<TrapAggregate> traps = new List<TrapAggregate>();
        public static List<ObstacleAggregate> obstacles = new List<ObstacleAggregate>();
        public static Map.Map map = new Map.Map("Map1", mapO);
        public static int howMany = 0;
        public static WebSocketSessionManager sesions;
        public static bool started = false;
        public static List<PickUpAggregate> pickables = new List<PickUpAggregate>();
        public static List<List<Projectile>> projectiles = new List<List<Projectile>>();
        public static List<Server.Match> matches = new List<Server.Match>();
        public static List<List<string>> matchIds = new List<List<string>>();
        
        internal static UseFirstItem useFirstItem = new UseFirstItem();  // command class for useage of first item
        internal static UseSecondItem useSecondItem = new UseSecondItem();  // command class for useage of second item
        internal static UseThirdItem useThirdItem = new UseThirdItem();  // command class for useage of third item
        internal static MatchBoost booster = new MatchBoost(); // boost granter for loser of a match 

        public static List<int> moveNpc = new List<int>();
        public static List<List<Npc>> npcs = new List<List<Npc>>();

        public static ObstacleAggregate createObstacles()
        {
            ObstacleAggregate matchObstacles = new ObstacleAggregate();
            var builder = new ObstacleBuilder();
            Random rnd = new Random(645);
            int obsCount = rnd.Next(1, 20);
            for (int i = 0; i < obsCount; i++)
            {
                builder.build(rnd.Next(5, 685), rnd.Next(5, 685));
                matchObstacles[i] = builder.GetObstacle();
            }
            return matchObstacles;
        }
        public static TrapAggregate createTraps()
        {
            TrapAggregate matchTraps = new TrapAggregate();
            var builder = new TrapBuilder();
            Random rnd = new Random(552);
            int trapCount = rnd.Next(5, 16);
            for (int i = 0; i < trapCount; i++)
            {
                builder.build(rnd.Next(5, 695), rnd.Next(5, 695));
                matchTraps[i] = builder.GetTrap();
            }
            return matchTraps;
        }
    }
    public class Echo : WebSocketBehavior, ISubject
    {
        Random rnd = new Random(978);
        Logger log = Logger.getInstance();
        CharacterFactory npcFactory = new CharacterFactory();

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
                    //log.WriteMessageWithDebug("Match created");
                    matchId = Variables.matches.Count;
                    Variables.matchIds.Add(new List<string>());
                    Variables.matchIds[matchId].Add(ID);
                    Variables.matches.Add(new Server.Match(matchId, text));
                    Variables.traps.Add(new TrapAggregate());
                    Variables.obstacles.Add(new ObstacleAggregate());
                    Variables.traps[matchId] = Variables.createTraps();
                    Variables.obstacles[matchId] = Variables.createObstacles();
                    Variables.pickables.Add(new PickUpAggregate());
                    Variables.pickables[matchId][0] = new DamagePowerUp((350, 350));
                    Variables.projectiles.Add(new List<Projectile>());

                    //Npc n1 = new Npc(15, 5, 5, 1, 30, 30);
                    //n1.selectDeathType("item");
                    //Npc n2 = n1.deepCopy();
                    //n2.coordinates = (655, 30);
                    //n2.selectDeathType("powerUp");
                    //Npc n3 = n1.deepCopy();
                    //n3.coordinates = (30, 655);
                    //n3.selectDeathType("item");
                    //Npc n4 = n1.deepCopy();
                    //n4.coordinates = (655, 655);
                    //n4.selectDeathType("powerUp");
                    Variables.npcs.Add(new List<Npc>() { (Npc)npcFactory.GetCharacter(0), (Npc)npcFactory.GetCharacter(1), 
                        (Npc)npcFactory.GetCharacter(2), (Npc)npcFactory.GetCharacter(3) });

                    Variables.moveNpc.Add(0);
                    var message = JsonConvert.SerializeObject((matchId, Variables.matches[matchId].peopleAmount - 1));
                    Send(message);
                }
                if (code == 876)////joined match
                {
                    //log.WriteMessageWithDebug("Match Joined");
                    matchId = int.Parse(text);
                    Variables.matchIds[matchId].Add(ID);
                    var message = JsonConvert.SerializeObject(Variables.matches[matchId].peopleAmount++);
                    Send(message);
                }
                if (code == 545)//readies up
                {
                    Variables.matches[matchId].ready += ready;
                    Variables.matches[matchId].players.Add(new Server.Player(ClassCreator.pickCreator(text, 15, which)));
                    if (rnd.Next(10) < 2)
                    {
                        if(rnd.Next(1) == 0)
                        {
                            Variables.booster.grantPowerUp(Variables.matches[matchId].players[Variables.matches[matchId].peopleAmount - 1].playerClass);
                        }
                        else
                        {
                            Variables.booster.grantItem(Variables.matches[matchId].players[Variables.matches[matchId].peopleAmount - 1].playerClass);
                        }
                    }
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
                    //var obstacles = Variables.obstacles[matchId];
                    //var pickups = Variables.pickables[matchId];
                    //int pad = 5;
                    foreach (var keyData in keys)
                    {
                        c.move(keyData, map);
                        var tempC = c.coordinates;
                        #region collision calc
                        Iterator ObsIterator = Variables.obstacles[matchId].CreateIterator();
                        var obs = (Obstacle)ObsIterator.First();
                        while(obs != null)
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
                            obs = (Obstacle)ObsIterator.Next();
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

                        Iterator TrapIterator = Variables.traps[matchId].CreateIterator();
                        var trap = (Trap)TrapIterator.First();
                        while (trap != null)
                        {
                            if (trap.collision(tempC, c.coordinates, c.size))
                            {
                                c.takeDamage(trap.damage);
                                Variables.traps[matchId].RemoveItemAt(TrapIterator.GetCurrentIndex());
                            }
                            trap = (Trap)TrapIterator.Next();
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
                        Iterator PickUpIterator = Variables.pickables[matchId].CreateIterator();
                        var pickUp = PickUpIterator.First();
                        while (pickUp != null)// checks whether a pickup has been touched and adds it to respected player
                        { 
                            if(pickUp is Item)
                            { 
                                var tempPickup = pickUp as Item;
                                if (tempPickup.collision(tempC, c.coordinates, c.size) && c.inventoryCheck()<3)
                                {
                                    Console.WriteLine("PickedUp Item");
                                    //c.addToInventory(tempPickup);
                                    //Variables.pickables[matchId].RemoveItemAt(PickUpIterator.GetCurrentIndex());
                                    tempPickup.pickedUp(c, matchId, PickUpIterator);
                                }
                            }
                            else if(pickUp is PowerUp)
                            {
                                var tempPickup = pickUp as PowerUp;
                                if (tempPickup.collision(tempC, c.coordinates, c.size))
                                {
                                    Console.WriteLine("PickedUp PowerUp");
                                    //c.applyPowerUp(tempPickup);
                                    //Variables.pickables[matchId].RemoveItemAt(PickUpIterator.GetCurrentIndex());
                                    tempPickup.pickedUp(c, matchId, PickUpIterator);
                                }
                            } 
                            pickUp = PickUpIterator.Next();
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
                            Variables.useFirstItem.Execute(c);
                        }
                        if (keyData == Keys.D2)
                        {
                            Variables.useSecondItem.Execute(c);
                        }
                        if (keyData == Keys.D3)
                        {
                            Variables.useThirdItem.Execute(c);
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
            List<int> npcsToAdd = BulletMovement(matchId);
            foreach (var proj in Variables.projectiles[matchId].ToList())
            {
                if (!proj.existsNotHit())
                {
                    Variables.projectiles[matchId].Remove(proj);
                }
            }
            foreach (int num in npcsToAdd)
            {
                var sw = Stopwatch.StartNew();
                long total = GC.GetTotalMemory(true);
                Variables.npcs[matchId].Insert(num, (Npc)npcFactory.GetCharacter(num));
                Console.WriteLine($"Diffrence in memory after an npc was returned {GC.GetTotalMemory(true) - total}");
                sw.Stop();
                Console.WriteLine($"Got npc {sw.Elapsed}");

            }
            if (Variables.moveNpc[matchId]++ == 4)
            {
                NpcMovement(matchId);
                Variables.moveNpc[matchId] = 0;
            }
            foreach(Player player in Variables.matches[matchId].players) 
            {
                player.playerClass.activeItemTimeExperationCheck();
            }
            List<ProjectileLeaf> projList = new List<ProjectileLeaf>();
            foreach (var proj in Variables.projectiles[matchId].ToList())
            {
                projList.AddRange(proj.getNotHitList());
            }
            var message = JsonConvert.SerializeObject((Variables.matches[matchId].players.ToList(), Variables.map,
                Variables.pickables[matchId].GetList().ToList(), projList, Variables.traps[matchId].GetList().ToList(),
                Variables.obstacles[matchId].GetList().ToList(), Variables.npcs[matchId].ToList()));
            
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
                    //log.WriteMessageWithDebug("Npc ability activated");
                }
            }
            else
            {
                if (ability.activated && ability.durationLeft == 0)
                {
                    if(ability.name == "Speed")npc.speed -= 20;
                    ability.activated = false;
                    //log.WriteMessageWithDebug("Npc ability deactivated");
                }
            }
            npc.ability = ability;

        }
        
        private static List<int> BulletMovement(int matchId)
        {
            var toRemoveNpc = new List<int>();
            foreach (var projectile in Variables.projectiles[matchId].ToList())
            {
                toRemoveNpc = BulletCalculation(projectile, matchId, toRemoveNpc);
            }
            foreach (int num in toRemoveNpc)
            {
                var npc = Variables.npcs[matchId][num];
                Variables.npcs[matchId].Remove(npc);
                Variables.pickables[matchId][Variables.pickables[matchId].Count] = npc.OnDeath();
            }
            return toRemoveNpc;
        }
        public static List<int> BulletCalculation(Projectile projectile, int matchId, List<int> toRemoveNpc)
        {
            RectangleF r = new Rectangle();
            var last = projectile.coordinates;
            var current = projectile.move();
            if (current.Item1 > 700 || current.Item2 > 700 || current.Item1 < 5 || current.Item2 < 5)
            {
                projectile.hit = true;
            }
            //var toRemovePlayer = new List<Player>();

            if (!projectile.hit) 
                foreach (var player in Variables.matches[matchId].players)
                {
                    if (player.playerClass == projectile.attacker || player.playerClass.health <= 0) continue;
                    var k = player.playerClass.collision(last, projectile.coordinates, projectile.size);
                    if (k != (-1, -1))
                    {
                        try
                        {
                            player.playerClass.takeDamage(projectile.attacker.damage);
                            projectile.hit = true;
                            break;

                        }   
                        catch (Exception) { }
                    }
                }
            int index = 0;
            if (!projectile.hit)
            {
                foreach (var npc in Variables.npcs[matchId])
                {
                    if (toRemoveNpc.Contains(index))
                    {
                        continue;
                    }
                    var k = npc.collision(last, projectile.coordinates, projectile.size);
                    if (k != (-1, -1))
                    {
                        try
                        {
                            if (npc.takeDamage(projectile.attacker.damage))
                            {
                                toRemoveNpc.Add(index);
                            }
                            projectile.hit = true;
                            break;
                        }
                        catch (Exception) { }
                    }
                    index++;
                }
                Iterator ObsIterator = Variables.obstacles[matchId].CreateIterator();
                var obs = (Obstacle)ObsIterator.First();
                if (!projectile.hit)
                {
                    while (obs != null)
                    {
                        var k = obs.collision(last, projectile.coordinates, projectile.size);
                        if (k != (-1, -1))
                        {
                            try
                            {
                                projectile.hit = true;
                                obs.takeDamage(projectile.attacker.damage);
                                if (obs.IsDestryed())
                                {
                                    Variables.obstacles[matchId].RemoveItemAt(ObsIterator.GetCurrentIndex());
                                }
                                Variables.projectiles[matchId].Remove(projectile);
                            }
                            catch (Exception) { }
                            break;
                        }
                        obs = (Obstacle)ObsIterator.Next();
                    }
                }
            }


            
            //foreach (int num in toRemoveNpc)
            //{
            //    var npc = Variables.npcs[matchId][num];
            //    Variables.npcs[matchId].Remove(npc);
            //    Variables.pickables[matchId][Variables.pickables[matchId].Count] = npc.OnDeath();
            //            projectile.hit = true;
            //            break;
            //}
                
            if (projectile is ProjectileComposite)
            {
                ProjectileComposite comp = (ProjectileComposite)projectile;
                foreach (var proj in comp.children)
                {
                    toRemoveNpc = BulletCalculation(proj, matchId, toRemoveNpc);
                }
            }
            return toRemoveNpc;
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
