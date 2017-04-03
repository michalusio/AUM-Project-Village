using System;
using System.Collections.Generic;
using Village.Agent;
using Village.Board;
using Village.Genes;

namespace Village.Genes
{
    public class Chromosome
    {
        public const int GENE_COUNT = 5;

        private readonly List<Func<Agent, Board.Board, object>> _genes;

        public Chromosome(List<Tuple<Chromosome, float>> chromosome, float sum)
        {
            _genes=new List<Func<Agent, Board.Board, object>>(GENE_COUNT);
            for (int i = 0; i < GENE_COUNT; ++i)
            {
                _genes.Add(getRandomGene(chromosome,i, sum));
            }
        }

        private Func<Agent, Board.Board, object> getRandomGene(List<Tuple<Chromosome, float>> chromosome, int i, float sum)
        {
            float result = (float) Genome.Rnd.NextDouble()*sum;
            foreach (var t in chromosome)
            {
                result -= t.Item2;
                if (result <= 0)
                {
                    return Genome.Rnd.NextDouble() < Genome.MUTATION_CHANCE ? mutationGene(i) : t.Item1._genes[i];
                }
            }
            return chromosome[chromosome.Count-1].Item1._genes[i];
        }

        private Func<Agent, Board.Board, object> mutationGene(int i)
        {
            return null;
        }
    }
}