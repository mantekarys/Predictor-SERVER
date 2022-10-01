using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Server
{
    public class Match
    {
        public int id;
        public DateTime date;
        public string name;
        public int peopleAmount = 1;
        public int ready = 0;


        public Match(int id, string name)
        {
            this.name = name;
            this.id = id;
        }

        public void start()
        {
            this.date = DateTime.Now;
        }
        public void end()
        {

        }
    }
}
