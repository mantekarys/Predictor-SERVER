using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Server
{
    public abstract class Subject
    {
        public abstract void Broadcast();
        public abstract void OnMessage();
    }
}
