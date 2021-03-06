using System;
using System.Collections.Generic;
using Village.Agents;
using Village.Map;
using Action = Village.Agents.Action;

namespace Village.Genes.Chromosomes.FoodFunctions
{
    public class FarmFoodFunction2 : FoodFunction
    {
        public override ActionList GetActions(Board b, Agent a)
        {
            var aList=new ActionList();
            List<Field> foodFields = new List<Field>();
            for (int i = -5; i < 6; i++)
            {
                int X = (int)a.GetCurrentX + i;
                for (int j = -5; j < 6; j++)
                {
                    int Y = (int)a.GetCurrentY + j;
                    if (b.IsValid(X, Y))
                    {
                        if (b.FullBoard[X, Y].GetCultivation())
                        {
                            foodFields.Add(b.FullBoard[X, Y]);
                        }
                    }
                }
            }
            if (foodFields.Count > 0)
            {
                aList.AddAction(new Action(ActionType.FarmingArea, foodFields[Genome.Rnd.Next(foodFields.Count)],2));
            }
            else
            {
                int x = (int)a.GetCurrentX, y = (int)a.GetCurrentY;
                int i, j;
                do
                {
                    float ang = (float)(Genome.Rnd.NextDouble() * 2 * Math.PI);
                    i = (int)(15 * Math.Cos(ang));
                    j = (int)(15 * Math.Sin(ang));
                } while (!b.IsValid(x + i, y + j));
                aList.UnPack(a.GetGenome().GetChromosomes().Item2.GetRandomMoveFunction().GetActions(b, a, b.FullBoard[x + i, y + j]), null);
            }
            return aList;
        }

        public override string GetName()
        {
            return "Farm II";
        }
    }
}