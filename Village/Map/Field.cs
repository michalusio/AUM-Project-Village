namespace Village.Map
{
    public class Field
    {
        private readonly bool _cultivation; // czy uprawa jest możliwa
        private readonly bool _base; // czy to pole wioski
        private Food _food; //

        public int X, Y;

        public Field(bool cultivation, bool _base, float foodValue)
        {
            this._base = _base;
            _cultivation = cultivation;
            _food = new Food(foodValue, null);
        }

        public Field(bool cultivation, bool _base) : this(cultivation, _base, 0)
        {
        }

        public bool GetBase()
        {
            return _base;
        }

        public bool GetCultivation()
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
    }
}