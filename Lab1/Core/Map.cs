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
        public bool IsDiagonalMoveEnabled { get; set; }

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
            var btnSize = (int)Math.Min(sizeX / _mRows, sizeY / _mCols);

            _drawCanvas.Children.Clear();
            for (int i = 0; i < _mRows; i++)
            {
                for (int j = 0; j < _mCols; j++)
                {
                    var btnToAdd = _mMap[i, j].ViewCell;
                    btnToAdd.Width = btnSize;
                    btnToAdd.Height = btnSize;
                    Canvas.SetLeft(btnToAdd, i * btnSize);
                    Canvas.SetTop(btnToAdd, j * btnSize);

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
                    switch (_mMap[i, j].State)
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

        private bool IsGoodToGoDiagonal(int origPosX, int origPosY, int x, int y)
        {
            return _mMap[origPosX, y].State != NavigateNode.StateEnum.WALL && _mMap[x, origPosY].State != NavigateNode.StateEnum.WALL;
        }

        private IEnumerable<NavigateNode> GetNeighbors(NavigateNode n)
        {
            int x = n.X, y = n.Y;
            var neighbors = new List<NavigateNode>();

            for (int ix = x-1; ix <= x+1; ix++)
            {
                for (int jy = y-1; jy <= y+1; jy++)
                {
                    if (!IsInRange(ix, jy) || (x == ix && y == jy) ||
                        (_mMap[ix, jy].State == NavigateNode.StateEnum.WALL) || (!IsDiagonalMoveEnabled && !IsGoodToGoDiagonal(x, y, ix, jy)))
                    {
                        continue;
                    }
                    neighbors.Add(_mMap[ix, jy]);
                }
            }

            return neighbors;
        }

        private async Task AstarRun(HeuristicEnum heuristic)
        {
            var openSet = new List<NavigateNode>();
            var closeSet = new List<NavigateNode>();

            openSet.Add(_startNode);
            _startNode.DirectCost = 0;
            _startNode.HeuristicCost = GetHeuristicCost(_startNode, heuristic);
            _startNode.TotalCost = _startNode.DirectCost + _startNode.HeuristicCost;

            while (openSet.Any())
            {
                var curNode = openSet.Last(z => z.TotalCost.Equals(openSet.Min(y => y.TotalCost)));
                openSet.Remove(curNode);

                closeSet.Add(curNode);
                curNode.State = NavigateNode.StateEnum.CLOSE;

                if (curNode.IsSameLocation(_endNode))
                {
                    TotalWayCost = curNode.TotalCost;
                    curNode.State = NavigateNode.StateEnum.GOAL;
                    curNode = curNode.Parent;
                    while (curNode.Parent != null)
                    {
                        curNode.State = NavigateNode.StateEnum.PATH;
                        curNode = curNode.Parent;
                    }
                    curNode.State = NavigateNode.StateEnum.START;
                    return;
                }

                foreach (var curNeigbNode in GetNeighbors(curNode).Where(n => !closeSet.Contains(n)))
                {
                    bool isCurNodeBetterChoice;
                    var curDirectCost = curNode.DirectCost + GetDirectCost(curNode, curNeigbNode);
                    if (!openSet.Contains(curNeigbNode))
                    {
                        openSet.Add(curNeigbNode);
                        curNeigbNode.State = NavigateNode.StateEnum.OPEN;
                        isCurNodeBetterChoice = true;
                    }
                    else
                    {
                        isCurNodeBetterChoice = curDirectCost < curNeigbNode.DirectCost;
                    }

                    if (isCurNodeBetterChoice)
                    {
                        curNeigbNode.Parent = curNode;
                        curNeigbNode.DirectCost = curDirectCost;
                        curNeigbNode.HeuristicCost = GetHeuristicCost(curNeigbNode, heuristic);
                        curNeigbNode.TotalCost = curNeigbNode.DirectCost + curNeigbNode.HeuristicCost;
                    }
                }
                await Task.Delay(DelayTime);
            }
        }

        private static double GetDirectCost(NavigateNode n, NavigateNode m)
        {
            var temp = Math.Abs(n.X - m.X) + Math.Abs(n.Y - m.Y);
            return temp == 2 ? 14.0 : 10.0;
        }

        private double GetHeuristicCost(NavigateNode n, HeuristicEnum heuristic)
        {
            const double d = 10;
            const double d2 = 14;
            double dx = Math.Abs(n.X - _endNode.X);
            double dy = Math.Abs(n.Y - _endNode.Y);

            switch (heuristic)
            {
                case HeuristicEnum.Manhattan:
                    return d * (dx + dy);

                case HeuristicEnum.Diagonal:
                    return d * (dx + dy) + (d2 - 2 * d) * Math.Min(dx, dy);

                case HeuristicEnum.Euclidean:
                    return d * Math.Sqrt(dx * dx + dy * dy);
            }

            throw new MissingMethodException("Запрошена неизвестная эвристика!");
        }
    }
}
