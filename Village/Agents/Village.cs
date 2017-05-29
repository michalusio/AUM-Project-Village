using System.Collections.Generic;
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

        public readonly Graph FoodGraph;
        public readonly Graph PopGraph;
        private int _t;

        public Village(Board b)
        {
            _board = b;
            GetAgentList = new List<Agent>();
            for (int i = 0; i < AGENT_START_COUNT; i++)
            {
                GetAgentList.Add(new Agent(b, this));
            }
            GetTotalFood = 500;
            _t = 0;
            FoodGraph = new Graph();
            PopGraph = new Graph();
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

        public void TickAge()
        {
            for (int i = GetAgentList.Count - 1; i >= 0; --i)
            {
                Agent a = GetAgentList[i];
                a.GetAge += BASE_AGE;
                if (a.GetAge >= a.GetGenome().GetDurability())
                {
                    GetAgentList.RemoveAt(i);
                }
            }
            if (_t >= 3)
            {
                _t = 0;
                FoodGraph.AddPoint(GetTotalFood);
                PopGraph.AddPoint(GetAgentList.Count);
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