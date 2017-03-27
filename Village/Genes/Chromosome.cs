using System;
using System.Collections.Generic;
using Village.Agents;
using Village.Map;

namespace Village.Genes
{
    public class Chromosome
    {
        public const int GeneCount = 5;

        private readonly List<Func<Agent, Board, object>> _genes;

        public Chromosome(IReadOnlyList<Tuple<Chromosome, float>> chromosome, float sum)
        {
            _genes=new List<Func<Agent, Board, object>>(GeneCount);
            for (int i = 0; i < GeneCount; ++i)
            {
                _genes.Add(GetRandomGene(chromosome,i, sum));
            }
        }

        private static Func<Agent, Board, object> GetRandomGene(IReadOnlyList<Tuple<Chromosome, float>> chromosome, int i, float sum)
        {
            float result = (float) Genome.Rnd.NextDouble()*sum;
            foreach (var t in chromosome)
            {
                result -= t.Item2;
                if (result <= 0)
                {
                    return Genome.Rnd.NextDouble() < Genome.MutationChance ? MutationGene(i) : t.Item1._genes[i];
                }
            }
            return chromosome[chromosome.Count-1].Item1._genes[i];
        }

        private static Func<Agent, Board, object> MutationGene(int i)
        {
            return null;
        }
    }
}