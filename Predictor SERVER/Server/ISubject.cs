using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Predictor_SERVER.Server
{
    public interface ISubject
    {
        void Broadcast(int matchId);
        //void OnMessage(MessageEventArgs e);
    }
}
