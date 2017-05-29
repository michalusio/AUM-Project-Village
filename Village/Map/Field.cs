using System;

namespace Village.Map
{
    public class Field
    {
        private float _cultivation; // czy uprawa jest możliwa
        private readonly bool _base; // czy to pole wioski
        private Food _food; //

        public int X, Y;

        public readonly Board Board;

        public Field(Board b, bool cultivation, bool _base, float foodValue)
        {
            Board = b;
            this._base = _base;
            _cultivation = cultivation?1:0;
            _food = new Food(foodValue, null);
        }

        public Field(Board b, bool cultivation, bool _base) : this(b, cultivation, _base, 0)
        {
        }

        public bool GetBase()
        {
            return _base;
        }

        public bool GetCultivation()
        {
            return _cultivation>0.75f;
        }

        public float GetGrass()
        {
            return _cultivation;
        }

        public Food GetFood()
        {
            return _food;
        }

        public void SetFood(Food f)
        {
            _food = f;
        }

        public float DistSqr(Field field)
        {
            return Extensions.Sqr(X - field.X) + Extensions.Sqr(Y - field.Y);
        }

        public Field GetRelative(int x, int y)
        {
            if (Board.IsValid(x + X, y + Y))
            {
                return Board.FullBoard[x + X, y + Y];
            }
            return this;
        }

        public void SetCultivation(bool cult)
        {
            _cultivation = cult?1:0;
        }

        public void AddCultivation(float f)
        {
            _cultivation = Math.Min(1, _cultivation + f);
        }
    }
}