using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace Predictor_SERVER.Server
{
    public class WebsocketLogger
    {
        public void WriteDebug(string path)
        {
            using (var ws = new WebSocket("ws://127.0.0.1:7890"))
            {
                if(ws.Log.File != path)
                {
                    ws.Log.File = path;
                    Console.WriteLine("change");
                }
                ws.Log.Level = LogLevel.Debug;
            }
        }
    }
}
