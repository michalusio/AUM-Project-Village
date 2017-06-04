using System;
using System.Collections.Generic;
using System.Linq;
using Village.Genes.Chromosomes.MoveFunctions;

namespace Village.Genes.Chromosomes
{
    public class MoveChromosome : Chromosome
    {
        public const int GeneCount = 5;
        private const double MUTATION_CHANCE = 0.055;
        public readonly List<MoveFunction> Functions = new List<MoveFunction>();

        public MoveChromosome()
        {
            for (int i = 0; i < GeneCount; i++)
            {
                Functions.Add(new NoDiagonalMoveFunction());
            }
        }

        public MoveChromosome(List<Tuple<Genome, float>> genomes)
        {
            for (int i = 0; i < GeneCount; i++)
            {
                if (Genome.Rnd.NextDouble() < MUTATION_CHANCE) Functions.Add(GetMutated());
                else
                {
                    Genome chosenGenome = genomes[genomes.Count-1].Item1;
                    float sum = genomes.Sum(g=>g.Item2);
                    float q = (float)Genome.Rnd.NextDouble()*sum;
                    foreach (var t in genomes)
                    {
                        q -= t.Item2;
                        if (q <= 0)
                        {
                            chosenGenome = t.Item1;
                            break;
                        }
                    }
                    Functions.Add(chosenGenome.GetChromosomes().Item2.Functions[i]);
                }
            }
        }

        public MoveFunction GetRandomMoveFunction()
        {
            return Functions[Genome.Rnd.Next(Functions.Count)];
        }

        private static MoveFunction GetMutated()
        {
            var lambdas = new Func<MoveFunction>[]
            {
                () => new NoDiagonalMoveFunction(),
                () => new NormalMoveFunction()
            };
            return lambdas[Genome.Rnd.Next(lambdas.Length)].Invoke();
        }
    }
}