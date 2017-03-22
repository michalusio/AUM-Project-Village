using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Board
{
    public class Board
    {
        public Board(int x, int y, double ratio)//ratio- green to brown ground 
        {
            int GreenSize = Convert.ToInt32(ratio * (x * y));
            Random generator = new Random();
            
            Field[,] fullBoard = new Field[x, y];
            double p;
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    p = generator.Next(1);
                    if (p < ratio && GreenSize > 0)
                    {
                        fullBoard[i, j] = new Field(true);
                    }
                    else
                        fullBoard[i, j] = new Field(false);

                }
            }
        }

    }
        
    
}
