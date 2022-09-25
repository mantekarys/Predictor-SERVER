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

namespace Predictor_SERVER
{
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var z = 2;
            if (e.Data == "159")
            {
                Class c1 = new Class(5,5,5,1,150,300);
                Class c2 = new Class(5, 5, 5, 1, 150, 50);
                List<Class> characters = new List<Class>();
                characters.Add(c1);
                characters.Add(c2);

                List<MapObject> mapO = new List<MapObject>();
                var map = new Map.Map("Map1", mapO);
                var message = JsonConvert.SerializeObject((characters, map));
                Send(message);
            }
            
        }
    }

    public class EchoAll : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "RequestMap")
            {

            }
            Console.WriteLine("Received message from EchoAll client: " + e.Data);
            Sessions.Broadcast(e.Data);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
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
