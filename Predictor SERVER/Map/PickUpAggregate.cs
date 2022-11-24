using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Predictor_SERVER.Map
{
    public class PickUpAggregate : Aggregate
    {
        List<PickUp> items = new List<PickUp>();
        public override Iterator CreateIterator()
        {
            return new PickUpIterator(this);
        }
        // Gets item count
        public int Count
        {
            get { return items.Count; }
        }
        // Indexer
        public PickUp this[int index]
        {
            get { return items[index]; }
            set { items.Add(value); }
        }

        public List<PickUp> GetList()
        {
            return items;
        }
        public void RemoveItemAt(int index)
        {
            items.RemoveAt(index);
        }
    }
}
