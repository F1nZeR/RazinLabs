using System;

namespace Lab4
{
    public class Tsp
    {
        public delegate void NewBestTourEventHandler(Object sender, TspEventArgs e);
        private Cities _cityList;
        private Population _population;
        private Random _rand;
        public bool Halt { get; set; }
        public event NewBestTourEventHandler FoundNewBestTour;

        public void Begin(int populationSize, int maxGenerations, int groupSize, int mutation, int seed,
            int chanceToUseCloseCity, Cities cityList)
        {
            _rand = new Random(seed);

            _cityList = cityList;

            _population = new Population();
            _population.CreateRandomPopulation(populationSize, cityList, _rand, chanceToUseCloseCity);

            DisplayTour(_population.BestTour, 0, false);

            int generation;
            for (generation = 0; generation < maxGenerations; generation++)
            {
                if (Halt) break;

                var foundNewBestTour = MakeChildren(groupSize, mutation);

                if (foundNewBestTour)
                {
                    DisplayTour(_population.BestTour, generation, false);
                }
            }

            DisplayTour(_population.BestTour, generation, true);
        }

        private bool MakeChildren(int groupSize, int mutation)
        {
            var tourGroup = new int[groupSize];

            for (var tourCount = 0; tourCount < groupSize; tourCount++)
            {
                tourGroup[tourCount] = _rand.Next(_population.Count);
            }

            for (var tourCount = 0; tourCount < groupSize - 1; tourCount++)
            {
                var topTour = tourCount;
                for (int i = topTour + 1; i < groupSize; i++)
                {
                    if (_population[tourGroup[i]].Fitness < _population[tourGroup[topTour]].Fitness)
                    {
                        topTour = i;
                    }
                }

                if (topTour != tourCount)
                {
                    var tempTour = tourGroup[tourCount];
                    tourGroup[tourCount] = tourGroup[topTour];
                    tourGroup[topTour] = tempTour;
                }
            }

            var foundNewBestTour = false;
            var childPosition = tourGroup[groupSize - 1];
            _population[childPosition] = Tour.Crossover(_population[tourGroup[0]], _population[tourGroup[1]], _cityList,
                _rand);
            if (_rand.Next(100) < mutation)
            {
                _population[childPosition].Mutate(_rand);
            }
            _population[childPosition].DetermineFitness(_cityList);

            if (_population[childPosition].Fitness < _population.BestTour.Fitness)
            {
                _population.BestTour = _population[childPosition];
                foundNewBestTour = true;
            }

            childPosition = tourGroup[groupSize - 2];
            _population[childPosition] = Tour.Crossover(_population[tourGroup[1]], _population[tourGroup[0]], _cityList,
                _rand);
            if (_rand.Next(100) < mutation)
            {
                _population[childPosition].Mutate(_rand);
            }
            _population[childPosition].DetermineFitness(_cityList);

            if (_population[childPosition].Fitness < _population.BestTour.Fitness)
            {
                _population.BestTour = _population[childPosition];
                foundNewBestTour = true;
            }

            return foundNewBestTour;
        }

        private void DisplayTour(Tour bestTour, int generationNumber, bool complete)
        {
            if (FoundNewBestTour != null)
            {
                FoundNewBestTour(this, new TspEventArgs(_cityList, bestTour, generationNumber, complete));
            }
        }
    }
}