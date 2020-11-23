using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstSteps
{
    public class Population
    {
        private List<Tour> tours;

        public Population()
        {
            tours = new List<Tour>();
        }

        public IReadOnlyCollection<Tour> Tours
        {
            get
            {
                return tours.AsReadOnly();
            }
        }

        public void AddTour(Tour tour)
        {
            tours.Add(tour);
        }

        public double GetBestTourDistance()
        {
            // Get distance of the tour with smallest distance
            double bestDistanceTour = tours.OrderBy(x => x.Distance).First().Distance;

            return bestDistanceTour;
        }

        public Tour GetBestTour()
        {
            return tours.OrderBy(x => x.Distance).First();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("--> Population:");

            foreach (var tour in tours.OrderByDescending(x => x.Fitness))
            {
                sb.AppendLine(tour.ToString());
            }


            return sb.ToString();
        }
    }
}
