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
    public abstract class LoggerHandler
    {
        protected LoggerHandler successor;
        public string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log.txt");
        public void SetSuccessor(LoggerHandler successor)
        {
            this.successor = successor;
        }

        public abstract void HandleLog(int code, HashSet<Keys> keys);

    }
}
