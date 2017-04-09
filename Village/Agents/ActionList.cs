using System.Collections.Generic;
using Village.Map;

namespace Village.Agents
{
    public class ActionList
    {
        private readonly List<Action> _actions;

        public ActionList()
        {
            _actions=new List<Action>();
        }

        public int Count => _actions.Count;

        public Action GetNextAction()
        {
            return _actions[0];
        }

        public void MarkAsDone()
        {
            _actions.RemoveAt(0);
        }

        public void UnPack(ActionList actions, Action action)
        {
            if (action != null) _actions.Insert(0,action);
            _actions.InsertRange(0,actions._actions);
        }

        public void AddAction(Action ac)
        {
            _actions.Add(ac);
        }
    }

    public class Action
    {
        public readonly ActionType Type;
        public readonly Field Destination;

        public Action(ActionType type, Field dest)
        {
            Type = type;
            Destination = dest;
        }
    }

    public enum ActionType
    {
        Move,Displace,PickUpFood,ReturnFood
    }
}