using System;
using System.Collections.Generic;
using System.Linq;

namespace Village.Genes
{
    public class Genome
    {
        public static readonly Random Rnd=new Random();
        public const int CHROMOSOME_COUNT = 2;
        public const double MUTATION_CHANCE = 0.01;
        public const double STAT_RANGE = 0.5;

        private readonly double _strength;
        private readonly double _durability;
        private readonly List<Chromosome> _chromosomes;

        public Genome(List<Tuple<Genome, float>> genomes)
        {
            float sum = genomes.Sum(a => a.Item2);

            _chromosomes =new List<Chromosome>(CHROMOSOME_COUNT);
            for (int i = 0; i < CHROMOSOME_COUNT; ++i)
            {
                _chromosomes.Add(new Chromosome(genomes.Select(g => new Tuple<Chromosome, float>(g.Item1._chromosomes[i], g.Item2)).ToList(),sum));
            }

            _strength = genomes.Sum(a => a.Item1._strength * a.Item2) / sum;
            if (Rnd.NextDouble() < MUTATION_CHANCE) _strength += (Rnd.NextDouble()-0.5)*STAT_RANGE;
            _strength = Math.Max(_strength, 0.5);

            _durability = genomes.Sum(a => a.Item1._durability * a.Item2) / sum;
            if (Rnd.NextDouble() < MUTATION_CHANCE) _durability += (Rnd.NextDouble() - 0.5) * STAT_RANGE;
            _durability = Math.Max(_durability, 0.5);
        }

        public double getStrength() => _strength;
        public double getDurability() => _durability;

        public IReadOnlyList<Chromosome> getChromosomes()
        {
            return _chromosomes.AsReadOnly();
        }

    }
}