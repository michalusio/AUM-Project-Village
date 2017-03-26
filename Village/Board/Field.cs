using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Board
{
    public class Field
    {
        private bool _cultivation; // czy uprawa jest możliwa
        Food food = new Food(0); //
        public bool isAgent;
        public Field(bool cultivation, int foodValue)
        {
            _cultivation = cultivation;
            isAgent = false;
            Food food = new Food(foodValue);
        }
        public Field(bool cultivation)
        {
            _cultivation = cultivation;
            isAgent = false;
        }

        public bool getCultivation()
        {
            return _cultivation;
        }
    }
}
