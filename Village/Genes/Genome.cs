using System;
using System.Collections.Generic;
using System.Linq;
using Village.Genes.Chromosomes;

namespace Village.Genes
{
    public class Genome
    {
        public const float StatRange = 5f;
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
                _strength = 50f;
                _durability = 50f;
            }
            else
            {
                var sum = genomes.Sum(a => a.Item2);

                _fchromosome = new FoodChromosome(genomes);
                _mchromosome = new MoveChromosome(genomes);

                _strength = genomes.Sum(a => a.Item1._strength * a.Item2) / sum;
                _strength += (float)(Rnd.NextDouble() - 0.5) * StatRange;

                _durability = genomes.Sum(a => a.Item1._durability * a.Item2) / sum;
                _durability += (float)(Rnd.NextDouble() - 0.5) * StatRange;
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