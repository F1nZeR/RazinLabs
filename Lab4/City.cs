using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lab4
{
    public class City
    {
        private readonly List<int> _closeCities = new List<int>();
        private List<double> _distances = new List<double>();
        private Point _location;

        public City(int x, int y)
        {
            Location = new Point(x, y);
        }

        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public List<double> Distances
        {
            get { return _distances; }
            set { _distances = value; }
        }

        public List<int> CloseCities
        {
            get { return _closeCities; }
        }

        public void FindClosestCities(int numberOfCloseCities)
        {
            var shortestCity = 0;
            var dist = new double[Distances.Count];
            Distances.CopyTo(dist);

            if (numberOfCloseCities > Distances.Count - 1)
            {
                numberOfCloseCities = Distances.Count - 1;
            }

            _closeCities.Clear();

            for (var i = 0; i < numberOfCloseCities; i++)
            {
                var shortestDistance = Double.MaxValue;
                for (var cityNum = 0; cityNum < Distances.Count; cityNum++)
                {
                    if (dist[cityNum] < shortestDistance)
                    {
                        shortestDistance = dist[cityNum];
                        shortestCity = cityNum;
                    }
                }
                _closeCities.Add(shortestCity);
                dist[shortestCity] = Double.MaxValue;
            }
        }
    }
}