using Village.Agents;
using Village.Map;

namespace Village.Genes.Chromosomes.MoveFunctions
{
    public class NoDiagonalMoveFunction : MoveFunction
    {
        public override ActionList GetActions(Board b, Agent a, Field dest)
        {
            var aList = new ActionList();
            aList.AddAction(new Action(ActionType.Moving, a.GetField()));
            int i = 0;
            var f = a.GetField();
            if (f.X != dest.X) i++;
            if (f.Y != dest.Y) i++;
            if (i==2) aList.AddAction(Genome.Rnd.NextDouble() > 0.5
                ? new Action(ActionType.Moving, dest.GetRelative(a.GetField().X - dest.X, 0))
                : new Action(ActionType.Moving, dest.GetRelative(0, a.GetField().Y - dest.Y)));
            aList.AddAction(new Action(ActionType.Moving, dest));
            return aList;
        }

        public override string GetName()
        {
            return "NoDiagonalMove I";
        }
    }
}