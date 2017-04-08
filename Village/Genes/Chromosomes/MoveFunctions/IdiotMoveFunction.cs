using Village.Agents;
using Village.Map;

namespace Village.Genes.Chromosomes.MoveFunctions
{
    public class IdiotMoveFunction : MoveFunction
    {
        public override ActionList GetActions(Board b, Agent a, Field dest)
        {
            var aList=new ActionList();
            aList.AddAction(new Action(ActionType.Displace, dest));
            return aList;
        }
    }
}