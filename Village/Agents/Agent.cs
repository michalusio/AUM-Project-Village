using System.Drawing;
using Village.Genes;

namespace Village.Agents
{
    public class Agent
    {
        //Coordinates
        private Point _currentCoordinates, _futureCoordinates;
        //Age and Food
        //List of genomes
        private Genome _genome;

        public Agent()
        {
            _currentCoordinates=Point.Empty;
            _futureCoordinates=Point.Empty;
            _genome = new Genome(null);
            GetAge = 0;
            GetFood = 0;
        }
        
        //Getters
        public int GetCurrentX => _currentCoordinates.X;
        public int GetCurrentY => _currentCoordinates.Y;
        public int GetAge { get; }
        public int GetFood { get; }

        //Setters
        public void setCoordinates(int X, int Y)
        {
            _currentCoordinates=new Point(X,Y);
        }
        
        //Other methods
        public void doRandomAction()
        {
            
        }
    }
}
