using ChessBoard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ChessBoard
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private Board _board = new Board();
        private ICommand newGameCommand;     
        private ICommand cellCommand;
        private int currentPlayer;
        private int coordinateXOfFigure;
        private int coordinateYOfFigure;
        private int countOfEnemyFugures;

        public IEnumerable<char> Numbers => "87654321";
        public IEnumerable<char> Letters => "ABCDEFGH";

        public MainViewModel()
        {
            using (BoardContext context = new BoardContext())
            {
                Cell FirstCell = context.Cells.FirstOrDefault();
                Player player = context.Players.FirstOrDefault();
                if (FirstCell == null)
                {
                    context.Players.Add(new Player() { CurrentPlayer = 1 });
                    currentPlayer = 1;

                    Cell cell_1 = new Cell() { CoordinateX = 0, CoordinateY = 0, Active = false, State = State.BlackRook};
                    Cell cell_2 = new Cell() { CoordinateX = 0, CoordinateY = 1, Active = false, State = State.BlackKnight };
                    Cell cell_3 = new Cell() { CoordinateX = 0, CoordinateY = 2, Active = false, State = State.BlackBishop };
                    Cell cell_4 = new Cell() { CoordinateX = 0, CoordinateY = 3, Active = false, State = State.BlackQueen };
                    Cell cell_5 = new Cell() { CoordinateX = 0, CoordinateY = 4, Active = false, State = State.BlackKing };
                    Cell cell_6 = new Cell() { CoordinateX = 0, CoordinateY = 5, Active = false, State = State.BlackBishop };
                    Cell cell_7 = new Cell() { CoordinateX = 0, CoordinateY = 6, Active = false, State = State.BlackKnight };
                    Cell cell_8 = new Cell() { CoordinateX = 0, CoordinateY = 7, Active = false, State = State.BlackRook };
                    context.Cells.AddRange(cell_1, cell_2, cell_3, cell_4, cell_5, cell_6, cell_7, cell_8);
                    for (int i = 0; i < 8; i++)
                    {
                        context.Cells.Add(new Cell { CoordinateX = 1, CoordinateY = i, Active = false, State = State.BlackBishop });
                    }
                    for (int i = 2; i < 6; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            context.Cells.Add(new Cell() { CoordinateX = i, CoordinateY = j, Active = false, State = State.Empty });
                        }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        context.Cells.Add(new Cell { CoordinateX = 6, CoordinateY = i, Active = false, State = State.WhiteBishop });
                    }
                    Cell cell_9 = new Cell() { CoordinateX = 7, CoordinateY = 0, Active = false, State = State.WhiteRook };
                    Cell cell_10 = new Cell() { CoordinateX = 7, CoordinateY = 1, Active = false, State = State.WhiteKnight };
                    Cell cell_11 = new Cell() { CoordinateX = 7, CoordinateY = 2, Active = false, State = State.WhiteBishop };
                    Cell cell_12 = new Cell() { CoordinateX = 7, CoordinateY = 3, Active = false, State = State.WhiteQueen };
                    Cell cell_13 = new Cell() { CoordinateX = 7, CoordinateY = 4, Active = false, State = State.WhiteKing };
                    Cell cell_14 = new Cell() { CoordinateX = 7, CoordinateY = 5, Active = false, State = State.WhiteBishop };
                    Cell cell_15 = new Cell() { CoordinateX = 7, CoordinateY = 6, Active = false, State = State.WhiteKnight };
                    Cell cell_16 = new Cell() { CoordinateX = 7, CoordinateY = 7, Active = false, State = State.WhiteRook };
                    context.Cells.AddRange(cell_9, cell_10, cell_11, cell_12, cell_13, cell_14, cell_15, cell_16);
                    SetupBoard();                   
                }
                else
                {                 
                    currentPlayer = player.CurrentPlayer;

                    var cells = context.Cells;
                    Board board = new Board();
                    // loading data from the database to the board
                    foreach (var cell in cells)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                if (cell.CoordinateX == i && cell.CoordinateY == j)
                                {
                                    board[i, j] = cell.State;
                                }
                            }
                        }
                    }
                    // data binding
                    Board = board;
                }
                context.SaveChanges();
            }         
        }

        public Board Board 
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }

        public void SaveMove(Cell cell, Cell activeCell)
        {     
            using (BoardContext context = new BoardContext())
            {
                var cells = context.Cells;
                foreach (var newCell in cells)
                {
                    if (newCell.CoordinateX == cell.CoordinateX && newCell.CoordinateY == cell.CoordinateY)
                    {
                        newCell.State = cell.State;
                    }
                    else if (newCell.CoordinateX == activeCell.CoordinateX && newCell.CoordinateY == activeCell.CoordinateY)
                    {
                        newCell.State = State.Empty;
                    }
                }
                context.SaveChanges();
            }
        }

        public ICommand NewGameCommand => newGameCommand ??= new RelayCommand(parameter => 
        {
            using (BoardContext context = new BoardContext())
            {
                Player player = context.Players.FirstOrDefault();
                player.CurrentPlayer = 1;
                currentPlayer = player.CurrentPlayer;
                var cells = context.Cells;
                foreach (var cell in cells)
                {
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 0)
                        cell.State = State.BlackRook;
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 1)
                        cell.State = State.BlackKnight;
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 2)
                        cell.State = State.BlackBishop;
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 3)
                        cell.State = State.BlackQueen;
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 4)
                        cell.State = State.BlackKing;
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 5)
                        cell.State = State.BlackBishop;
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 6)
                        cell.State = State.BlackKnight;
                    if (cell.CoordinateX == 0 && cell.CoordinateY == 7)
                        cell.State = State.BlackRook;
                    for (int i = 0; i < 8; i++)
                    {
                        if (cell.CoordinateX == 1 && cell.CoordinateY == i)
                        {
                            cell.State = State.BlackPawn;
                        }
                    }
                    for (int i = 2; i < 6; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (cell.CoordinateX == i && cell.CoordinateY == j)
                            {
                                cell.State = State.Empty;
                            }
                        }
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        if (cell.CoordinateX == 6 && cell.CoordinateY == i)
                        {
                            cell.State = State.WhitePawn;
                        }
                    }
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 0)
                        cell.State = State.WhiteRook;
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 1)
                        cell.State = State.WhiteKnight;
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 2)
                        cell.State = State.WhiteBishop;
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 3)
                        cell.State = State.WhiteQueen;
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 4)
                        cell.State = State.WhiteKing;
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 5)
                        cell.State = State.WhiteBishop;
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 6)
                        cell.State = State.WhiteKnight;
                    if (cell.CoordinateX == 7 && cell.CoordinateY == 7)
                        cell.State = State.WhiteRook;
                }
                context.SaveChanges();
            }
            SetupBoard();
            currentPlayer = 1;
        });
        
        public ICommand CellCommand => cellCommand ??= new RelayCommand(parameter =>
        {
            Cell cell = (Cell)parameter;
            Cell activeCell = Board.FirstOrDefault(x => x.Active);
            if (cell.State != State.Empty && Player(cell))
            {
                coordinateXOfFigure = cell.CoordinateX;
                coordinateYOfFigure = cell.CoordinateY;
                if (!cell.Active && activeCell != null)
                    activeCell.Active = false;
                cell.Active = !cell.Active;
            }
            else if (activeCell != null && CanMove(cell.CoordinateX, cell.CoordinateY, activeCell.State))
<<<<<<< HEAD
            {           
=======
            {
                // sorry for so bad interface
>>>>>>> f24cd0525b59dae9d1a03418d1ff8f787e7cf71b
                if (!CheckBlack(activeCell, cell, _board) && currentPlayer == 2)
                {
                    MessageBox.Show("You cant move black");
                    countOfEnemyFugures = 0;
                    
                }
                else if (!CheckWhite(activeCell, cell, _board) && currentPlayer == 1)
                {
                    MessageBox.Show("You cant move white");
                    countOfEnemyFugures = 0;
                }
                else
                {
                    if (activeCell.State == State.WhitePawn && cell.CoordinateX == 0)
                    {
                        activeCell.Active = false;
                        activeCell.State = State.Empty;
                        cell.State = State.WhiteQueen;
                        Move(cell, activeCell);
                    }
                    else if (activeCell.State == State.BlackPawn && cell.CoordinateX == 7)
                    {
                        activeCell.Active = false;
                        activeCell.State = State.Empty;
                        cell.State = State.BlackQueen;
                        Move(cell, activeCell);
                    }
                    else
                    {
                        activeCell.Active = false;
                        cell.State = activeCell.State;
                        activeCell.State = State.Empty;
                        Move(cell, activeCell);
                    }
                    
                }
            }
        }, parameter => parameter is Cell cell && (Board.Any(x => x.Active) || cell.State != State.Empty));

        public void Move(Cell cell, Cell activeCell) 
        {
            SaveMove(cell, activeCell);
            SwitchPlayer();
            countOfEnemyFugures = 0;
            if (isCheckBlack() && !CheckMateBlack())
            {
                MessageBox.Show("CheckMate Black");

                this.newGameCommand.Execute(null);
            }
            else if (isCheckWhite() && !CheckMateWhite())
            {
                MessageBox.Show("CheckMate White");
                this.newGameCommand.Execute(null);
            }
        }
        public bool CheckMateBlack()
        {
            foreach (Cell activeCell in _board)
            {   
                State state = State.Empty;
                if (StateColor(activeCell.State) == "Black")
                {
                    coordinateXOfFigure = activeCell.CoordinateX;
                    coordinateYOfFigure = activeCell.CoordinateY;      
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (StateColor(_board[i, j]) == "Empty" || StateColor(_board[i, j]) == "White")
                            {
                                if (CanMove(i, j, activeCell.State))
                                {
                                    state = _board[i, j];
                                    _board[i, j] = activeCell.State;
                                    activeCell.State = State.Empty;
                                    if (isCheckBlack())
                                    {
                                        activeCell.State = _board[i, j];
                                        _board[i, j] = state;
                                    }
                                    else
                                    {
                                        activeCell.State = _board[i, j];
                                        _board[i, j] = state;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool CheckMateWhite()
        {
            foreach (Cell activeCell in _board)
            {
                State state = State.Empty;
                if (StateColor(activeCell.State) == "White")
                {
                    coordinateXOfFigure = activeCell.CoordinateX;
                    coordinateYOfFigure = activeCell.CoordinateY;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (StateColor(_board[i, j]) == "Empty" || StateColor(_board[i, j]) == "Black")
                            {
                                if (CanMove(i, j, activeCell.State))
                                {
                                    // We change the current piece with all possible moves,
                                    // if we have changed all the pieces and there is no one at which Check would give falsе, then checkmate
                                    state = _board[i, j];
                                    _board[i, j] = activeCell.State;
                                    activeCell.State = State.Empty;
                                    if (isCheckWhite())
                                    {
                                        activeCell.State = _board[i, j];
                                        _board[i, j] = state;
                                    }
                                    else
                                    {
                                        activeCell.State = _board[i, j];
                                        _board[i, j] = state;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool CheckWhite(Cell activeCell, Cell cell, Board _board)
        {
            State state1 = new State();
            state1 = _board[cell.CoordinateX, cell.CoordinateY];
            _board[cell.CoordinateX, cell.CoordinateY] = activeCell.State;
            activeCell.State = State.Empty;
            if (isCheckWhite())
            {
                activeCell.State = _board[cell.CoordinateX, cell.CoordinateY];
                _board[cell.CoordinateX, cell.CoordinateY] = state1;
                return false;
            }
            else
            {
                activeCell.State = _board[cell.CoordinateX, cell.CoordinateY];
                _board[cell.CoordinateX, cell.CoordinateY] = state1;
                return true;
            }
        }
        public bool CheckBlack(Cell activeCell, Cell cell, Board _board)
        {
            State state1 = new State();
            state1 = _board[cell.CoordinateX, cell.CoordinateY];
            _board[cell.CoordinateX, cell.CoordinateY] = activeCell.State;
            activeCell.State = State.Empty;
            if (isCheckBlack())
            {
                activeCell.State = _board[cell.CoordinateX, cell.CoordinateY];
                _board[cell.CoordinateX, cell.CoordinateY] = state1;
                return false;
            }
            else
            {
                activeCell.State = _board[cell.CoordinateX, cell.CoordinateY];
                _board[cell.CoordinateX, cell.CoordinateY] = state1;
                return true;
            }
        }
        private bool isCheckWhite()
        {
            int X = coordinateXOfFigure;
            int Y = coordinateYOfFigure;
            int IWhiteKing = 0;
            int JWhiteKing = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_board[i, j] == State.WhiteKing)
                    {
                        IWhiteKing = i;
                        JWhiteKing = j;
                        break;
                    }
                }
            }

            foreach (var cell in _board)
            {
                coordinateXOfFigure = cell.CoordinateX;
                coordinateYOfFigure = cell.CoordinateY;
               
                if (StateColor(cell.State) == "Black")
                {
                    if (CanMove(IWhiteKing, JWhiteKing, cell.State))
                    {
                        coordinateXOfFigure = X;
                        coordinateYOfFigure = Y;
                        return true;
                    }
                }
            }
            coordinateXOfFigure = X;
            coordinateYOfFigure = Y;
            return false;
        }

        private bool isCheckBlack()
        {
            int X = coordinateXOfFigure;
            int Y = coordinateYOfFigure;
            int IBlackKing = 0;
            int JBlackKing = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_board[i, j] == State.BlackKing)
                    {
                        IBlackKing = i;
                        JBlackKing = j;
                        break;
                    }
                }
            }

            foreach (var cell in _board)
            {
                coordinateXOfFigure = cell.CoordinateX;
                coordinateYOfFigure = cell.CoordinateY;
                if (StateColor(cell.State) == "White")
                {
                    if (CanMove(IBlackKing, JBlackKing, cell.State))
                    {
                        coordinateXOfFigure = X;
                        coordinateYOfFigure = Y;
                        return true;
                    }
                }           
            }
            coordinateXOfFigure = X;
            coordinateYOfFigure = Y;
            return false;
        }

        public bool CanMove(int CoordinateX, int CoordinateY, State state)
        {
            if (state == State.Empty)
            {
                return false;
            }
            switch (state)
            {
                case State.BlackPawn:
                    if (MoveBlackPawn(CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure) && coordinateXOfFigure < CoordinateX)
                    {
                        return true;
                    }
                    return false;
                case State.WhitePawn:
                    if (MoveWhitePawn(CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure) && coordinateXOfFigure > CoordinateX)
                    {
                        return true;
                    }
                    return false;
                case State.BlackBishop:
                    if (MoveDiagonal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.BlackRook:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        return true;
                    }
                    return false;
                case State.BlackQueen:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure) || MoveDiagonal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.BlackKnight:
                    if (MoveKnight(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        return true;
                    }
                    return false;
                case State.WhiteBishop:
                    if (MoveDiagonal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.WhiteRook:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        return true;
                    }
                    return false;
                case State.WhiteQueen:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure) || MoveDiagonal(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.WhiteKnight:
                    if (MoveKnight(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        return true;
                    }
                    return false;
                case State.BlackKing:
                    if (MoveKing(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        return true;
                    }
                    return false;
                case State.WhiteKing:
                    if (MoveKing(state, CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
                    {
                        return true;
                    }
                    return false;
            }
            return false;
        }

        private bool MoveKing(State state, int CoordinateX, int CoordinateY, int coordinateXOfFigure, int coordinateYOfFigure)
        {
            if (BlackOrWhite(_board, coordinateXOfFigure, coordinateYOfFigure) != BlackOrWhite(_board, CoordinateX, CoordinateY))
            {
                if (Math.Abs(coordinateXOfFigure - CoordinateX) < 2 && Math.Abs(coordinateYOfFigure - CoordinateY) < 2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool MoveKnight(State state, int CoordinateX, int CoordinateY, int coordinateXOfFigure, int coordinateYOfFigure)
        {
            if (BlackOrWhite(_board, coordinateXOfFigure, coordinateYOfFigure) != BlackOrWhite(_board, CoordinateX, CoordinateY))
            {
                if (Math.Abs(coordinateXOfFigure - CoordinateX) == 2 && Math.Abs(coordinateYOfFigure - CoordinateY) == 1)
                {
                    return true;
                }
                else if (Math.Abs(coordinateYOfFigure - CoordinateY) == 2 && Math.Abs(coordinateXOfFigure - CoordinateX) == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private bool MoveVerticalHorizontal(State state, int CoordinateX, int CoordinateY, int coordinateXOfFigure, int coordinateYOfFigure)
        {
            if (CoordinateX - coordinateXOfFigure > 0 && CoordinateY - coordinateYOfFigure == 0)
            {
                int j = coordinateYOfFigure;
                for (int i = coordinateXOfFigure + 1; i <= CoordinateX; i++)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (CoordinateX - coordinateXOfFigure < 0 && CoordinateY - coordinateYOfFigure == 0)
            {
                int j = coordinateYOfFigure;
                for (int i = coordinateXOfFigure - 1; i >= CoordinateX; i--)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (CoordinateX - coordinateXOfFigure == 0 && CoordinateY - coordinateYOfFigure > 0)
            {
                int i = coordinateXOfFigure;
                for (int j = coordinateYOfFigure + 1; j <= CoordinateY; j++)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                    }
                }
                return true;
            }
            else if (CoordinateX - coordinateXOfFigure == 0 && CoordinateY - coordinateYOfFigure < 0)
            {
                int i = coordinateXOfFigure;
                for (int j = coordinateYOfFigure - 1; j >= CoordinateY; j--)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }   
        public bool isDiagonal(int CoordinateX, int CoordinateY, int coordinateXOfFigure, int coordinateYOfFigure)
        {
            for (int i = 0; i < 8; i++)
            {
                if (Math.Abs(CoordinateY - coordinateYOfFigure) == i && Math.Abs(CoordinateX - coordinateXOfFigure) == i)
                    return true;
            }
            return false;
        }
        public bool MoveDiagonal(State state, int CoordinateX, int CoordinateY, int coordinateXOfFigure, int coordinateYOfFigure)
        {    
            if (CoordinateY - coordinateYOfFigure > 0 && CoordinateX - coordinateXOfFigure < 0 && isDiagonal(CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
            {
                int j = coordinateYOfFigure + 1;
                for (int i = coordinateXOfFigure - 1; i >= CoordinateX; i--)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                    }
                    if (j <= CoordinateY)
                        j++;
                }       
                return true;
            }
            else if (CoordinateX - coordinateXOfFigure < 0 && CoordinateY - coordinateYOfFigure < 0 && isDiagonal(CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
            {
                int j = coordinateYOfFigure - 1;
                for (int i = coordinateXOfFigure - 1; i >= CoordinateX; i--)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                        
                    }
                    if (j > CoordinateY)
                        j--;
                }
                return true;
            }
            else if (CoordinateX - coordinateXOfFigure > 0 && CoordinateY - coordinateYOfFigure < 0 && isDiagonal(CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
            {
                int j = coordinateYOfFigure - 1;
                for (int i = coordinateXOfFigure + 1; i <= CoordinateX; i++)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                    }
                    if (j >= CoordinateY)
                        j--;
                }
                return true;
            }
            else if (CoordinateX - coordinateXOfFigure > 0 && CoordinateY - coordinateYOfFigure > 0 && isDiagonal(CoordinateX, CoordinateY, coordinateXOfFigure, coordinateYOfFigure))
            {
                int j = coordinateYOfFigure + 1;
                for (int i = coordinateXOfFigure + 1; i <= CoordinateX; i++)
                {
                    if (InsideBoard(i, j))
                    {
                        if (!isClear(state, i, j) || countOfEnemyFugures > 1)
                        {
                            countOfEnemyFugures = 0;
                            return false;
                        }
                    }
                    if (j <= CoordinateY)
                        j++;
                }
                return true;
            }
            return false;
        }

        private bool isClear(State state, int i, int j)
        {
            if (_board[i, j] == State.Empty)
            {
                return true;
            }
            else if ((BlackOrWhite(_board, i, j) == "Black" && StateColor(state) == "White") || (BlackOrWhite(_board, i, j) == "White" && StateColor(state) == "Black"))
            {
                // If the number of enemy pieces on the "path" is more than one, then we cannot move, since we will jump over a piece
                countOfEnemyFugures++;
                return true; 
            }         
            return false;      
        }

        public string StateColor(State state)
        {
            if (state == State.WhiteBishop || state == State.WhiteRook || state == State.WhiteKnight || state == State.WhitePawn || state == State.WhiteQueen || state == State.WhiteKing)
            {
                return "White";
            }
            else if (state == State.Empty)
            {
                return "Empty";
            }
            return "Black";
        }

        public bool MoveBlackPawn(int CoordinateX, int CoordinateY, int coordinateXOfFigure, int coordinateYOfFigure)
        {
            if (CoordinateY == coordinateYOfFigure && coordinateXOfFigure < CoordinateX && _board[CoordinateX, CoordinateY] == State.Empty)
            {
                if (coordinateXOfFigure == 1 && CoordinateX - coordinateXOfFigure <= 2)
                {
                    return true;
                }
                else if (CoordinateX - coordinateXOfFigure <= 1)
                {
                    return true;
                }
                return false;
            }
            else if ((CoordinateX - coordinateXOfFigure == 1) && (BlackOrWhite(_board, coordinateXOfFigure + 1, coordinateYOfFigure + 1) == "White" || BlackOrWhite(_board, coordinateXOfFigure + 1, coordinateYOfFigure - 1) == "White") && (coordinateYOfFigure == CoordinateY + 1 || coordinateYOfFigure == CoordinateY - 1) && (_board[CoordinateX, CoordinateY] != State.Empty))
            {
                return true;
            }
            return false;
        }

        public bool MoveWhitePawn(int CoordinateX, int CoordinateY, int coordinateXOfFigure, int coordinateYOfFigure)
        {
            if (CoordinateY == coordinateYOfFigure && coordinateXOfFigure > CoordinateX && _board[CoordinateX, CoordinateY] == State.Empty)
            {
                if (coordinateXOfFigure == 6 && coordinateXOfFigure - CoordinateX <= 2)
                {
                    return true;
                }
                else if (coordinateXOfFigure - CoordinateX <= 1)
                {
                    return true;
                }
                return false;
            }
            else if ((coordinateXOfFigure - CoordinateX == 1) && (BlackOrWhite(_board, coordinateXOfFigure - 1, coordinateYOfFigure + 1) == "Black" || BlackOrWhite(_board, coordinateXOfFigure - 1, coordinateYOfFigure - 1) == "Black") && (coordinateYOfFigure == CoordinateY + 1 || coordinateYOfFigure == CoordinateY - 1) && (_board[CoordinateX, CoordinateY] != State.Empty))
            {
                return true;
            }
            return false;
        }
        public string BlackOrWhite(Board board, int i, int j)
        {
            if (InsideBoard(i, j))
            {
                if (board[i, j] == State.WhiteKing || board[i, j] == State.WhiteBishop || board[i, j] == State.WhiteKnight || board[i, j] == State.WhitePawn || board[i, j] == State.WhiteQueen || board[i, j] == State.WhiteRook)
                {
                    return "White";
                }
                else if (board[i,j] == State.Empty)
                {
                    return "Empty";
                }
                else
                    return "Black";
            }
            return "Null";
        }
        public bool Player(Cell cell)
        {
            if (currentPlayer == 1 && StateColor(cell.State) == "White")
            {
                return true;
            }
            else if (currentPlayer == 2 && StateColor(cell.State) == "Black")
            {
                return true;
            }
            return false;
        }

        public bool InsideBoard(int i, int j)
        {
            if (i < 0 || i > 7  || j < 0 || j > 7)
            {
                return false;
            }
            return true;
        }
        private void SetupBoard()
        {
            Board board = new Board();
            board[0, 0] = State.BlackRook;
            board[0, 1] = State.BlackKnight;
            board[0, 2] = State.BlackBishop;
            board[0, 3] = State.BlackQueen;
            board[0, 4] = State.BlackKing;
            board[0, 5] = State.BlackBishop;
            board[0, 6] = State.BlackKnight;
            board[0, 7] = State.BlackRook;
            for (int i = 0; i < 8; i++)
            {
                board[1, i] = State.BlackPawn;
                board[6, i] = State.WhitePawn;
            }
            board[7, 0] = State.WhiteRook;
            board[7, 1] = State.WhiteKnight;
            board[7, 2] = State.WhiteBishop;
            board[7, 3] = State.WhiteQueen;
            board[7, 4] = State.WhiteKing;
            board[7, 5] = State.WhiteBishop;
            board[7, 6] = State.WhiteKnight;
            board[7, 7] = State.WhiteRook;
            Board = board;
        }

        public void SwitchPlayer()
        {
            using (BoardContext context = new BoardContext())
            {
                Player player = context.Players.FirstOrDefault();
                if (player.CurrentPlayer == 1)
                {
                    player.CurrentPlayer = 2;
                    currentPlayer = 2;
                }
                else
                {
                    player.CurrentPlayer = 1;
                    currentPlayer = 1;
                }
                context.SaveChanges();
            }
        }    
    }
}