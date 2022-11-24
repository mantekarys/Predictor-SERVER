using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictor_SERVER.Map
{
    public class TrapIterator : Iterator
    {
        TrapAggregate collection;
        int current = 0;
        int step = 1;
        // Constructor
        public TrapIterator(TrapAggregate collection)
        {
            this.collection = collection;
        }
        // Gets first item
        public override object First()
        {
            current = 0;
            if (IsDone()) 
            { 
                return null; 
            }
            else
                return collection[current];
        }
        // Gets next item
        public override object Next()
        {
            current += step;
            if (!IsDone())
                return collection[current];
            else
                return null;
        }
        // Gets or sets stepsize
        public int Step
        {
            get { return step; }
            set { step = value; }
        }
        // Gets current iterator item
        public override object CurrentItem()
        {
            if (!IsDone())
                return collection[current];
            else
                return null;
        }
        // Gets whether iteration is complete
        public override bool IsDone()
        {
            return current >= collection.Count;
        }

        public override int GetCurrentIndex()
        {
            return current;
        }
    }
}
