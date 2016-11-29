using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasticBits
{
    public class Entity : Point, IPos
    {
        private int _id;
        private string _type;
        private int _vx;
        private int _vy;
        private int _state;
        protected int _radius;

        public int Id { get { return _id; } }
        public string Type { get { return _type; } }
        public int VX { get { return _vx; } }
        public int VY { get { return _vy; } }
        public int State { get { return _state; } }
        public int Radius { get { return _radius; } }

        public Entity(string str)
        {
            string[] inputs = str.Split(' ');
            this._id = int.Parse(inputs[0]);
            this._type = inputs[1];
            this.X = int.Parse(inputs[2]);
            this.Y = int.Parse(inputs[3]);
            this._vx = int.Parse(inputs[4]);
            this._vy = int.Parse(inputs[5]);
            this._state = int.Parse(inputs[6]);
            this._radius = 0;
        }

        public double PrecisionSpeed()
        {
            return Math.Sqrt(this._vx * this._vx + this._vy * this._vy);
        }

        public override bool Equals(object obj)
        {
            Entity e = (Entity)obj;
            if (e != null)
                return this.Id == e.Id;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public override string ToString()
        {
            return $"{this._id} {this._type} {this.X} {this.Y}";
        }
    }

    public class Wizard : Entity
    {
        public Wizard(string entityData) : base(entityData)
        {
            this._radius = 400;
        }
    }

    public class Opponent : Entity
    {
        public Opponent(string entityData) : base(entityData)
        {
            this._radius = 400;
        }
    }

    public class Snaffle : Entity
    {
        public bool Reserve { get; set; }

        public Snaffle(string entityData) : base(entityData)
        {
            this._radius = 150;
        }
    }

    public class Bludger : Entity
    {
        public Bludger(string entityData) : base(entityData)
        {
            this._radius = 200;
        }
    }
}
