using System;
using System.Collections.Generic;
using Village.Agents;
using Village.Map;
using Action = Village.Agents.Action;

namespace Village.Genes.Chromosomes.FoodFunctions
{
    public class ScavengeFoodFunction2 : FoodFunction
    {
        public override ActionList GetActions(Board b, Agent a)
        {
            var aList=new ActionList();
            List<Field> foodFields=new List<Field>();
            for (int i = -9; i < 10; i++)
            {
                int X = (int) a.GetCurrentX + i;
                for (int j = -9; j < 10; j++)
                {
                    int Y = (int)a.GetCurrentY + j;
                    if (b.IsValid(X,Y))
                    {
                        if (b.FullBoard[X, Y].GetFood().Value >= a.GetGenome().GetStrength()*0.25f)
                        {
                            foodFields.Add(b.FullBoard[X, Y]);
                        }
                    }
                }
            }
            if (foodFields.Count > 0)
            {
                Field nearestField = null;
                float minDist = float.MaxValue;
                foreach (var f in foodFields)
                {
                    if (f.DistSqr(a.GetField()) < minDist)
                    {
                        minDist = f.DistSqr(a.GetField());
                        nearestField = f;
                    }
                }
                aList.AddAction(new Action(ActionType.PickingUpFood, nearestField));
                aList.AddAction(new Action(ActionType.ReturningFood, b.FullBoard[(int)b.GetVillage().VillageMain.X, (int)b.GetVillage().VillageMain.Y]));
            }
            else
            {
                int x=(int)a.GetCurrentX, y=(int)a.GetCurrentY;
                int i, j;
                do
                {
                    float ang = (float) (Genome.Rnd.NextDouble() * 2 * Math.PI);
                    i = (int) (15 * Math.Cos(ang));
                    j = (int) (15 * Math.Sin(ang));
                } while (!b.IsValid(x + i, y + j));
                aList.UnPack(a.GetGenome().GetChromosomes().Item2.GetRandomMoveFunction().GetActions(b, a, b.FullBoard[x + i, y + j]), null);
            }
            return aList;
        }

        public override string GetName()
        {
            return "Scavenge II";
        }
    }
}