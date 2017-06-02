using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Village.Genes;
using Village.Map;

namespace Village.Agents
{
    public class Village
    {
        private readonly Board _board;

        private const int AGENT_START_COUNT = 3;
        private const int REQUIRED_FOOD = 500;
        private const float BASE_EAT = 2.5f;
        private const float BASE_AGE = 0.01f;
        private const float BREED_CHANCE = 0.001f;


        public PointF VillageMain;
        public readonly Graph FoodGraph;
        public readonly Graph PopGraph;
        public readonly TwoGraph Genes;

        public readonly Graph WholeFoodGraph;
        public readonly Graph WholePopGraph;
        public readonly TwoGraph WholeGenes;
        private int _t;

        public Village(Board b, PointF start)
        {
            VillageMain = start;
            _board = b;
            GetAgentList = new List<Agent>();
            for (int i = 0; i < AGENT_START_COUNT; i++)
            {
                GetAgentList.Add(new Agent(b, this));
            }
            GetTotalFood = 500;
            _t = 0;
            FoodGraph = new Graph();
            PopGraph = new Graph {Color = Color.Purple};
            Genes = new TwoGraph {Color1 = Color.DeepSkyBlue,Color2 = Color.Green};

            WholeFoodGraph = new Graph { PointCount = 1000000};
            WholePopGraph = new Graph { Color = Color.Purple, PointCount = 1000000 };
            WholeGenes = new TwoGraph { Color1 = Color.DeepSkyBlue, Color2 = Color.Green, PointCount = 1000000 };
        }

        //Getters
        public float GetTotalFood { get; set; }
        public List<Agent> GetAgentList { get; }

        private bool FoodCheck()
        {
            return GetTotalFood >= REQUIRED_FOOD*1.25f;
        }

        public void TickFood()
        {
            float totalDurability = GetAgentList.Sum(a => BASE_EAT/a.GetGenome().GetDurability());
            GetTotalFood -= totalDurability;
            if (GetTotalFood <= 0)
            {
                float rndDurability = (float)Genome.Rnd.NextDouble() * totalDurability;
                for (int i = GetAgentList.Count - 1; i >= 0; --i)
                {
                    rndDurability -= GetAgentList[i].GetGenome().GetDurability();
                    if (rndDurability <= 0)
                    {
                        GetAgentList.RemoveAt(i);
                        break;
                    }
                }
                GetTotalFood += 250;
            }
        }

        public void TickAge(float agingSpeed)
        {
            int farm = 0;
            int scav = 0;
            for (int i = GetAgentList.Count - 1; i >= 0; --i)
            {
                Agent a = GetAgentList[i];
                a.GetAge += BASE_AGE * agingSpeed;
                if (a.GetAge >= a.GetGenome().GetDurability())
                {
                    GetAgentList.RemoveAt(i);
                }
                else
                {
                    foreach (var f in a.GetGenome().GetChromosomes().Item1.Functions)
                    {
                        if (f.GetName().Contains("Farm")) farm++;
                        else if (f.GetName().Contains("Scavenge")) scav++;
                    }
                }
            }
            
            if (_t >= 3)
            {
                _t = 0;
                float a1 = farm / (float)GetAgentList.Count;
                float a2 = scav / (float)GetAgentList.Count;
                Genes.AddPoint(a1, a2);
                WholeGenes.AddPoint(a1, a2);
                FoodGraph.AddPoint(GetTotalFood);
                PopGraph.AddPoint(GetAgentList.Count);
                WholeFoodGraph.AddPoint(GetTotalFood);
                WholePopGraph.AddPoint(GetAgentList.Count);
            }
            else _t++;
        }

        public void ReproduceAgents()
        {
            while (FoodCheck() && Genome.Rnd.NextDouble()<BREED_CHANCE*(GetTotalFood*0.01f))
            {
                GetAgentList.Add(new Agent(_board, this));
                GetTotalFood -= REQUIRED_FOOD;
            }
        }
    }
}