using Village.Agents;
using Village.Map;

namespace Village.Genes.Chromosomes
{
    public abstract class MoveFunction
    {
        public abstract ActionList GetActions(Board b, Agent a, Field dest);
    }
}