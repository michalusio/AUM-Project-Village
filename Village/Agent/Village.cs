using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Agent
{
    class Village
    {
        //Total food stored in the village
        int Food = 0;
        //A number of food required to reproduce a new agent
        int requiredFood = 500;
        //A list of agents associated with the village
        List<Agent> agentsList;

        //Getters
        public int getTotalFood => Food;
        public int getRequiredFood => requiredFood;
        public List<Agent> getAgentList => agentsList;


        private bool foodCheck()
        {
            return getTotalFood > getRequiredFood ? true : false;
        }

        //Yet to be implemented
        public Agent reproduceAgent(Agent agentA, Agent agentB)
        {
            if ( !foodCheck() )
                return null;

            Agent newAgent = new Agent();

            Food -= getRequiredFood;
            requiredFood += 500;

            return newAgent;
        }
    }
}
