using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasticBits
{
    public class Goal : Point, IPos
    {
        public int YTop { get; set; }
        public int YDown { get; set; }

        public Goal(int x, int y, int yTop, int yDown)
        {
            X = x;
            Y = y;
            YTop = yTop;
            YDown = yDown;
        }

        public override string ToString()
        {
            return $"Goal {X} {Y}";
        }
    }
}
