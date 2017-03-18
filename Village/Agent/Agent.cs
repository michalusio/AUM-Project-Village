using System;
using System.Collections.Generic;
using System.Linq;

namespace Village.Agent
{
    class Agent
    {
        private int age, food;
        private double strength, durability, speed;

        private readonly Genes.Genome genome;

        public Agent(List<Tuple<Agent>> agents)
        {
            
        }

        public double getStrength() => strength;
        public double getDurability() => durability;
        public double getSpeed() => speed;
    }
}
