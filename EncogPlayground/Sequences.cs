using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground
{
    public class Sequences
    {
        public IEnumerable<int> FibbonacciLessThan(int max)
        {
            int prev = 1;
            int current = 1;
            while (current <= max)
            {
                yield return current;
                var temp = current;
                current += prev;
                prev = temp;
            }
        }

        public IEnumerable<int> PowersOf10(int max)
        {
            int current = 1;
            while(current <= max)
            {
                yield return current;
                current *= 10;
            }
        }
    }
}
