using System;
using System.Collections.Generic;

namespace Lab4
{
    public class Tour : List<Link>
    {
        public Tour(int capacity)
            : base(capacity)
        {
            ResetTour(capacity);
        }

        public double Fitness { get; set; }

        private void ResetTour(int numberOfCities)
        {
            Clear();
            
            for (int i = 0; i < numberOfCities; i++)
            {
                var link = new Link {Connection1 = -1, Connection2 = -1};
                Add(link);
            }
        }

        public void DetermineFitness(Cities cities)
        {
            Fitness = 0;

            var lastCity = 0;
            var nextCity = this[0].Connection1;

            for (int i = 0; i < Count; i++)
            {
                Fitness += cities[lastCity].Distances[nextCity];

                if (lastCity != this[nextCity].Connection1)
                {
                    lastCity = nextCity;
                    nextCity = this[nextCity].Connection1;
                }
                else
                {
                    lastCity = nextCity;
                    nextCity = this[nextCity].Connection2;
                }
            }
        }

        private static void JoinCities(Tour tour, int[] cityUsage, int city1, int city2)
        {
            if (tour[city1].Connection1 == -1)
            {
                tour[city1].Connection1 = city2;
            }
            else
            {
                tour[city1].Connection2 = city2;
            }

            if (tour[city2].Connection1 == -1)
            {
                tour[city2].Connection1 = city1;
            }
            else
            {
                tour[city2].Connection2 = city1;
            }

            cityUsage[city1]++;
            cityUsage[city2]++;
        }

        private static int FindNextCity(Tour parent, Tour child, Cities cityList, int[] cityUsage, int city)
        {
            if (TestConnectionValid(child, cityList, cityUsage, city, parent[city].Connection1))
            {
                return parent[city].Connection1;
            }
            if (TestConnectionValid(child, cityList, cityUsage, city, parent[city].Connection2))
            {
                return parent[city].Connection2;
            }

            return -1;
        }

        private static bool TestConnectionValid(Tour tour, Cities cityList, int[] cityUsage, int city1, int city2)
        {
            if ((city1 == city2) || (cityUsage[city1] == 2) || (cityUsage[city2] == 2))
            {
                return false;
            }

            if ((cityUsage[city1] == 0) || (cityUsage[city2] == 0))
            {
                return true;
            }

            for (int direction = 0; direction < 2; direction++)
            {
                var lastCity = city1;
                var currentCity = direction == 0 ? tour[city1].Connection1 : tour[city1].Connection2;
                var tourLength = 0;
                while ((currentCity != -1) && (currentCity != city2) && (tourLength < cityList.Count - 2))
                {
                    tourLength++;
                    if (lastCity != tour[currentCity].Connection1)
                    {
                        lastCity = currentCity;
                        currentCity = tour[currentCity].Connection1;
                    }
                    else
                    {
                        lastCity = currentCity;
                        currentCity = tour[currentCity].Connection2;
                    }
                }

                if (tourLength >= cityList.Count - 2)
                {
                    return true;
                }

                if (currentCity == city2)
                {
                    return false;
                }
            }

            return true;
        }

        public static Tour Crossover(Tour parent1, Tour parent2, Cities cityList, Random rand)
        {
            var child = new Tour(cityList.Count);
            var cityUsage = new int[cityList.Count];
            int nextCity;

            for (var city = 0; city < cityList.Count; city++)
            {
                cityUsage[city] = 0;
            }
            
            for (var city = 0; city < cityList.Count; city++)
            {
                if (cityUsage[city] < 2)
                {
                    if (parent1[city].Connection1 == parent2[city].Connection1)
                    {
                        nextCity = parent1[city].Connection1;
                        if (TestConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            JoinCities(child, cityUsage, city, nextCity);
                        }
                    }
                    if (parent1[city].Connection2 == parent2[city].Connection2)
                    {
                        nextCity = parent1[city].Connection2;
                        if (TestConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            JoinCities(child, cityUsage, city, nextCity);
                        }
                    }
                    if (parent1[city].Connection1 == parent2[city].Connection2)
                    {
                        nextCity = parent1[city].Connection1;
                        if (TestConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            JoinCities(child, cityUsage, city, nextCity);
                        }
                    }
                    if (parent1[city].Connection2 == parent2[city].Connection1)
                    {
                        nextCity = parent1[city].Connection2;
                        if (TestConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            JoinCities(child, cityUsage, city, nextCity);
                        }
                    }
                }
            }

            for (var city = 0; city < cityList.Count; city++)
            {
                if (cityUsage[city] < 2)
                {
                    if (city%2 == 1)
                    {
                        nextCity = FindNextCity(parent1, child, cityList, cityUsage, city);
                        if (nextCity == -1)
                        {
                            nextCity = FindNextCity(parent2, child, cityList, cityUsage, city);
                        }
                    }
                    else
                    {
                        nextCity = FindNextCity(parent2, child, cityList, cityUsage, city);
                        if (nextCity == -1)
                        {
                            nextCity = FindNextCity(parent1, child, cityList, cityUsage, city);
                        }
                    }

                    if (nextCity != -1)
                    {
                        JoinCities(child, cityUsage, city, nextCity);

                        if (cityUsage[city] == 1)
                        {
                            if (city%2 != 1)
                            {
                                nextCity = FindNextCity(parent1, child, cityList, cityUsage, city);
                                if (nextCity == -1)
                                {
                                    nextCity = FindNextCity(parent2, child, cityList, cityUsage, city);
                                }
                            }
                            else
                            {
                                nextCity = FindNextCity(parent2, child, cityList, cityUsage, city);
                                if (nextCity == -1)
                                {
                                    nextCity = FindNextCity(parent1, child, cityList, cityUsage, city);
                                }
                            }

                            if (nextCity != -1)
                            {
                                JoinCities(child, cityUsage, city, nextCity);
                            }
                        }
                    }
                }
            }

            for (var city = 0; city < cityList.Count; city++)
            {
                while (cityUsage[city] < 2)
                {
                    do
                    {
                        nextCity = rand.Next(cityList.Count);
                    } while (!TestConnectionValid(child, cityList, cityUsage, city, nextCity));

                    JoinCities(child, cityUsage, city, nextCity);
                }
            }

            return child;
        }

        public void Mutate(Random rand)
        {
            var cityNumber = rand.Next(Count);
            var link = this[cityNumber];
            int tmpCityNumber;
            
            // вытаскиваем
            if (this[link.Connection1].Connection1 == cityNumber)
            {
                if (this[link.Connection2].Connection1 == cityNumber)
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection1 = link.Connection1;
                    this[link.Connection1].Connection1 = tmpCityNumber;
                }
                else
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection2 = link.Connection1;
                    this[link.Connection1].Connection1 = tmpCityNumber;
                }
            }
            else
            {
                if (this[link.Connection2].Connection1 == cityNumber)
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection1 = link.Connection1;
                    this[link.Connection1].Connection2 = tmpCityNumber;
                }
                else
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection2 = link.Connection1;
                    this[link.Connection1].Connection2 = tmpCityNumber;
                }
            }

            int replaceCityNumber;
            do
            {
                replaceCityNumber = rand.Next(Count);
            } while (replaceCityNumber == cityNumber);
            var replaceLink = this[replaceCityNumber];

            // возвращаем
            tmpCityNumber = replaceLink.Connection2;
            link.Connection2 = replaceLink.Connection2;
            link.Connection1 = replaceCityNumber;
            replaceLink.Connection2 = cityNumber;

            if (this[tmpCityNumber].Connection1 == replaceCityNumber)
            {
                this[tmpCityNumber].Connection1 = cityNumber;
            }
            else
            {
                this[tmpCityNumber].Connection2 = cityNumber;
            }
        }
    }
}