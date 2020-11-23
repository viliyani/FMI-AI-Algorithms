using System;
using System.Collections.Generic;
using System.Linq;

namespace FirstSteps
{
    public class TSPSolver
    {
        private const int MAX_X = 150;
        private const int MAX_Y = 150;
        private const int MUTATION_PERCENT = 5;

        private int numberOfCities;
        private int populationSize;
        private List<City> allCities;
        private Population currentPopulation;
        private Random rand;

        public TSPSolver(int n)
        {
            numberOfCities = n;
            if (n < 4)
            {
                populationSize = n * 2;
            }
            else if (n < 10)
            {
                populationSize = n;
            }
            else
            {
                populationSize = n / 2;
            }
            allCities = new List<City>();
            currentPopulation = new Population();
            rand = new Random();
        }

        public void Solve()
        {
            // 1. Generate All N cities
            GenerateAllCities();

            // 2. Init First Population
            InitFirstPopulation();
        }

        public void GenerateAllCities()
        {
            // Generate all cities
            for (int cityId = 1; cityId <= numberOfCities; cityId++)
            {
                int x = rand.Next(1, MAX_X);
                int y = rand.Next(1, MAX_Y);

                allCities.Add(new City(cityId, x, y));
            }
        }

        public void InitFirstPopulation()
        {
            // Create N / 2 tours
            for (int i = 0; i < populationSize; i++)
            {
                // Add all cities to the tour
                Tour newTour = new Tour(new List<City>(allCities));

                // Shuffle the cities in the tour so the tour will be different from the other tours
                newTour.ShuffleCities();

                // Add the new tour to the population
                currentPopulation.AddTour(newTour);
            }
        }

        // Create new population by selecting the best parents and crossover them
        public void EvolvePopulation()
        {
            // Select 60% of the best parents
            List<Tour> bestParents = new List<Tour>();

            foreach (var item in currentPopulation.Tours.OrderBy(t => t.Distance).Take((int)(populationSize * 0.6)))
            {
                bestParents.Add(item); // add tour to the 60% bestParents
            }

            // List for the new generated childs
            List<Tour> newGeneratedChilds = new List<Tour>();

            // Crossover to create the childs
            for (int i = 0; i < bestParents.Count - 1; i++)
            {
                // Generate childs from parents: 0-1, 0-2, 1-2, 1-3, 2-3, 2-4, ...
                if (i + 1 < bestParents.Count)
                {
                    // parent1 = i, parent2 = i + 1
                    MakeOnePointCrossover(bestParents[i], bestParents[i + 1], newGeneratedChilds);
                }
                if (i + 2 < bestParents.Count)
                {
                    // parent1 = i, parent2 = i + 2
                    MakeOnePointCrossover(bestParents[i], bestParents[i + 2], newGeneratedChilds);
                }
            }

            // Mutate some childs
            for (int i = 0; i < newGeneratedChilds.Count * (MUTATION_PERCENT * 1.0 / 100); i++)
            {
                Mutate(newGeneratedChilds[i]);
            }

            // Add tours to the new poupulation => 30% of new population is from best parents and 70% is from best childs
            int t30p = (int)(currentPopulation.Tours.Count * 0.3);
            int t70c = currentPopulation.Tours.Count - t30p;

            List<Tour> toursForNewPopulation = new List<Tour>();

            // Create new population
            Population newPopulation = new Population();

            // Get and add 30% of best tours to new population
            foreach (var itemTour in currentPopulation.Tours.OrderBy(t => t.Distance).Take(t30p))
            {
                newPopulation.AddTour(itemTour);
            }

            // Get and add 70% of best childs to new population
            foreach (var itemTour in newGeneratedChilds.OrderBy(t => t.Distance).Take(t70c))
            {
                newPopulation.AddTour(itemTour);
            }

            // Set current population to be the new population
            currentPopulation = newPopulation;
        }

        // Make Crossover and add the two new childs to the newGeneratedChilds list
        public void MakeOnePointCrossover(Tour parent1, Tour parent2, List<Tour> newGeneratedChilds)
        {
            // Create lists for the childs
            List<City> child1 = new List<City>();
            List<City> child2 = new List<City>();

            // Get random number
            int randNumber = rand.Next(1, parent1.Cities.Count - 1);

            // Add first randNumber elements from parent 1 to child 1
            child1.AddRange(parent1.Cities.GetRange(0, randNumber));

            // Add first randNumber elements from parent 2 to child 2
            child2.AddRange(parent2.Cities.GetRange(0, randNumber));

            // Add remaining elements from parent 2 to child 1
            for (int i = 0; i < parent2.Cities.Count; i++)
            {
                if (!child1.Contains(parent2.Cities[i]))
                {
                    child1.Add(parent2.Cities[i]);
                }
            }

            // Add remaining elements from parent 1 to child 2
            for (int i = 0; i < parent1.Cities.Count; i++)
            {
                if (!child2.Contains(parent1.Cities[i]))
                {
                    child2.Add(parent1.Cities[i]);
                }
            }

            // Create tours from childs
            Tour newTour1 = new Tour(child1);
            Tour newTour2 = new Tour(child2);

            // Add new two tours to the childs list
            newGeneratedChilds.Add(newTour1);
            newGeneratedChilds.Add(newTour2);
        }

        public void Mutate(Tour tour)
        {
            // Get random indexes
            int randomIdx1 = rand.Next(0, tour.Cities.Count);
            int randomIdx2 = rand.Next(0, tour.Cities.Count);

            // Swap two cities in the tour
            City temp = tour.Cities[randomIdx1];
            tour.Cities[randomIdx1] = tour.Cities[randomIdx2];
            tour.Cities[randomIdx2] = temp;
        }

        public void PrintInfo()
        {
            Tour bestTour = currentPopulation.GetBestTour();

            Console.WriteLine();
            Console.WriteLine($"---> Best Distance: {bestTour.Distance:f2}");
            Console.WriteLine();
            Console.WriteLine("Tour Info: ");
            Console.WriteLine(bestTour.ToString());
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
