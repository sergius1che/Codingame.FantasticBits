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
                        Defender(W);
                    else
                        Raider(W);
                }
                else
                {
                    Point p = new Point()
                    {
                        X = OppGoal.X,
                        Y = OppGoal.Y + (-1) * W.VY
                    };
                    Console.Error.WriteLine($"THROW {p} 500");
                    Console.WriteLine($"THROW {p.Pos()} 500");
                }
            }
            return mana;
        }

        public void Raider(Wizard w)
        {
            Snaffle nearSnaf = GetNearSnuffle(w);
            if (nearSnaf != null)
            {
                Entity snafToFlip = FlipendoOn(w);
                if (snafToFlip != null && mana > 20)
                {
                    mana -= 20;
                    Console.WriteLine($"FLIPENDO {snafToFlip.Id}");
                }
                else if (mana > 25)
                {
                    Entity b = (Entity)GetNear(w, new List<Point>(Bludger));
                    Console.WriteLine($"OBLIVIATE {b.Id}");
                }
                else
                {
                    nearSnaf.Reserve = true;
                    Point p = nearSnaf;
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

        public void Defender(Wizard w)
        {
            List<Snaffle> toDef = Snaffles.Where(x => x.PrecisionLength(MyGoal) <= 8000).ToList();
            Snaffle nearSnaf = GetNearSnuffle(w, toDef);
            Snaffle acio = AccioOn(w);
            Snaffle fastSnaf = Snaffles
                .Where(x => ((x.VX + Math.Abs(x.VY) <= -1200 && myTeamId == 0) || (x.VX + Math.Abs(x.VY) >= 1200 && myTeamId == 1)) && !x.Reserve)
                .FirstOrDefault();
            if (nearSnaf != null)
            {
                Snaffle snafToFlip = FlipendoOn(w);
                if (snafToFlip != null && mana > 20)
                {
                    snafToFlip.Reserve = true;
                    mana -= 20;
                    Console.Error.WriteLine($"{snafToFlip} {snafToFlip.VX} {snafToFlip.VY}");
                    Console.WriteLine($"FLIPENDO {snafToFlip.Id}");
                }

                else if (fastSnaf != null
                && mana > 10)
                {
                    fastSnaf.Reserve = true;
                    mana -= 10;
                    Console.Error.WriteLine($"{fastSnaf} {fastSnaf.VX} {fastSnaf.VY}");
                    Console.WriteLine($"PETRIFICUS {fastSnaf.Id}");
                }
                else if (acio != null && mana > 20)
                {
                    acio.Reserve = true;
                    mana -= 20;
                    Console.Error.WriteLine($"{acio} {acio.VX} {acio.VY}");
                    Console.WriteLine($"ACCIO {acio.Id}");
                }
                else
                {
                    nearSnaf.Reserve = true;
                    Point p = nearSnaf;
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

        public Snaffle AccioOn(Wizard w)
        {
            Point w2 = new Point()
            {
                X = w.X + w.VX,
                Y = w.Y + w.VY
            };
            bool op = Opponents.Any(x => Between(x.X + x.VX, w2.X, MyGoal.X));
            List<Snaffle> snf = Snaffles.Where(x => Between(x.X, w2.X, MyGoal.X)).ToList();
            if (snf.Count > 0 && op)
            {
                Snaffle s = GetNearSnuffle(w2, snf);
                if (w2.PrecisionLength(s) < 2000)
                    return s;
            }
            return null;
        }

        public Snaffle FlipendoOn(Wizard w)
        {
            Point w2 = new Point()
            {
                X = w.X + (int)(w.VX * 0.75),
                Y = w.Y + (int)(w.VY * 0.75)

            };

            List<Snaffle> snfls = Snaffles.Where(x => Between((int)(x.X + x.VX * 0.75), w2.X, OppGoal.X)).ToList();
            Point s = GetNear(w2, new List<Point>(snfls));
            int Y = (int)Fx(OppGoal.X, w2, s);
            if (Between(Y, OppGoal.YDown, OppGoal.YTop) && w2.PrecisionLength(s) < 2450D)
                return s as Snaffle;
            else
                return null;
        }

        public bool Between(int val, int border1, int border2)
        {
            if (border1 > border2)
            {
                int b = border2;
                border2 = border1;
                border1 = b;
            }
            return border1 < val && val < border2;
        }

        public Point GetNear(Point point, List<Point> points)
        {
            double min = 16000D;
            Point near = null;
            for (int i = 0; i < points.Count; i++)
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

        public double Fx(int x, IPos A, IPos B)
        {
            int bxax = B.X - A.X;
            bxax = bxax == 0 ? 1 : bxax;
            return (x - A.X) * (B.Y - A.Y) / (bxax) + A.Y;
        }

        public double Fy(int y, IPos A, IPos B)
        {
            int byay = B.Y - A.Y;
            byay = byay == 0 ? 1 : byay;
            return (B.X - A.X) * (y - A.Y) / (byay) + A.X;
        }

        public Snaffle GetNearSnuffle(Point point, List<Snaffle> cur = null)
        {
            cur = cur == null ? Snaffles : cur;
            double min = 16000D;
            int vx = myTeamId == 0 ? 150 : -150;
            Snaffle s = null;
            for (int i = 0; i < cur.Count; i++)
            {
                double buf = point.PrecisionLength(cur[i]);
                if (buf < min
                && (!Snaffles[i].Reserve || Snaffles.Count == 1))
                {
                    s = cur[i];
                    min = buf;
                }
            }
            return s;
        }
    }
}
