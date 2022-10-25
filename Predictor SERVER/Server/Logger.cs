using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Predictor_SERVER.Server
{
    public abstract class Logger
    {
        private static Logger instance = new Adapter();
        protected Logger()
        {
        }
        public static Logger getInstance()
        {
            //if (instance == null)
            //{
            //    lock (_lock)
            //    {
            //        if (instance == null)
            //        {
            //            instance = new Adapter();
            //        }
            //    }
            //}
            return instance;
        }

        public abstract void WriteMessageWithDebug(string message);
        public abstract void WriteMessage(string message);
    }
}
