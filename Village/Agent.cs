using System;
using System.Collections.Generic;
using System.Linq;

namespace Village.Agent
{
    class Agent
    {
        //Coordinates
        private int [] currentCoordinates = new int[2];
        private int [] futureCoordinates = new int[2];
        //Age and Food
        private int age, food;
        //List of genomes
        private readonly List<Tuple<Genes.Genome, float>> genome;

        public Agent()
        {
            Genes.Genome(genome);
        }
        
        //Getters
        public int getCurrentX => currentCoordinates[0];
        public int getCurrentY => currentCoordinates[1];
        public int getAge => age;
        public int getFood => food;
        
        //Setters
        public void setCoordinates(int X, int Y)
        {
            currentCoordinates[0] = X;
            currentCoordinates[1] = Y;
        }
        
        //Other methods
        public void doRandomAction()
        {
            
        }
    }
}
