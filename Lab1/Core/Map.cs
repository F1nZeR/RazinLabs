using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab1.Core
{
    public class Map
    {
        public enum HeuristicEnum
        {
            Manhattan = 0,
            Diagonal = 1,
            Euclidean = 2
        }

        private readonly Canvas _drawCanvas;
        private NavigateNode[,] _mMap;
        private int _mRows, _mCols;
        private NavigateNode _startNode, _endNode;

        public double TotalWayCost { get; set; }
        public int DelayTime { get; set; }

        public Map(Canvas drawCanvas)
        {
            _drawCanvas = drawCanvas;
        }

        public void Init(int countX, int countY)
        {
            _mRows = countX;
            _mCols = countY;
            _mMap = new NavigateNode[_mRows, _mCols];

            for (int i = 0; i < _mRows; i++)
            {
                for (int j = 0; j < _mCols; j++)
                {
                    _mMap[i, j] = new NavigateNode(i, j);
                }
            }

            Draw();
        }

        private void Draw()
        {
            var sizeX = _drawCanvas.ActualWidth;
            var sizeY = _drawCanvas.ActualHeight;
            var btnSize = (int) Math.Min(sizeX/_mRows, sizeY/_mCols);

            _drawCanvas.Children.Clear();
            for (int i = 0; i < _mRows; i++)
            {
                for (int j = 0; j < _mCols; j++)
                {
                    var btnToAdd = _mMap[i, j].ViewCell;
                    btnToAdd.Width = btnSize;
                    btnToAdd.Height = btnSize;
                    Canvas.SetLeft(btnToAdd, i*btnSize);
                    Canvas.SetTop(btnToAdd, j*btnSize);

                    _drawCanvas.Children.Add(btnToAdd);
                }
            }
        }

        private void ClearPathes()
        {
            for (int i = 0; i < _mRows; i++)
            {
                for (int j = 0; j < _mCols; j++)
                {
                    switch (_mMap[i, j].State)
                    {
                        case NavigateNode.StateEnum.PATH:
                        case NavigateNode.StateEnum.OPEN:
                        case NavigateNode.StateEnum.CLOSE:
                            _mMap[i, j].State = NavigateNode.StateEnum.NAVIGABLE;
                            break;
                    }
                }
            }
        }

        public async Task FindPath(HeuristicEnum heuristic)
        {
            if (!IsValidMap())
            {
                MessageBox.Show("Карта невалидна!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            ClearPathes();
            await AstarRun(heuristic);
            //Draw();
        }

        private bool IsValidMap()
        {
            int countStarts = 0, countGoals = 0;
            for (int i = 0; i < _mRows; i++)
            {
                for (int j = 0; j < _mCols; j++)
                {
                    switch (_mMap[i,j].State)
                    {
                        case NavigateNode.StateEnum.START:
                            countStarts++;
                            _startNode = _mMap[i, j];
                            break;

                        case NavigateNode.StateEnum.GOAL:
                            _endNode = _mMap[i, j];
                            countGoals++;
                            break;
                    }
                }
            }

            return countStarts == 1 && countGoals == 1;
        }

        private bool IsInRange(int x, int y)
        {
            return (x >= 0 && x < _mRows && y >= 0 && y < _mCols);
        }

        private IEnumerable<NavigateNode> GetNeighbors(NavigateNode n)
        {
            int x = n.X, y = n.Y;
            var neighbors = new List<NavigateNode>();
            if (IsInRange(x - 1, y))
            {
                if (_mMap[x - 1, y].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x - 1, y]);
            }

            if (IsInRange(x + 1, y))
            {
                if (_mMap[x + 1, y].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x + 1, y]);
            }

            if (IsInRange(x, y - 1))
            {
                if (_mMap[x, y - 1].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x, y - 1]);
            }

            if (IsInRange(x, y + 1))
            {
                if (_mMap[x, y + 1].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x, y + 1]);
            }

            if (IsInRange(x - 1, y - 1))
            {
                if (_mMap[x - 1, y - 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x, y - 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x - 1, y].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x - 1, y - 1]);
            }

            if (IsInRange(x + 1, y - 1))
            {
                if (_mMap[x + 1, y - 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x, y - 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x + 1, y].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x + 1, y - 1]);
            }

            if (IsInRange(x - 1, y + 1))
            {
                if (_mMap[x - 1, y + 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x, y + 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x - 1, y].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x - 1, y + 1]);
            }

            if (IsInRange(x + 1, y + 1))
            {
                if (_mMap[x + 1, y + 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x, y + 1].State != NavigateNode.StateEnum.WALL &&
                    _mMap[x + 1, y].State != NavigateNode.StateEnum.WALL)
                    neighbors.Add(_mMap[x + 1, y + 1]);
            }

            return neighbors;
        }

        private async Task AstarRun(HeuristicEnum heuristic)
        {
            var openSet = new PriorityQueue<NavigateNode>();
            var closeSet = new PriorityQueue<NavigateNode>();

            openSet.Add(_startNode);

            while (!openSet.Empty)
            {
                var current = openSet.Pop();

                // добавляем в закрытый
                closeSet.Add(current);
                current.State = NavigateNode.StateEnum.CLOSE;

                // нашли конец
                if (current.IsSameLocation(_endNode))
                {
                    TotalWayCost = current.TotalCost;
                    current.State = NavigateNode.StateEnum.GOAL;
                    current = current.Parent;
                    while (current.Parent != null)
                    {
                        current.State = NavigateNode.StateEnum.PATH;
                        current = current.Parent;
                    }
                    current.State = NavigateNode.StateEnum.START;
                    return;
                }

                var neighbors = GetNeighbors(current);

                foreach (var n in neighbors.Where(n => !closeSet.IsMember(n)))
                {
                    if (!openSet.IsMember(n))
                    {
                        n.Parent = current;
                        n.DirectCost = current.DirectCost + GetDirectCost(current, n);
                        n.HeuristicCost = GetHeuristicCost(n, heuristic);
                        n.TotalCost = n.DirectCost + n.HeuristicCost;

                        // добавляем к открытому
                        openSet.Add(n);
                        n.State = NavigateNode.StateEnum.OPEN;
                    }
                    else
                    {
                        var costFromThisPathToM = current.DirectCost + GetDirectCost(current, n);
                        // есть лучший путь
                        if (costFromThisPathToM < n.DirectCost)
                        {
                            n.Parent = current;
                            n.DirectCost = costFromThisPathToM;
                            n.TotalCost = n.HeuristicCost + n.DirectCost;
                        }
                    }
                }
                await Task.Delay(DelayTime);
            }
        }

        private double GetDirectCost(NavigateNode n, NavigateNode m)
        {
            var temp = Math.Abs(n.X - m.X) + Math.Abs(n.Y - m.Y);
            return temp == 2 ? 14.0 : 10.0;
        }

        private double GetHeuristicCost(NavigateNode n, HeuristicEnum heuristic)
        {
            // http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
            const double d = 10;
            const double d2 = 14;
            double dx = Math.Abs(n.X - _endNode.X);
            double dy = Math.Abs(n.Y - _endNode.Y);
            
            switch (heuristic)
            {
                case HeuristicEnum.Manhattan:
                    return d*(dx + dy);
                    
                case HeuristicEnum.Diagonal:
                    return d*(dx + dy) + (d2 - 2*d)*Math.Min(dx, dy);

                case HeuristicEnum.Euclidean:
                    return d*Math.Sqrt(dx*dx + dy*dy);
            }

            throw new MissingMethodException("Запрошена неизвестная эвристика!");
        }
    }
}
