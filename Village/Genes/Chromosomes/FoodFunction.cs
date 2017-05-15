using Village.Agents;
using Village.Map;

namespace Village.Genes.Chromosomes
{
    public abstract class FoodFunction:Function
    {
        public abstract ActionList GetActions(Board b, Agent a);
    }
}