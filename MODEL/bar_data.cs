using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
        public class bar_data
        {
                private int scFrom;

                public int ScFrom
                {
                        get { return scFrom; }
                        set { scFrom = value; }
                }
                private int scTo;

                public int ScTo
                {
                        get { return scTo; }
                        set { scTo = value; }
                }
                private int scNum;

                public int ScNum
                {
                        get { return scNum; }
                        set { scNum = value; }
                }

                private double ratio;

                public double Ratio
                {
                    get { return ratio; }
                    set { ratio = value; }
                }
        }
}
