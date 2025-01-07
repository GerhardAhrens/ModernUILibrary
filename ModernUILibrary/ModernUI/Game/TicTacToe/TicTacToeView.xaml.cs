//-----------------------------------------------------------------------
// <copyright file="TicTacToeView.xaml.cs" company="Lifeprojects.de">
//     Class: TicTacToeView
//     Copyright © Lifeprojects.de GmbH 2015
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.02.2015</date>
//
// <summary>
// Spiel Tic Tac Toe
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TicTacToeView : Window
    {
        private const byte FILLED_WITH_CIRCLE = 1;
        private const byte FILLED_WITH_CROSS = 2;

        private ControlTemplate _crossTemplate = new ControlTemplate();
        private ControlTemplate _circleTemplate;
        private readonly CrossDC _corssDataContext = new CrossDC(45, -45);
        private readonly CircleDC _circleDataContext = new CircleDC(0);
        private readonly int[][] _matches = new int[][] 
        {
            new int[]{0,1,2},
            new int[]{3,4,5},
            new int[]{6,7,8},
            new int[]{0,3,6},
            new int[]{1,4,7},
            new int[]{2,5,8},
            new int[]{0,4,8},
            new int[]{2,4,6}
            };

        private readonly int[][][] _goodStart = new int[][][]
        {
            new int[][]  {new int[]{4} ,new int[]{0,2,6,8}},
            new int[][]  {new int[]{1} ,new int[]{6,8} ,new int[]{0,2}},
            new int[][]  {new int[]{7} ,new int[]{0,2} ,new int[]{6,8}},
            new int[][]  {new int[]{0} ,new int[]{4,8,6,2}}
        };

        private int _myLastGoodStart = 0;
        private int _numOfPlayers = 2;
        private bool _itsMyTurn = false;
        private bool _toBeStartedByMe = false;
        private bool _isGameOver = false;
        private bool _drawCircle = true;
        private int _numFilled = 0;

        private readonly Cell[] _Cells = new Cell[9];

        public TicTacToeView(Window owner = null)
        {
            this.InitializeComponent();

            if (owner != null)
            {
                this.Owner = owner;
            }
            else
            {
                this.Owner = Application.Current.Windows.LastActiveWindow();
            }

            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, KeyEventArgs>.AddHandler(this, "KeyUp", this.OnKeyUp);

            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.ExitBtn, "Click", this.OnClose);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.OnePlayerBtn, "Click", this.OnOnePlayer);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.TwoPlayerBtn, "Click", this.OnTwoPlayer);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.NewGameBtn, "Click", this.OnNewGame);

            this.OnOnePlayer(null, null);
        }

        public static new bool? Show()
        {
            return Create(null).Display();
        }

        private static TicTacToeView Create(Window owner)
        {
            return new TicTacToeView(owner);
        }

        private bool? Display()
        {
            return this.ShowDialog(); ;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.MainGrid.Height = this.ActualHeight - ButtonGrid.ActualHeight - 10;
            this.MainGrid.Width = this.ActualWidth - 10;

            this._crossTemplate = this.MainGrid.TryFindResource("BlueCross") as ControlTemplate;
            this._circleTemplate = this.MainGrid.TryFindResource("TheCircleTemplate") as ControlTemplate;

            Control[] cells = new Control[] { B1, B2, B3, B4, B5, B6, B7, B8, B9 };
            for (int i = 0; i < this._Cells.Length; i++)
            {
                this._Cells[i] = new Cell(cells[i]);
            }

            this.Cursor = Cursors.Cross;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnNewGame(object sender, RoutedEventArgs e)
        {
            if (this._numOfPlayers > 1)
            {
                this._itsMyTurn = false;
            }

            this._drawCircle = true;
            this._isGameOver = false;
            this._numFilled = 0;
            this.TB_GameOver.Visibility = Visibility.Hidden;

            foreach (Cell c in _Cells)
            {
                if (c != null)
                {
                    c.border.Child = null;
                    c.filledWith = 0;
                    (c.control.DataContext as BorderDC).IsBlinking = false;
                }
            }

            this._corssDataContext.Angle1 = 45;
            this._corssDataContext.Angle2 = -45;
            this._corssDataContext.IsRotating = false;
            this._circleDataContext.IsRotating = false;

            this.Cursor = Cursors.Cross;

            if (this._numOfPlayers == 1 && _toBeStartedByMe)
            {
                this.ItsMyTurn();
            }
            else
            {
                this._itsMyTurn = false;
            }
        }

        private void OnOnePlayer(object sender, RoutedEventArgs e)
        {

            this.OnePlayerBtn.Background = Brushes.Green;
            this.TwoPlayerBtn.Background = Brushes.LightGray;
            this._numOfPlayers = 1;
            this._itsMyTurn = false;
            this._toBeStartedByMe = false;
            this.OnNewGame(sender, e);
        }

        private void OnTwoPlayer(object sender, RoutedEventArgs e)
        {
            this.TwoPlayerBtn.Background = Brushes.Green;
            this.OnePlayerBtn.Background = Brushes.LightGray;
            this._numOfPlayers = 2;
            this._itsMyTurn = false;
            this._toBeStartedByMe = false;
            this.OnNewGame(sender, e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    this.OnNewGame(null, null);
                    break;

                case Key.System:
                    {
                        switch (e.SystemKey)
                        {
                            case Key.N:
                                this.OnNewGame(null, null);
                                break;

                            case Key.D1:
                                this.OnOnePlayer(null, null);
                                break;

                            case Key.D2:
                                this.OnTwoPlayer(null, null);
                                break;

                            case Key.X:
                                this.OnClose(null, null);
                                break;
                        }
                    }

                    break;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.ActualHeight < this.MinHeight)
            {
                this.Height = this.MinHeight;
            }

            if (this.ActualWidth < this.MinWidth)
            {
                this.Width = this.MinWidth;
            }

            this.MainGrid.Height = this.ActualHeight - this.ButtonGrid.ActualHeight - 10;
            this.MainGrid.Width = this.ActualWidth - 10;

            this.MainGrid.UpdateLayout();

            foreach (Cell c in this._Cells)
            {
                if (c != null)
                {
                    if (c.border.Child != null && c.border.Child is Control)
                    {
                        this.SetSize(c.border.Child);
                    }
                }
            }
        }

        private void SetSize(object obj)
        {
            Control c = obj as Control;
            if (c.DataContext is CircleDC)
            {
                this.SetCircleSize(c);
            }
            else if (c.DataContext is CrossDC)
            {
                this.SetCrossSize(c);
            }
        }

        private void SetCircleSize(Control c)
        {
            double h = (B1.ActualWidth < B1.ActualHeight) ? B1.ActualWidth : B1.ActualHeight;
            c.Height = h / 1.25;
            c.Width = c.Height;
            (c.DataContext as CircleDC).RadiousXY = c.Height / 2;
            c.UpdateLayout();
        }

        private void SetCrossSize(Control c)
        {
            double w = (B1.ActualWidth < B1.ActualHeight) ? B1.ActualWidth : B1.ActualHeight;
            c.Width = w / 1.25;
            base.UpdateLayout();
        }

        private Control CreateCircle()
        {
            Control theCircle = new Control();

            theCircle.DataContext = this._circleDataContext;
            SetCircleSize(theCircle);
            theCircle.Template = this._circleTemplate;

            return theCircle;
        }

        private Control CreateCross()
        {
            Control control = new Control();

            control.DataContext = this._corssDataContext;
            SetCrossSize(control);
            control.Template = this._crossTemplate;

            return control;
        }

        private bool CheckIfGameOver()
        {
            this._isGameOver = false;

            List<int> matchedCells = new List<int>();

            foreach (int[] m in this._matches)
            {
                if ((this._Cells[m[0]].filledWith & this._Cells[m[1]].filledWith & this._Cells[m[2]].filledWith) > 0)
                {
                    if (this._numOfPlayers > 1)
                    {
                        this.TB_GameOver.Text = "Game Over!";
                    }
                    else
                    {
                        this.TB_GameOver.Text = (!this._itsMyTurn) ? "Du hast gewonnen!" : "Computer hat gewonnen!";
                    }

                    this.TB_GameOver.Visibility = Visibility.Visible;
                    if (!matchedCells.Contains(m[0]))
                    {
                        matchedCells.Add(m[0]);
                    }

                    if (!matchedCells.Contains(m[1]))
                    {
                        matchedCells.Add(m[1]);
                    }

                    if (!matchedCells.Contains(m[2]))
                    {
                        matchedCells.Add(m[2]);
                    }
                }
            }

            if (matchedCells.Count > 0)
            {
                foreach (int cIndex in matchedCells)
                {
                    (this._Cells[cIndex].control.DataContext as BorderDC).IsBlinking = true;
                }

                if (this._Cells[matchedCells[0]].filledWith == FILLED_WITH_CIRCLE)
                {
                    this._circleDataContext.IsRotating = true;
                }
                else
                {
                    this._corssDataContext.IsRotating = true;
                }

                this._isGameOver = true;
                this._toBeStartedByMe = !this._toBeStartedByMe;
            }

            if (this._numFilled > 8 && this._isGameOver == false)
            {
                foreach (Cell c in this._Cells)
                {
                    (c.control.DataContext as BorderDC).IsBlinking = true;
                }

                this.TB_GameOver.Text = "It's a Draw!";
                this.TB_GameOver.Visibility = Visibility.Visible;
                this._toBeStartedByMe = !this._toBeStartedByMe;
            }

            return this._isGameOver;
        }

        private bool AddChildIfEmpty(Border b)
        {
            if (b.Child != null)
            {
                return false;
            }

            Control c = b.TemplatedParent as Control;
            int index = c.Name[1] - 49;

            if (this._drawCircle)
            {
                b.Child = CreateCircle();
                b.UpdateLayout();
                this._Cells[index].filledWith = FILLED_WITH_CIRCLE;
            }
            else
            {
                b.Child = this.CreateCross();
                b.UpdateLayout();
                this._Cells[index].filledWith = FILLED_WITH_CROSS;
            }

            this._numFilled++;

            if (this._numFilled > 4)
            {
                if (this.CheckIfGameOver())
                {
                    this.Cursor = Cursors.Arrow;
                    return true;
                }
            }

            this._drawCircle = !this._drawCircle;

            if (this._drawCircle)
            {
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                this.Cursor = Cursors.Cross;
            }

            if (this._numOfPlayers == 1)
            {
                this._itsMyTurn = !this._itsMyTurn;
            }

            return true;
        }

        private void ItsMyTurn()
        {
            this._itsMyTurn = true;

            if (this._numFilled == 0)
                if ((++this._myLastGoodStart) >= this._goodStart.GetLength(0)) this._myLastGoodStart = 0;

            int selectedCell = -1;

            if (this._numFilled > 2)
            {
                if (this._drawCircle)
                {
                    foreach (int[] m in this._matches)
                    {
                        if (((this._Cells[m[0]].filledWith & this._Cells[m[1]].filledWith) == FILLED_WITH_CIRCLE) && this._Cells[m[2]].filledWith == 0)
                        {
                            selectedCell = m[2];
                        }
                        else if (((this._Cells[m[0]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CIRCLE) && this._Cells[m[1]].filledWith == 0)
                        {
                            selectedCell = m[1];
                        }
                        else if (((this._Cells[m[1]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CIRCLE) && this._Cells[m[0]].filledWith == 0)
                        {
                            selectedCell = m[0];
                        }

                        if (selectedCell >= 0)
                        {
                            this.AddChildIfEmpty(this._Cells[selectedCell].border);
                            return;
                        }
                    }

                    foreach (int[] m in this._matches)
                    {
                        if (((this._Cells[m[0]].filledWith & this._Cells[m[1]].filledWith) == FILLED_WITH_CROSS) && this._Cells[m[2]].filledWith == 0)
                        {
                            selectedCell = m[2];
                        }
                        else if (((this._Cells[m[0]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CROSS) && this._Cells[m[1]].filledWith == 0)
                        {
                            selectedCell = m[1];
                        }
                        else if (((this._Cells[m[1]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CROSS) && this._Cells[m[0]].filledWith == 0)
                        {
                            selectedCell = m[0];
                        }

                        if (selectedCell >= 0)
                        {
                            this.AddChildIfEmpty(this._Cells[selectedCell].border);
                            return;
                        }
                    }
                }
                else
                {
                    foreach (int[] m in this._matches)
                    {
                        if (((this._Cells[m[0]].filledWith & this._Cells[m[1]].filledWith) == FILLED_WITH_CROSS) && this._Cells[m[2]].filledWith == 0)
                        {
                            selectedCell = m[2];
                        }
                        else if (((this._Cells[m[0]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CROSS) && this._Cells[m[1]].filledWith == 0)
                        {
                            selectedCell = m[1];
                        }
                        else if (((this._Cells[m[1]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CROSS) && this._Cells[m[0]].filledWith == 0)
                        {
                            selectedCell = m[0];
                        }

                        if (selectedCell >= 0)
                        {
                            this.AddChildIfEmpty(this._Cells[selectedCell].border);
                            return;
                        }
                    }
                    foreach (int[] m in this._matches)
                    {
                        if (((this._Cells[m[0]].filledWith & this._Cells[m[1]].filledWith) == FILLED_WITH_CIRCLE) && this._Cells[m[2]].filledWith == 0)
                        {
                            selectedCell = m[2];
                        }
                        else if (((this._Cells[m[0]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CIRCLE) && this._Cells[m[1]].filledWith == 0)
                        {
                            selectedCell = m[1];
                        }
                        else if (((this._Cells[m[1]].filledWith & this._Cells[m[2]].filledWith) == FILLED_WITH_CIRCLE) && this._Cells[m[0]].filledWith == 0)
                        {
                            selectedCell = m[0];
                        }

                        if (selectedCell >= 0)
                        {
                            this.AddChildIfEmpty(this._Cells[selectedCell].border);
                            return;
                        }
                    }

                    if (_numFilled == 3)
                    {
                        if ((this._Cells[0].filledWith & this._Cells[8].filledWith) == FILLED_WITH_CIRCLE ||
                            (this._Cells[2].filledWith & this._Cells[6].filledWith) == FILLED_WITH_CIRCLE)
                        {
                            if (AddChildIfEmpty(this._Cells[1].border) == false)
                            {
                                this.AddChildIfEmpty(this._Cells[7].border);
                            }

                            return;
                        }
                    }
                }
            }

            if (_toBeStartedByMe && _numFilled < 3)
            {
                if (this._goodStart[this._myLastGoodStart].GetLength(0) >= _numFilled / 2)
                {
                    int[] probales = this._goodStart[this._myLastGoodStart][_numFilled / 2];

                    foreach (int i in probales)
                    {
                        if (this._Cells[i].filledWith == 0)
                        {
                            this.AddChildIfEmpty(this._Cells[i].border);
                            return;
                        }
                    }
                }
            }


            if (this._Cells[4].filledWith == 0)
            {
                selectedCell = 4;
            }
            else if (this._Cells[0].filledWith == 0)
            {
                selectedCell = 0;
            }
            else if (this._Cells[2].filledWith == 0)
            {
                selectedCell = 2;
            }

            if (selectedCell >= 0)
            {
                this.AddChildIfEmpty(this._Cells[selectedCell].border);
                return;
            }

            foreach (Cell c in this._Cells)
            {
                if (c.filledWith == 0)
                {
                    this.AddChildIfEmpty(c.border);
                    return;
                }
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this._isGameOver || this._itsMyTurn)
            {
                return;
            }

            this.Cursor = Cursors.Cross;

            if (sender is Border)
            {
                if (this.AddChildIfEmpty(sender as Border) == true && _isGameOver == false)
                {
                    if (this._numOfPlayers == 1)
                    {
                        this.ItsMyTurn();
                    }
                }
            }
        }

        private class Cell
        {
            public Control control;
            public Border border = null;
            public byte filledWith = 0;

            public Cell(Control c)
            {
                c.DataContext = new BorderDC();
                control = c;

                Border b = c.Template.FindName("theBorder", c) as Border;
                if (b != null)
                {
                    border = b;
                }
                else
                {
                    border = null;
                }
            }
        }

        private class BorderDC : MyDataContexts
        {
            private bool _isBlinking = false;

            public bool IsBlinking
            {
                get { return this._isBlinking; }
                set
                {
                    if (this._isBlinking != value)
                    {
                        this._isBlinking = value;
                        this.OnPropertyChanged();
                    }
                }
            }
        }

        private class CrossDC : MyDataContexts
        {
            private double _angle1 = 45;
            private double _angle2 = -45;
            private bool _isRotating = false;

            public CrossDC(double angle1, double angle2)
            {
                this._angle1 = angle1;
                this._angle2 = angle2;
            }

            public double Angle1
            {
                get { return this._angle1; }
                set
                {
                    if (this._angle1 != value)
                    {
                        this._angle1 = value;
                        this.OnPropertyChanged();
                    }
                }
            }
            public double Angle2
            {
                get { return this._angle2; }
                set
                {
                    if (this._angle2 != value)
                    {
                        this._angle2 = value;
                        this.OnPropertyChanged();
                    }
                }
            }

            public bool IsRotating
            {
                get { return this._isRotating; }
                set
                {
                    this._isRotating = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private class CircleDC : MyDataContexts
        {
            private double _radiousXY = 0;
            private bool _isRotating = false;

            public CircleDC(double radiousXY)
            {
                this._radiousXY = radiousXY;
            }

            public double RadiousXY
            {
                get { return this._radiousXY; }
                set
                {
                    if (this._radiousXY != value)
                    {
                        this._radiousXY = value;
                        this.OnPropertyChanged();
                    }
                }
            }

            public bool IsRotating
            {
                get { return this._isRotating; }
                set
                {
                    this._isRotating = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private abstract class MyDataContexts : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
