using Village.Agents;
using Village.Map;

namespace Village.Genes.Chromosomes.MoveFunctions
{
    public class NormalMoveFunction : MoveFunction
    {
        public override ActionList GetActions(Board b, Agent a, Field dest)
        {
            var aList=new ActionList();
            aList.AddAction(new Action(ActionType.Moving, dest));
            return aList;
        }

        public override string GetName()
        {
            return "NormalMove I";
        }
    }
}