using System;
using System.Drawing;
using System.Linq;
using Village.Genes;
using Village.Map;

namespace Village.Agents
{
    public class Agent
    {
        private const float SPEED = 0.5f;

        private PointF _currentCoordinates;

        private readonly Genome _genome;
        private readonly ActionList _actionsToDo;
        private int _farm;

        private readonly Board _board;
        private readonly Village _village;

        public Agent(Board b,Village v)
        {
            _board = b;
            _village = v;
            _currentCoordinates = PointF.Empty;
            _genome = v.GetAgentList.Count<3 ? new Genome(null) : new Genome(v.GetAgentList.Select(a => new Tuple<Genome,float>(a._genome,a.GetFood/(a.GetAge+0.1f))).ToList());
            GetAge = 0;
            GetFood = 0;
            GetHoldedFood = 0;
            _actionsToDo = new ActionList();
        }

        //Getters
        public float GetCurrentX => _currentCoordinates.X;
        public float GetCurrentY => _currentCoordinates.Y;
        public float GetAge { get; set; }
        public float GetFood { get; set; }
        public float GetHoldedFood { get; set; }

        
        public void SetCoordinates(float x, float y)
        {
            _currentCoordinates = new PointF(x, y);
        }

        public Field GetField()
        {
            return _board.FullBoard[(int) GetCurrentX, (int) GetCurrentY];
        }
        
        public void DoAction()
        {
            if (_actionsToDo.Count==0)
            {
                _actionsToDo.UnPack(_genome.GetChromosomes().Item1.GetRandomFoodFunction().GetActions(_board, this), null);
            }
            if (_actionsToDo.Count > 0)
            {
                Action a = _actionsToDo.GetNextAction();
                if ((a.Type != ActionType.Moving && a.Destination != GetField())||a.Type==ActionType.Pathing)
                {
                    _actionsToDo.MarkAsDone();
                    _actionsToDo.UnPack(_genome.GetChromosomes().Item2.GetRandomMoveFunction().GetActions(_board,this,a.Destination),a);
                    a = _actionsToDo.GetNextAction();
                }
                switch (a.Type)
                {
                    case ActionType.Moving:
                        PointF moveVector=new PointF(a.Destination.X-(int)GetCurrentX,a.Destination.Y-(int)GetCurrentY);
                        float dist = (float) Math.Sqrt(moveVector.X*moveVector.X+moveVector.Y*moveVector.Y);
                        if (dist > SPEED)
                        {
                            float spDist = SPEED / dist;
                            moveVector.X *= spDist;
                            moveVector.Y *= spDist;
                        }
                        SetCoordinates(GetCurrentX+moveVector.X,GetCurrentY+moveVector.Y);
                        if (dist < SPEED)
                        {
                            _actionsToDo.MarkAsDone();
                        }
                        break;
                    case ActionType.PickingUpFood:
                        float foodTaken = Math.Min(_genome.GetStrength()-GetHoldedFood,GetField().GetFood().Value);
                        if (GetField().GetFood().Farmer != null)
                        {
                            GetField().GetFood().Farmer.GetFood += foodTaken;
                        }
                        GetHoldedFood += foodTaken;
                        GetField().SetFood(new Food(GetField().GetFood().Value-foodTaken,GetField().GetFood().Farmer));
                        _actionsToDo.MarkAsDone();
                        break;
                    case ActionType.ReturningFood:
                        GetFood += GetHoldedFood;
                        _village.GetTotalFood += GetHoldedFood;
                        GetHoldedFood = 0;
                        _actionsToDo.MarkAsDone();
                        break;
                    case ActionType.FarmingArea:
                        if (_farm>=1500/GetGenome().GetDurability())
                        {
                            _farm = 0;
                            for (int i = -1; i < 2; i++)
                            {
                                for (int j = -1; j < 2; j++)
                                {
                                    Field f = GetField().GetRelative(i, j);
                                    if (f.GetCultivation() && f.GetFood().Value < 1)
                                    {
                                        f.SetFood(new Food(GetGenome().GetStrength() * 0.5f, this));
                                        f.SetCultivation(false);
                                    }
                                }
                            }
                            _actionsToDo.MarkAsDone();
                        }
                        else _farm++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Action GetAction()
        {
            return _actionsToDo.GetNextAction();
        }

        public Genome GetGenome()
        {
            return _genome;
        }
    }
}