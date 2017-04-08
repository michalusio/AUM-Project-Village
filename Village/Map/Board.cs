using System;
using System.Drawing;
using Village.Genes;

namespace Village.Map
{
    public class Board
    {
        private const int NODE_COUNT = 5;
        private const float FOOD_SIZE = 5;
        private const int FOOD_COUNT = 5;

        public Field[,] FullBoard;
        private readonly Agents.Village _village;

        public Board(int x, int y)
        {
            FullBoard = new Field[x, y];
            var nodes = new Tuple<PointF, bool>[NODE_COUNT];
            for (var i = 0; i < nodes.Length; ++i)
                nodes[i] =
                    new Tuple<PointF, bool>(
                        new PointF((float) Genome.Rnd.NextDouble() * x, (float) Genome.Rnd.NextDouble() * y),
                        Genome.Rnd.NextDouble() > 0.5);
            for (var i = 0; i < x; i++)
            for (var j = 0; j < y; j++)
            {
                var dist = float.MaxValue;
                var nearestNode = -1;
                for (var index = 0; index < nodes.Length; index++)
                {
                    var node = nodes[index];
                    var dst = Sqr(node.Item1.X - i) + Sqr(node.Item1.Y - j);
                    if (dst < dist)
                    {
                        dist = dst;
                        nearestNode = index;
                    }
                }
                FullBoard[i, j] = new Field(nodes[nearestNode].Item2,(i<2 && j<2), nodes[nearestNode].Item2?dist<FOOD_SIZE?FOOD_COUNT:0:0) {X=i,Y=j};
            }
            _village=new Agents.Village(this);
        }

        public Agents.Village GetVillage()
        {
            return _village;
        }

        private static float Sqr(float a)
        {
            return a * a;
        }

        public bool IsValid(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < FullBoard.GetLength(0) && y < FullBoard.GetLength(1));
        }
    }
}