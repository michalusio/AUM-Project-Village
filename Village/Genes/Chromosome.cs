using System;
using System.Collections.Generic;

namespace Village.Genes
{
    public class Chromosome
    {
        public const int GENE_COUNT = 5;

        private readonly List<Func<Agent, World, object>> _genes;

        public Chromosome(List<Tuple<Chromosome, float>> chromosome, float sum)
        {
            _genes=new List<Func<Agent, World, object>>(GENE_COUNT);
            for (int i = 0; i < GENE_COUNT; ++i)
            {
                _genes.Add(getRandomGene(chromosome,i, sum));
            }
        }

        private Func<Agent, World, object> getRandomGene(List<Tuple<Chromosome, float>> chromosome, int i, float sum)
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

        private Func<Agent, World, object> mutationGene(int i)
        {
            return null;
        }
    }
}