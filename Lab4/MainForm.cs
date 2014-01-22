using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace Lab4
{
    public partial class MainForm : Form
    {
        public delegate void DrawEventHandler(Object sender, TspEventArgs e);
        private readonly Cities _cityList = new Cities();
        private Graphics _cityGraphics;
        private Image _cityImage;
        private Tsp _tsp;

        public MainForm()
        {
            InitializeComponent();
        }

        private void tsp_foundNewBestTour(object sender, TspEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new DrawEventHandler(DrawTour), new[] { sender, e });
                return;
            }

            DrawTour(sender, e);
        }

        public void DrawTour(object sender, TspEventArgs e)
        {
            lastFitnessValue.Text = Math.Round(e.BestTour.Fitness, 2).ToString(CultureInfo.CurrentCulture);
            lastIterationValue.Text = e.Generation.ToString(CultureInfo.CurrentCulture);

            if (_cityImage == null)
            {
                _cityImage = new Bitmap(tourDiagram.Width, tourDiagram.Height);
                _cityGraphics = Graphics.FromImage(_cityImage);
            }

            var lastCity = 0;
            var nextCity = e.BestTour[0].Connection1;

            _cityGraphics.FillRectangle(Brushes.White, 0, 0, _cityImage.Width, _cityImage.Height);
            foreach (var city in e.CityList)
            {
                _cityGraphics.FillEllipse(Brushes.Black, city.Location.X - 4, city.Location.Y - 4, 8, 8);
                _cityGraphics.DrawLine(Pens.Black, _cityList[lastCity].Location, _cityList[nextCity].Location);

                if (lastCity != e.BestTour[nextCity].Connection1)
                {
                    lastCity = nextCity;
                    nextCity = e.BestTour[nextCity].Connection1;
                }
                else
                {
                    lastCity = nextCity;
                    nextCity = e.BestTour[nextCity].Connection2;
                }
            }

            tourDiagram.Image = _cityImage;

            if (e.Complete)
            {
                StartButton.Text = "Начать";
            }
        }

        private void DrawCityList(IEnumerable<City> cityList)
        {
            Image cityImage = new Bitmap(tourDiagram.Width, tourDiagram.Height);
            var graphics = Graphics.FromImage(cityImage);

            foreach (var city in cityList)
            {
                graphics.FillEllipse(Brushes.Black, city.Location.X - 4, city.Location.Y - 4, 8, 8);
            }

            tourDiagram.Image = cityImage;

            UpdateCityCount();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (_tsp != null)
            {
                _tsp.Halt = true;
                return;
            }

            StartButton.Text = "Остановить";
            ThreadPool.QueueUserWorkItem(state =>
            {
                var populationSize = Convert.ToInt32(populationSizeTextBox.Text);
                var maxGenerations = Convert.ToInt32(maxGenerationTextBox.Text);
                var mutation = Convert.ToInt32(mutationTextBox.Text);
                var groupSize = Convert.ToInt32(groupSizeTextBox.Text);
                const int seed = 0;
                var numberOfCloseCities = Convert.ToInt32(NumberCloseCitiesTextBox.Text);
                var chanceUseCloseCity = Convert.ToInt32(CloseCityOddsTextBox.Text);

                _cityList.CalculateCityDistances(numberOfCloseCities);

                _tsp = new Tsp();
                _tsp.FoundNewBestTour += tsp_foundNewBestTour;
                _tsp.Begin(populationSize, maxGenerations, groupSize, mutation, seed, chanceUseCloseCity, _cityList);
                _tsp.FoundNewBestTour -= tsp_foundNewBestTour;
                _tsp = null;
            });
        }
        
        private void clearCityListButton_Click(object sender, EventArgs e)
        {
            _cityList.Clear();
            DrawCityList(_cityList);
        }

        private void tourDiagram_MouseDown(object sender, MouseEventArgs e)
        {
            _cityList.Add(new City(e.X, e.Y));
            DrawCityList(_cityList);
        }

        private void UpdateCityCount()
        {
            NumberCitiesValue.Text = _cityList.Count.ToString();
        }
    }
}