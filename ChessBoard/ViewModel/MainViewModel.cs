﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ChessBoard
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private Board _board = new Board();
        private ICommand _newGameCommand;
        private ICommand _clearCommand;
        private ICommand _cellCommand;
        private int currentPlayer;
        private int coorX;
        private int coorY;
        private int countOfEnemyFugures;

        public IEnumerable<char> Numbers => "87654321";
        public IEnumerable<char> Letters => "ABCDEFGH";

        public Board Board 
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewGameCommand => _newGameCommand ??= new RelayCommand(parameter => 
        {
            SetupBoard();
            currentPlayer = 1;
        });

        public ICommand ClearCommand => _clearCommand ??= new RelayCommand(parameter =>
        {
            Board = new Board();
        });

        public ICommand CellCommand => _cellCommand ??= new RelayCommand(parameter =>
        {
            Cell cell = (Cell)parameter;
            Cell activeCell = Board.FirstOrDefault(x => x.Active);
            if (cell.State != State.Empty && Player(cell))
            {
                coorX = cell.CoordinateX;
                coorY = cell.CoordinateY;
                if (!cell.Active && activeCell != null)
                    activeCell.Active = false;
                cell.Active = !cell.Active;
            }
            else if (activeCell != null && CanMove(cell.CoordinateX, cell.CoordinateY, activeCell.State))
            {

                if (!RevealedCheckBlack(activeCell, cell, _board) && currentPlayer == 2)
                {
                    MessageBox.Show("You cant move black");
                    countOfEnemyFugures = 0;
                    
                }
                else if (!RevealedCheckWhite(activeCell, cell, _board) && currentPlayer == 1)
                {
                    MessageBox.Show("You cant move white");
                    countOfEnemyFugures = 0;
                }
                else
                {
                    activeCell.Active = false;
                    cell.State = activeCell.State;
                    activeCell.State = State.Empty;
                    SwitchPlayer();
                    countOfEnemyFugures = 0;
                    if (CheckBlack() && !CheckMateBlack())
                    {
                        MessageBox.Show("CheckMate Black");
                    }
                    else if (CheckWhite() && !CheckMateWhite())
                    {
                        MessageBox.Show("CheckMate White");
                    }
                }
            }
        }, parameter => parameter is Cell cell && (Board.Any(x => x.Active) || cell.State != State.Empty));

        public bool CheckMateBlack()
        {
            foreach (Cell activeCell in _board)
            {   
                State state = State.Empty;
                if (StateColor(activeCell.State) == "Black")
                {
                    coorX = activeCell.CoordinateX;
                    coorY = activeCell.CoordinateY;      
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
                                    if (CheckBlack())
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
                    coorX = activeCell.CoordinateX;
                    coorY = activeCell.CoordinateY;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (StateColor(_board[i, j]) == "Empty" || StateColor(_board[i, j]) == "Black")
                            {
                                if (CanMove(i, j, activeCell.State))
                                {
                                    state = _board[i, j];
                                    _board[i, j] = activeCell.State;
                                    activeCell.State = State.Empty;
                                    if (CheckWhite())
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
        public bool RevealedCheckWhite(Cell activeCell, Cell cell, Board _board)
        {
            State state1 = new State();
            state1 = _board[cell.CoordinateX, cell.CoordinateY];
            _board[cell.CoordinateX, cell.CoordinateY] = activeCell.State;
            activeCell.State = State.Empty;
            if (CheckWhite())
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
        public bool RevealedCheckBlack(Cell activeCell, Cell cell, Board _board)
        {
            State state1 = new State();
            state1 = _board[cell.CoordinateX, cell.CoordinateY];
            _board[cell.CoordinateX, cell.CoordinateY] = activeCell.State;
            activeCell.State = State.Empty;
            if (CheckBlack())
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
        private bool CheckWhite()
        {
            int X = coorX;
            int Y = coorY;
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
                coorX = cell.CoordinateX;
                coorY = cell.CoordinateY;
               
                if (StateColor(cell.State) == "Black")
                {
                    if (CanMove(IWhiteKing, JWhiteKing, cell.State))
                    {
                        coorX = X;
                        coorY = Y;
                        return true;
                    }
                }
            }
            coorX = X;
            coorY = Y;
            return false;
        }

        private bool CheckBlack()
        {
            int X = coorX;
            int Y = coorY;
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
                coorX = cell.CoordinateX;
                coorY = cell.CoordinateY;
                if (StateColor(cell.State) == "White")
                {
                    if (CanMove(IBlackKing, JBlackKing, cell.State))
                    {
                        coorX = X;
                        coorY = Y;
                        return true;
                    }
                }           
            }
            coorX = X;
            coorY = Y;
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
                    if (MoveBlackPawn(CoordinateX, CoordinateY, coorX, coorY) && coorX < CoordinateX)
                    {
                        return true;
                    }
                    return false;
                case State.WhitePawn:
                    if (MoveWhitePawn(CoordinateX, CoordinateY, coorX, coorY) && coorX > CoordinateX)
                    {
                        return true;
                    }
                    return false;
                case State.BlackBishop:
                    if (MoveDiagonal(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.BlackRook:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        return true;
                    }
                    return false;
                case State.BlackQueen:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coorX, coorY) || MoveDiagonal(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.BlackKnight:
                    if (MoveKnight(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        return true;
                    }
                    return false;
                case State.WhiteBishop:
                    if (MoveDiagonal(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.WhiteRook:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        return true;
                    }
                    return false;
                case State.WhiteQueen:
                    if (MoveVerticalHorizontal(state, CoordinateX, CoordinateY, coorX, coorY) || MoveDiagonal(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        countOfEnemyFugures = 0;
                        return true;
                    }
                    countOfEnemyFugures = 0;
                    return false;
                case State.WhiteKnight:
                    if (MoveKnight(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        return true;
                    }
                    return false;
                case State.BlackKing:
                    if (MoveKing(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        return true;
                    }
                    return false;
                case State.WhiteKing:
                    if (MoveKing(state, CoordinateX, CoordinateY, coorX, coorY))
                    {
                        return true;
                    }
                    return false;
            }
            return false;
        }

        private bool MoveKing(State state, int CoordinateX, int CoordinateY, int coorX, int coorY)
        {
            if (BlackOrWhite(_board, coorX, coorY) != BlackOrWhite(_board, CoordinateX, CoordinateY))
            {
                if (Math.Abs(coorX - CoordinateX) < 2 && Math.Abs(coorY - CoordinateY) < 2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool MoveKnight(State state, int CoordinateX, int CoordinateY, int coorX, int coorY)
        {
            if (BlackOrWhite(_board, coorX, coorY) != BlackOrWhite(_board, CoordinateX, CoordinateY))
            {
                if (Math.Abs(coorX - CoordinateX) == 2 && Math.Abs(coorY - CoordinateY) == 1)
                {
                    return true;
                }
                else if (Math.Abs(coorY - CoordinateY) == 2 && Math.Abs(coorX - CoordinateX) == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private bool MoveVerticalHorizontal(State state, int CoordinateX, int CoordinateY, int coorX, int coorY)
        {
            if (CoordinateX - coorX > 0 && CoordinateY - coorY == 0)
            {
                int j = coorY;
                for (int i = coorX + 1; i <= CoordinateX; i++)
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
            else if (CoordinateX - coorX < 0 && CoordinateY - coorY == 0)
            {
                int j = coorY;
                for (int i = coorX - 1; i >= CoordinateX; i--)
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
            else if (CoordinateX - coorX == 0 && CoordinateY - coorY > 0)
            {
                int i = coorX;
                for (int j = coorY + 1; j <= CoordinateY; j++)
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
            else if (CoordinateX - coorX == 0 && CoordinateY - coorY < 0)
            {
                int i = coorX;
                for (int j = coorY - 1; j >= CoordinateY; j--)
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
        public bool isDiagonal(int CoordinateX, int CoordinateY, int coorX, int coorY)
        {
            for (int i = 0; i < 8; i++)
            {
                if (Math.Abs(CoordinateY - coorY) == i && Math.Abs(CoordinateX - coorX) == i)
                    return true;
            }
            return false;
        }
        public bool MoveDiagonal(State state, int CoordinateX, int CoordinateY, int coorX, int coorY)
        {    
            if (CoordinateY - coorY > 0 && CoordinateX - coorX < 0 && isDiagonal(CoordinateX, CoordinateY, coorX, coorY))
            {
                int j = coorY + 1;
                for (int i = coorX - 1; i >= CoordinateX; i--)
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
            else if (CoordinateX - coorX < 0 && CoordinateY - coorY < 0 && isDiagonal(CoordinateX, CoordinateY, coorX, coorY))
            {
                int j = coorY - 1;
                for (int i = coorX - 1; i >= CoordinateX; i--)
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
            else if (CoordinateX - coorX > 0 && CoordinateY - coorY < 0 && isDiagonal(CoordinateX, CoordinateY, coorX, coorY))
            {
                int j = coorY - 1;
                for (int i = coorX + 1; i <= CoordinateX; i++)
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
            else if (CoordinateX - coorX > 0 && CoordinateY - coorY > 0 && isDiagonal(CoordinateX, CoordinateY, coorX, coorY))
            {
                int j = coorY + 1;
                for (int i = coorX + 1; i <= CoordinateX; i++)
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

        public bool MoveBlackPawn(int CoordinateX, int CoordinateY, int coorX, int coorY)
        {
            if (CoordinateY == coorY && coorX < CoordinateX && _board[CoordinateX, CoordinateY] == State.Empty)
            {
                if (coorX == 1 && CoordinateX - coorX <= 2)
                {
                    return true;
                }
                else if (CoordinateX - coorX <= 1)
                {
                    return true;
                }
                return false;
            }
            else if ((CoordinateX - coorX == 1) && (BlackOrWhite(_board, coorX + 1, coorY + 1) == "White" || BlackOrWhite(_board, coorX + 1, coorY - 1) == "White") && (coorY == CoordinateY + 1 || coorY == CoordinateY - 1) && (_board[CoordinateX, CoordinateY] != State.Empty))
            {
                return true;
            }
            return false;
        }

        public bool MoveWhitePawn(int CoordinateX, int CoordinateY, int coorX, int coorY)
        {
            if (CoordinateY == coorY && coorX > CoordinateX && _board[CoordinateX, CoordinateY] == State.Empty)
            {
                if (coorX == 6 && coorX - CoordinateX <= 2)
                {
                    return true;
                }
                else if (coorX - CoordinateX <= 1)
                {
                    return true;
                }
                return false;
            }
            else if ((coorX - CoordinateX == 1) && (BlackOrWhite(_board, coorX - 1, coorY + 1) == "Black" || BlackOrWhite(_board, coorX - 1, coorY - 1) == "Black") && (coorY == CoordinateY + 1 || coorY == CoordinateY - 1) && (_board[CoordinateX, CoordinateY] != State.Empty))
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
            if (currentPlayer == 1)
            {
                currentPlayer = 2;
            }
            else
                currentPlayer = 1;
        }

        public MainViewModel()
        {
            currentPlayer = 1;
            SetupBoard();
        }
    }
}