using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Server
{
    public class Adapter : Logger
    {
        private WebsocketLogger adaptee = new WebsocketLogger();

        public Adapter()
        {
        }
        public override void WriteMessageWithDebug(string message)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, "log.txt");
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(string.Format("{0}: {1}", DateTime.Now, message));
            }

            adaptee.WriteDebug(path);
        }
        public override void WriteMessage(string message)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, "log.txt");
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(string.Format("{0}: {1}", DateTime.Now, message));
            }
        }
    }
}
