using System;

namespace Lab4
{
    public class TspEventArgs : EventArgs
    {
        private readonly Cities _cityList;
        public Cities CityList
        {
            get { return _cityList; }
        }

        private readonly Tour _bestTour;
        public Tour BestTour
        {
            get { return _bestTour; }
        }

        public int Generation { get; set; }
        public bool Complete { get; set; }
        
        public TspEventArgs(Cities cityList, Tour bestTour, int generation, bool complete)
        {
            _cityList = cityList;
            _bestTour = bestTour;
            Generation = generation;
            Complete = complete;
        }
    }
}