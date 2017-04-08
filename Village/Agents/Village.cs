using System.Collections.Generic;
using Village.Map;

namespace Village.Agents
{
    public class Village
    {
        private readonly Board _board;

        private const int AGENT_START_COUNT = 3;
        private const int REQUIRED_FOOD = 500;

        public Village(Board b)
        {
            _board = b;
            GetAgentList = new List<Agent>();
            for (int i = 0; i < AGENT_START_COUNT; i++)
            {
                GetAgentList.Add(new Agent(b, this));
            }
        }

        //Getters
        public float GetTotalFood { get; set; }
        public List<Agent> GetAgentList { get; }

        private bool FoodCheck()
        {
            return GetTotalFood >= REQUIRED_FOOD*1.25;
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