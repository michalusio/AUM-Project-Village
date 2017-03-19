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
        public int eat; //eat = 0 brak jedzenia
        public bool isAgent;
        public Field(bool cultivation, int eatingValue)
        {
            _cultivation = cultivation;
            isAgent = false;
            eat = eatingValue;
        }
        public Field(bool cultivation)
        {
            _cultivation = cultivation;
            isAgent = false;
            eat = 0;
        }
        Field()
        {
            _cultivation = true;
            isAgent = false;
            eat = 0;
        }
    }
}
