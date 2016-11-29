using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasticBits
{
    // game master
    public class GM
    {
        public List<Wizard> Wizards { get; set; }
        public List<Opponent> Opponents { get; set; }
        public List<Snaffle> Snaffles { get; set; }
        public List<Bludger> Bludger { get; set; }
        public Goal MyGoal { get; set; }
        public Goal OppGoal { get; set; }
        private int myTeamId;
        private int mana;

        public GM(int myTeamId)
        {
            this.myTeamId = myTeamId;
        }

        public int Turn(int m)
        {
            mana = m;
            Console.Error.WriteLine($"{Wizards.Count} {Opponents.Count} {Snaffles.Count}");
            for (int i = 0; i < 2; i++)
            {
                Wizard W = Wizards[i];
                if (W.State == 0)
                {
                    if (i == 0)
                        Raider(W);
                    else
                        Defender(W);
                }
                else
                {
                    Point p = GetOptimalWay(W, OppGoal);
                    p.Y += (-1) * W.VY;
                    Console.Error.WriteLine($"THROW {p} 500");
                    Console.WriteLine($"THROW {OppGoal.Pos()} 500");
                }
            }
            return mana;
        }

        public void Raider(Wizard w)
        {
            Snaffle nearSnaf = GetNearSnuffle(OppGoal);
            if (nearSnaf != null)
            {
                nearSnaf.Reserve = true;
                Point p = GetOptimalWay(w, nearSnaf);
                p.X += nearSnaf.VX;
                p.Y += nearSnaf.VY;
                Console.Error.WriteLine($"MOVE {p} 150");
                Console.WriteLine($"MOVE {p.Pos()} 150");
            }
            else
            {
                Point o = GetNear(OppGoal, new List<Point>(Opponents));
                Console.WriteLine($"MOVE {o.Pos()} 150");
            }
        }

        public void Defender(Wizard w)
        {
            Snaffle nearSnaf = GetNearSnuffle(MyGoal);
            if (nearSnaf != null)
            {
                nearSnaf.Reserve = true;
                if (MyGoal.PrecisionLength(w) > MyGoal.PrecisionLength(nearSnaf)
                && mana > 20)
                {
                    mana -= 20;
                    Console.WriteLine($"ACCIO {nearSnaf.Id}");
                }
                else
                {
                    Point p = GetOptimalWay(w, nearSnaf);
                    p.X += nearSnaf.VX;
                    p.Y += nearSnaf.VY;
                    Console.Error.WriteLine($"MOVE {p} 150");
                    Console.WriteLine($"MOVE {p.Pos()} 150");
                }
            }
            else
            {
                Point o = GetNear(OppGoal, new List<Point>(Opponents));
                Console.WriteLine($"MOVE {o.Pos()} 150");
            }
        }

        public Point GetNear(Point point, List<Point> points)
        {
            double min = 16000D;
            Point near = null;
            for(int i = 0; i < points.Count; i++)
            {
                double b = point.PrecisionLength(points[i]);
                if (min > b)
                {
                    min = b;
                    near = points[i];
                }
            }
            if (near != null)
                return near;
            return point;
        }

        public Point GetOptimalWay(Wizard w, Point target)
        {
            List<Entity> entities = new List<Entity>(this.Bludger.Count + this.Opponents.Count);
            entities.AddRange(this.Bludger.Where(x => x.X < target.X && x.X > w.X).ToList());
            entities.AddRange(this.Opponents.Where(x => x.X < target.X && x.X > w.X).ToList());
            double min = 16000D;
            Entity e = null;
            for(int i = 0; i < entities.Count; i++)
            {
                double buf = entities[i].PrecisionLength(target);
                double lenToWay = GetLenToWay(w, target, entities[i]);
                if (buf < min && Math.Abs(lenToWay) <= w.Radius * 2)
                {
                    min = buf;
                    e = entities[i];
                }
            }
            if (e != null)
                return GetOptimalPoint(w, target, e, w.Radius * 2);
            else
                return target;
        }

        public double GetLenToWay(IPos start, IPos end, IPos barier)
        {
            double a = (barier.X - start.X) * (end.Y - start.Y) / (end.X - start.X) + start.Y - (double)barier.Y;
            double b = (end.X - start.X) * (barier.Y - start.Y) / (end.Y - start.Y) + start.X - (double)barier.X;
            Point A = new Point() { X = barier.X, Y = barier.Y + (int)Math.Round(a, 0) };
            Point B = new Point() { X = barier.X + (int)Math.Round(b, 0), Y = barier.Y };
            return a * b / A.PrecisionLength(B);
        }

        public Point GetOptimalPoint(IPos A, IPos B, IPos C, double r = 800D)
        {
            double a = (C.X - A.X) * (B.Y - A.Y) / (B.X - A.X) + A.Y - C.Y;
            double b = (B.X - A.X) * (C.Y - A.Y) / (B.Y - A.Y) + A.X - C.X;
            double c = Math.Sqrt(a*a + b*b);
            double h = a * b / c;
            double c2 = b * r / h;
            Point D = new Point();
            D.X = (int)Math.Round(C.X + (Math.Sqrt(c2*c2 - r*r)) * r / c2, 0);
            D.Y = (int)Math.Round(C.Y + a,0);
            if (D.X < 0 || D.X > 16000 || D.Y < 0 || D.Y > 16000)
                return new Point() { X = A.X, Y = A.Y };
            return D;
        }

        public Snaffle GetNearSnuffle(Point point)
        {
            double min = 16000D;
            int vx = myTeamId == 0 ? 150 : -150;
            Snaffle s = null;
            for (int i = 0; i < Snaffles.Count; i++)
            {
                double buf = point.PrecisionLength(Snaffles[i]);
                if (buf < min
                && ((Snaffles[i].VX <= vx && myTeamId == 0)
                    || (Snaffles[i].VX >= vx && myTeamId == 1))
                && !Snaffles[i].Reserve)
                {
                    s = Snaffles[i];
                    min = buf;
                }
            }
            return s;
        }
    }
}
