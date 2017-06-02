using Village.Agents;

namespace Village.Map
{
    public class Food
    {
        public float Value;
        public readonly Agent Farmer;
        public readonly int Type;

        public Food(float var, Agent farmer) : this(var, farmer, 1)
        {
        }

        public Food(float var, Agent farmer, int type)
        {
            Value = var;
            Farmer = farmer;
            Type = type;
        }
    }
}