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
        private const float BASE_EAT = 3;
        private const float BASE_AGE = 0.01f;

        public Village(Board b)
        {
            _board = b;
            GetAgentList = new List<Agent>();
            for (int i = 0; i < AGENT_START_COUNT; i++)
            {
                GetAgentList.Add(new Agent(b, this));
            }
            GetTotalFood = 500;
        }

        //Getters
        public float GetTotalFood { get; set; }
        public List<Agent> GetAgentList { get; }

        private bool FoodCheck()
        {
            return GetTotalFood >= REQUIRED_FOOD*1.25;
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
        }

        public void ReproduceAgents(Agent agentA, Agent agentB)
        {
            while (FoodCheck())
            {
                GetAgentList.Add(new Agent(_board, this));
                GetTotalFood -= REQUIRED_FOOD;
            }
        }
    }
}