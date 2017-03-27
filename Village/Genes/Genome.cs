using System;
using System.Collections.Generic;
using System.Linq;

namespace Village.Genes
{
    public class Genome
    {
        public static readonly Random Rnd=new Random();
        public const int ChromosomeCount = 2;
        public const double MutationChance = 0.01;
        public const double StatRange = 0.5;

        private readonly double _strength;
        private readonly double _durability;
        private readonly List<Chromosome> _chromosomes;

        public Genome(List<Tuple<Genome, float>> genomes)
        {
            float sum = genomes.Sum(a => a.Item2);

            _chromosomes =new List<Chromosome>(ChromosomeCount);
            for (int i = 0; i < ChromosomeCount; ++i)
            {
                _chromosomes.Add(new Chromosome(genomes.Select(g => new Tuple<Chromosome, float>(g.Item1._chromosomes[i], g.Item2)).ToList(),sum));
            }

            _strength = genomes.Sum(a => a.Item1._strength * a.Item2) / sum;
            if (Rnd.NextDouble() < MutationChance) _strength += (Rnd.NextDouble()-0.5)*StatRange;
            _strength = Math.Max(_strength, 0.5);

            _durability = genomes.Sum(a => a.Item1._durability * a.Item2) / sum;
            if (Rnd.NextDouble() < MutationChance) _durability += (Rnd.NextDouble() - 0.5) * StatRange;
            _durability = Math.Max(_durability, 0.5);
        }

        public double GetStrength() => _strength;
        public double GetDurability() => _durability;

        public IReadOnlyList<Chromosome> GetChromosomes()
        {
            return _chromosomes.AsReadOnly();
        }

    }
}