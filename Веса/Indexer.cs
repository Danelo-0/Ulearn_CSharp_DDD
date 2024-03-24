using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    class Indexer
    {
        private double[] range1to4;
        private int start;
        private int length;
        public int Length
        { 
            get { return length; }

        }
        public Indexer(double[] range1to4, int start, int length)
        {
            if (start < 0 || length < 0 || start < Length || start + length > range1to4.Length)
            { 
                throw new ArgumentException(); 
            }
            else 
            {
                this.range1to4 = range1to4;
                this.start = start;
                this.length = length;
            }
            
        }

        public double this[int index]
        {
            get 
            {
                if (index < 0 || index > length - 1)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    return range1to4[index + start];
                }
            }
            set 
            {
                if (index < 0 || index > length - 1)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    range1to4[index + start] = value;
                }
            }
        }
    }
}
