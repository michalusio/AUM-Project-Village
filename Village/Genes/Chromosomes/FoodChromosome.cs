using System;
using System.Collections.Generic;
using System.Linq;
using Village.Genes.Chromosomes.FoodFunctions;

namespace Village.Genes.Chromosomes
{
    public class FoodChromosome : Chromosome
    {
        private const int GENE_COUNT = 5;
        private const double MUTATION_CHANCE = 0.01;
        public readonly List<FoodFunction> Functions;

        public FoodChromosome()
        {
            Functions=new List<FoodFunction>();
            for (int i = 0; i < 5; i++)
            {
                Functions.Add(new ScavengeFoodFunction());
            }
        }

        public FoodChromosome(IReadOnlyList<Tuple<Genome, float>> genomes)
        {
            for (int i = 0; i < GENE_COUNT; i++)
            {
                if (Genome.Rnd.NextDouble() < MUTATION_CHANCE) Functions.Add(GetMutated());
                else
                {
                    Genome chosenGenome = genomes[genomes.Count - 1].Item1;
                    float sum = genomes.Sum(g => g.Item2);
                    float q = (float)Genome.Rnd.NextDouble() * sum;
                    foreach (var t in genomes)
                    {
                        q -= t.Item2;
                        if (q > 0) continue;
                        chosenGenome = t.Item1;
                        break;
                    }
                    Functions.Add(chosenGenome.GetChromosomes().Item1.Functions[i]);
                }
            }
        }

        public FoodFunction GetRandomFoodFunction()
        {
            return Functions[Genome.Rnd.Next(Functions.Count)];
        }

        private static FoodFunction GetMutated()
        {
            return new ScavengeFoodFunction();
        }
    }
}