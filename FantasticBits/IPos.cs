using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasticBits
{
    public interface IPos
    {
        int X { get; }
        int Y { get; }
        string Pos();
    }
}
