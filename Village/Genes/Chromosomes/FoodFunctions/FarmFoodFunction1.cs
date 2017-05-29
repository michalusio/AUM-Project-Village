using Village.Agents;
using Village.Map;
using Action = Village.Agents.Action;

namespace Village.Genes.Chromosomes.FoodFunctions
{
    public class FarmFoodFunction1 : FoodFunction
    {
        public override ActionList GetActions(Board b, Agent a)
        {
            var aList=new ActionList();
            aList.AddAction(new Action(ActionType.FarmingArea,a.GetField()));
            return aList;
        }

        public override string GetName()
        {
            return "Farm I";
        }
    }
}