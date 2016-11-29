using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasticBits
{
    public class Point : IPos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point()
        {
        }

        public double PrecisionLength(IPos target)
        {
            int x = target.X - this.X;
            int y = target.Y - this.Y;
            return Math.Sqrt(x * x + y * y);
        }

        public string Pos()
        {
            return $"{this.X} {this.Y}";
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}
