using System;
using System.Drawing;
using Village.Genes;

namespace Village.Map
{
    public class Board
    {
        private const int NODE_COUNT = 5;

        public Field[,] FullBoard;
        public Board(int x, int y)
        {
            FullBoard = new Field[x, y];
            Tuple<PointF,bool>[] nodes=new Tuple<PointF, bool>[NODE_COUNT];
            for (int i = 0; i < nodes.Length; ++i)
            {
                nodes[i]=new Tuple<PointF, bool>(new PointF((float)Genome.Rnd.NextDouble()*x, (float)Genome.Rnd.NextDouble() * y), Genome.Rnd.NextDouble()>0.5);
            }
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    float dist = float.MaxValue;
                    int nearestNode = -1;
                    for (var index = 0; index < nodes.Length; index++)
                    {
                        var node = nodes[index];
                        float dst = Sqr(node.Item1.X - i) + Sqr(node.Item1.Y - j);
                        if (dst < dist)
                        {
                            dist = dst;
                            nearestNode = index;
                        }
                    }
                    FullBoard[i,j]=new Field(nodes[nearestNode].Item2);
                }
            }
        }

        private static float Sqr(float a)
        {
            return a * a;
        }
    }
        
    
}