using System;
using System.Collections.Generic;
using System.Linq;
using Village.Genes.Chromosomes.FoodFunctions;

namespace Village.Genes.Chromosomes
{
    public class FoodChromosome : Chromosome
    {
        public const int GeneCount = 5;
        private const double MUTATION_CHANCE = 0.055;
        public readonly List<FoodFunction> Functions = new List<FoodFunction>();

        public FoodChromosome()
        {
            for (int i = 0; i < GeneCount; i++)
            {
                Functions.Add(new ScavengeFoodFunction1());
            }
        }

        public FoodChromosome(List<Tuple<Genome, float>> genomes)
        {
            for (int i = 0; i < GeneCount; i++)
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
                        if (q <= 0)
                        {
                            chosenGenome = t.Item1;
                            break;
                        }
                    }
                    Functions.Add(chosenGenome.GetChromosomes().Item1.Functions[i]);
                }
            }
        }

        public FoodFunction GetRandomFoodFunction()
        {
            return Functions[Genome.Rnd.Next(Functions.Count)];
        }

        public static FoodFunction GetMutated()
        {
            var lambdas = new Func<FoodFunction>[]
            {
                () => new ScavengeFoodFunction1(),
                () => new ScavengeFoodFunction2(),
                () => new FarmFoodFunction1(),
                () => new FarmFoodFunction2()
            };
            return lambdas[Genome.Rnd.Next(lambdas.Length)].Invoke();
        }
    }
}