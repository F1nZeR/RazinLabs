using System;
using System.Collections.Generic;

namespace Lab4
{
    public class Cities : List<City>
    {
        public void CalculateCityDistances(int numberOfCloseCities)
        {
            foreach (var city in this)
            {
                city.Distances.Clear();

                for (var i = 0; i < Count; i++)
                {
                    city.Distances.Add(Math.Sqrt(Math.Pow(city.Location.X - this[i].Location.X, 2) +
                                                 Math.Pow(city.Location.Y - this[i].Location.Y, 2)));
                }
            }

            foreach (var city in this)
            {
                city.FindClosestCities(numberOfCloseCities);
            }
        }
    }
}