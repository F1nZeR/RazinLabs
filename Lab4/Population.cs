using System;
using System.Collections.Generic;

namespace Lab4
{
    public class Population : List<Tour>
    {
        private Tour _bestTour;
        public Tour BestTour
        {
            set { _bestTour = value; }
            get { return _bestTour; }
        }

        public void CreateRandomPopulation(int populationSize, Cities cityList, Random rand, int chanceToUseCloseCity)
        {
            for (var tourCount = 0; tourCount < populationSize; tourCount++)
            {
                var tour = new Tour(cityList.Count);

                var firstCity = rand.Next(cityList.Count);
                var lastCity = firstCity;

                for (var city = 0; city < cityList.Count - 1; city++)
                {
                    int nextCity;
                    do
                    {
                        if ((rand.Next(100) < chanceToUseCloseCity) && (cityList[city].CloseCities.Count > 0))
                        {
                            nextCity = cityList[city].CloseCities[rand.Next(cityList[city].CloseCities.Count)];
                        }
                        else
                        {
                            nextCity = rand.Next(cityList.Count);
                        }
                    } while ((tour[nextCity].Connection2 != -1) || (nextCity == lastCity));

                    tour[lastCity].Connection2 = nextCity;
                    tour[nextCity].Connection1 = lastCity;
                    lastCity = nextCity;
                }

                tour[lastCity].Connection2 = firstCity;
                tour[firstCity].Connection1 = lastCity;

                tour.DetermineFitness(cityList);

                Add(tour);

                if ((_bestTour == null) || (tour.Fitness < _bestTour.Fitness))
                {
                    BestTour = tour;
                }
            }
        }
    }
}