using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WebSocketSharp;

namespace Predictor_SERVER.Server
{
    public class ReadyLoggerHandler : LoggerHandler
    {
        public override void HandleLog(int code, HashSet<Keys> keys)
        {
            if (code == 545)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(string.Format("Player has redied for match"));
                    sw.WriteLine(string.Format("{0} handled log", this.GetType().Name));
                }
            }
            else if (successor != null)
            {
                successor.HandleLog(code, keys);
            }
        }
    }
}
