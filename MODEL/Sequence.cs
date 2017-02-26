using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Sequence
    {
        private List<int> array;

        public List<int> Array
        {
            get { return array; }
            set { array = value; }
        }
        private List<int> array_prio;

        public List<int> Array_prio
        {
            get { return array_prio; }
            set { array_prio = value; }
        }
        public Sequence()
        {
            array = new List<int>();
            array_prio = new List<int>();
        }

        public Sequence(Sequence seq)
        {
            array = new List<int>();
            array_prio = new List<int>();
            foreach (int n in seq.Array)
            {
                array.Add(n);
            }
            foreach (int n in seq.array_prio)
            {
                array_prio.Add(n);
            }
        }
    }
}
