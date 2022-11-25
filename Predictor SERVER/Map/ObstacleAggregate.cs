using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Predictor_SERVER.Map
{
    public class ObstacleAggregate : Aggregate
    {
        List<Obstacle> items = new List<Obstacle>();
        public override Iterator CreateIterator()
        {
            return new ObstacleIterator(this);
        }
        // Gets item count
        public int Count
        {
            get { return items.Count; }
        }
        // Indexer
        public Obstacle this[int index]
        {
            get { return items[index]; }
            set { items.Add(value); }
        }

        public List<Obstacle> GetList()
        {
            return items;
        }
        public void RemoveItemAt(int index)
        {
            items.RemoveAt(index);
        }
    }
}
