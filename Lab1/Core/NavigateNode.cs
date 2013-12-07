using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lab1.Core
{
    public class NavigateNode : IComparable<NavigateNode>
    {
        public enum StateEnum
        {
            WALL,
            NAVIGABLE,
            PATH,
            GOAL,
            START,
            OPEN,
            CLOSE
        };

        private readonly int _mX;
        private readonly int _mY;
        private double _mTotalCost;
        private StateEnum _state;

        #region

        public Rectangle ViewCell { get; set; }

        public StateEnum State
        {
            get { return _state; }
            set
            {
                _state = value;
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal, (ThreadStart)UpdateViewCellColor);
            }
        }

        public double DirectCost { get; set; }

        public double HeuristicCost { get; set; }

        public double TotalCost
        {
            get { return _mTotalCost; }
            set
            {
                _mTotalCost = value;
                ViewCell.ToolTip = TotalCost;
            }
        }

        public int X
        {
            get { return _mX; }
        }

        public int Y
        {
            get { return _mY; }
        }

        public NavigateNode Parent { get; set; }

        #endregion
        
        public NavigateNode(int x, int y, StateEnum state = StateEnum.NAVIGABLE)
        {
            _mX = x;
            _mY = y;

            ViewCell = new Rectangle {Stroke = Brushes.Black};
            ViewCell.MouseDown += ViewCellOnClick;
            ViewCell.MouseMove += ViewCellOnMouseMove;

            State = state;
            DirectCost = 0.0;
            HeuristicCost = 0.0;
            _mTotalCost = 0.0;
            Parent = null;
        }

        private void ViewCellOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.RightButton == MouseButtonState.Pressed)
            {
                State = StateEnum.WALL;
            }
        }

        private void ViewCellOnClick(object sender, MouseButtonEventArgs args)
        {
            if (args.ChangedButton == MouseButton.Right)
            {
                State = StateEnum.NAVIGABLE;
                return;
            }

            switch (State)
            {
                case StateEnum.NAVIGABLE:
                    State = StateEnum.START;
                    break;

                case StateEnum.START:
                    State = StateEnum.GOAL;
                    break;

                case StateEnum.GOAL:
                case StateEnum.PATH:
                    State = StateEnum.NAVIGABLE;
                    break;
            }
        }

        public int CompareTo(NavigateNode n)
        {
            if (_mTotalCost < n._mTotalCost)
                return -1;
            if (_mTotalCost > n._mTotalCost)
                return 1;
            return 0;
        }

        public bool IsSameLocation(NavigateNode n)
        {
            return (X == n.X && Y == n.Y);
        }

        private void UpdateViewCellColor()
        {
            switch (State)
            {
                case StateEnum.NAVIGABLE:
                    ViewCell.Fill = Brushes.WhiteSmoke;
                    break;
                case StateEnum.START:
                    ViewCell.Fill = Brushes.Green;
                    break;
                case StateEnum.GOAL:
                    ViewCell.Fill = Brushes.Blue;
                    break;
                case StateEnum.WALL:
                    ViewCell.Fill = Brushes.Black;
                    break;
                case StateEnum.PATH:
                    ViewCell.Fill = Brushes.Red;
                    break;
                case StateEnum.OPEN:
                    ViewCell.Fill = Brushes.LightGray;
                    break;
                case StateEnum.CLOSE:
                    ViewCell.Fill = Brushes.DimGray;
                    break;
            }
        }
    }
}