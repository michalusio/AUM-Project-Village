using System.Collections.Generic;

namespace Village.Agents
{
    public class Village
    {
        private const int REQUIRED_FOOD = 500;

        //Getters
        public int GetTotalFood { get; private set; }
        public int GetRequiredFood => REQUIRED_FOOD;
        public List<Agent> GetAgentList { get; }

        public Village()
        {
            GetAgentList=new List<Agent>();
        }

        private bool FoodCheck()
        {
            return GetTotalFood > GetRequiredFood;
        }

        public Agent ReproduceAgent(Agent agentA, Agent agentB)
        {
            if (!FoodCheck()) return null;

            Agent newAgent = new Agent();

            GetTotalFood -= GetRequiredFood;

            return newAgent;
        }
    }
}
