using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasticBits
{
    class Program
    {
        static void Main(string[] args)
        {
            int myTeamId = int.Parse(Console.ReadLine()); // if 0 you need to score on the right of the map, if 1 you need to score on the left
            Goal myGoal, oppGoal;
            int mana = 0;
            if (myTeamId == 0)
            {
                myGoal = new Goal(0, 3750, 5600, 1900);
                oppGoal = new Goal(16000, 3750, 5600, 1900);
            }
            else
            {
                myGoal = new Goal(16000, 3750, 5600, 1900);
                oppGoal = new Goal(0, 3750, 5600, 1900);
            }
            // game loop
            while (true)
            {
                List<Wizard> wizards = new List<Wizard>();
                List<Opponent> opponents = new List<Opponent>();
                List<Snaffle> snaffles = new List<Snaffle>();
                List<Bludger> bludger = new List<Bludger>();
                GM gm = new GM(myTeamId);

                int entities = int.Parse(Console.ReadLine()); // number of entities still in game
                for (int i = 0; i < entities; i++)
                {
                    string input = Console.ReadLine();
                    if (input.Contains(" WIZARD "))
                        wizards.Add(new Wizard(input));
                    if (input.Contains(" OPPONENT_WIZARD "))
                        opponents.Add(new Opponent(input));
                    if (input.Contains(" SNAFFLE "))
                        snaffles.Add(new Snaffle(input));
                    if (input.Contains(" BLUDGER "))
                        bludger.Add(new Bludger(input));
                }
                gm.Wizards = wizards;
                gm.Opponents = opponents;
                gm.Snaffles = snaffles;
                gm.Bludger = bludger;
                gm.MyGoal = myGoal;
                gm.OppGoal = oppGoal;
                mana++;
                mana = gm.Turn(mana);
            }
        }

        /*static void Main(string[] args)
        {
            GM gm = new GM(0);
            Point A = new Point() { X = 3601, Y = 2954 };
            Point B = new Point() { X = 16000, Y = 3750 };
            Point C = new Point() { X = 13139, Y = 3727 };
            Point D = gm.GetOptimalPoint(A, B, C, 2000);
            double H = gm.GetLenToWay(A, B, C);
            Console.WriteLine($"D: {D}");
            Console.WriteLine($"H: {H}");
            Console.ReadKey();
        }*/
    }
}
