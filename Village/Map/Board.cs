using System;
using System.Drawing;
using Village.Genes;

namespace Village.Map
{
    public class Board
    {
        private const int NODE_COUNT = 10;
        private const float FOOD_SIZE = 3;
        private const int FOOD_COUNT = 35;

        private readonly Agents.Village _village;
        public Field[,] FullBoard;

        public Board(int x, int y, PointF start)
        {
            FullBoard = new Field[x, y];
            var nodes = new Tuple<PointF, bool>[NODE_COUNT * FullBoard.Length / (60 * 60)];
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
                    var dst = Extensions.Sqr(node.Item1.X - i) + Extensions.Sqr(node.Item1.Y - j);
                    if (dst < dist)
                    {
                        dist = dst;
                        nearestNode = index;
                    }
                }
                FullBoard[i, j] = new Field(this, nodes[nearestNode].Item2,Extensions.Sqr(i-x*0.5f) + Extensions.Sqr(j - y * 0.5f) < 2, nodes[nearestNode].Item2?dist<FOOD_SIZE * FullBoard.Length / (60 * 60) ? FOOD_COUNT:0:0) {X=i,Y=j};
            }
            _village=new Agents.Village(this,start);
        }

        public Agents.Village GetVillage()
        {
            return _village;
        }

        public bool IsValid(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < FullBoard.GetLength(0) && y < FullBoard.GetLength(1));
        }
    }
}