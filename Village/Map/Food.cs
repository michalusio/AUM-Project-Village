using Village.Agents;

namespace Village.Map
{
    public class Food
    {
        public float Value;
        public readonly Agent Farmer;
        public readonly int Level;
        public Food(float var, Agent farmer) : this(var, farmer, 1)
        {
        }

        public Food(float var, Agent farmer, int level)
        {
            Value = var;
            Farmer = farmer;
            Level = level;
        }

    }
}