using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChessBoard
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] _area;

        public State this[int row, int column]
        {
            get { return _area[row, column].State; }
            set
            {
                _area[row, column].State = value;
            }
        }

        public Board()
        {
            _area = new Cell[8, 8];
            for (int i = 0; i < _area.GetLength(0); i++)
                for (int j = 0; j < _area.GetLength(1); j++)
                    _area[i, j] = new Cell() {CoordinateX = i, CoordinateY = j };
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return _area.Cast<Cell>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _area.GetEnumerator();
        }
    }
}