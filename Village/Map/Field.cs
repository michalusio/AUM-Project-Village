namespace Village.Map
{
    public class Field
    {
        private readonly bool _cultivation; // czy uprawa jest możliwa
        private Food _food; //
        public bool IsAgent;
        
        public Field(bool cultivation, int foodValue)
        {
            _cultivation = cultivation;
            IsAgent = false;
            _food = new Food(foodValue);
        }

        public Field(bool cultivation) : this(cultivation, 0) { }

        public bool getCultivation()
        {
            return _cultivation;
        }
    }
}
