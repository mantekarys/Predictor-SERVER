using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Predictor_SERVER.Map
{
    public class TrapAggregate : Aggregate
    {
        List<Trap> items = new List<Trap>();
        public override Iterator CreateIterator()
        {
            return new TrapIterator(this);
        }
        // Gets item count
        public int Count
        {
            get { return items.Count; }
        }
        // Indexer
        public Trap this[int index]
        {
            get { return items[index]; }
            set { items.Add(value); }
        }

        public List<Trap> GetList()
        {
            return items;
        }

        public void  RemoveItemAt(int index)
        {
            items.RemoveAt(index);
        }
    }
}
