using System;

namespace Classificadores
{
    public class IndexAndDistance : IComparable<IndexAndDistance>
    {
        public int idx;  
        public double dist;  
        public int CompareTo(IndexAndDistance other)
        {
            if (this.dist < other.dist) return -1;
            else if (this.dist > other.dist) return +1;
            else return 0;
        }
    }
}
