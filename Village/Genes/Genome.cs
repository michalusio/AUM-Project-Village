using System;
using System.Collections.Generic;
using System.Linq;
using Village.Genes.Chromosomes;

namespace Village.Genes
{
    public class Genome
    {
        public const float MutationChance = 0.01f;
        public const float StatRange = 0.5f;
        public static readonly Random Rnd = new Random();
        private readonly FoodChromosome _fchromosome;
        private readonly MoveChromosome _mchromosome;
        private readonly float _durability;

        private readonly float _strength;

        public Genome(List<Tuple<Genome, float>> genomes)
        {
            if (genomes == null)
            {
                _fchromosome = new FoodChromosome();
                _mchromosome = new MoveChromosome();
                _strength = 0.5f;
                _durability = 0.5f;
            }
            else
            {
                var sum = genomes.Sum(a => a.Item2);

                _fchromosome = new FoodChromosome(genomes);
                _mchromosome = new MoveChromosome(genomes);

                _strength = genomes.Sum(a => a.Item1._strength * a.Item2) / sum;
                if (Rnd.NextDouble() < MutationChance) _strength += (float)(Rnd.NextDouble() - 0.5) * StatRange;
                _strength = Math.Max(_strength, 0.5f);

                _durability = genomes.Sum(a => a.Item1._durability * a.Item2) / sum;
                if (Rnd.NextDouble() < MutationChance) _durability += (float)(Rnd.NextDouble() - 0.5) * StatRange;
                _durability = Math.Max(_durability, 0.5f);
            }
        }

        public float GetStrength() => _strength;
        public float GetDurability() => _durability;

        public Tuple<FoodChromosome,MoveChromosome> GetChromosomes()
        {
            return new Tuple<FoodChromosome, MoveChromosome>(_fchromosome,_mchromosome);
        }
    }
}