using Village.Agents;

namespace Village.Map
{
    public class Food
    {
        public float Value;
        public readonly Agent Farmer;

        public Food(float var, Agent farmer)
        {
            Value = var;
            Farmer = farmer;
        }
    }
}