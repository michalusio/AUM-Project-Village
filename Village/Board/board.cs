using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Board
{
    public class Board
    {
        
        private Field[,] _greenGroundTable = new Field[5, 5];
        private Field[,] _greenGroundTableWithEat = new Field[5, 5];
        private Field[,] _brownGroundTable = new Field[5, 5];
        private Field _greenField = new Field(true);
        private Field _brownField = new Field(false);
        private Field _eatField = new Field(true);

        Board()
        {
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    _greenGroundTable[i,j] = _greenField;
                    _brownGroundTable[i, j] = _brownField;
                    if(i==3 && j==3)
                    {
                        _greenGroundTableWithEat[i, j] = _eatField;
                    }else
                        _greenGroundTableWithEat[i, j] = _greenField;
                }
            }
        }

        public Field[,][,] createRandomBoard(int x, int y, double ratio)//ratio- green to brown ground 
        {
            int GreenSize = Convert.ToInt32(ratio * (x * y));
            Random generator = new Random();
            
            Field[,][,] fullBoard = new Field[x, y][,];
            double p;
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    p = generator.Next(1);
                    if (p < ratio && GreenSize > 0 && GreenSize%2 ==1)
                    {
                        fullBoard[i, j] = _greenGroundTable;   
                    }
                    if (p < ratio && GreenSize > 0 && GreenSize % 2 == 0)
                    {
                        fullBoard[i, j] = _greenGroundTable;
                    }

                }
            }
            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= y; j++)
                {
                    if(fullBoard[i, j] != _greenGroundTable && GreenSize == 0)
                    {
                        fullBoard[i, j] = _brownGroundTable;
                    }
                    else
                    {
                        fullBoard[i, j] = _greenGroundTableWithEat;
                    }
                    
                }
            }
            return fullBoard;
        }

    }
        
    
}
