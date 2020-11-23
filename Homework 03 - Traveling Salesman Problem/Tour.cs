using System.Collections.Generic;
using System.Text;

namespace FirstSteps
{
    public class Tour
    {
        private List<City> tour;
        private double distance;
        private double fitness;

        public Tour()
        {
            tour = new List<City>();
            Distance = 0;
            Fitness = 0;
        }

        public Tour(List<City> tour)
            : this()
        {
            this.tour = tour;
        }

        public List<City> Cities
        {
            get
            {
                return tour;
            }
        }

        public double Distance
        {
            get
            {
                // Total tour distance variable
                double tourDistance = 0;

                // Loop cities in the tour and calculate the distances 
                for (int i = 0; i < tour.Count - 1; i++)
                {
                    City cityFrom = tour[i];
                    City cityTo = tour[i + 1];

                    tourDistance += cityFrom.distanceTo(cityTo);
                }

                distance = tourDistance;

                return distance;
            }
            private set
            {
                distance = value;
            }
        }

        public double Fitness
        {
            get
            {
                if (fitness == 0)
                {
                    fitness = (1.0 / Distance) * 10000;
                }

                return fitness;
            }
            private set
            {
                fitness = value;
            }
        }

        public void ShuffleCities()
        {
            MyHelper.Shuffle(tour);
        }

        public int TourSize()
        {
            return tour.Count;
        }

        public bool ContainsCity(City city)
        {
            return tour.Contains(city);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("- |");

            for (int i = 0; i < tour.Count; i++)
            {
                sb.Append(tour[i].Id);
                sb.Append("|");
            }

            sb.AppendLine();
            sb.AppendLine($"- Distance: {Distance:f2}, Fitness: {Fitness:f2}");

            return sb.ToString();
        }
    }
}
