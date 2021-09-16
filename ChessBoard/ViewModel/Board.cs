using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChessBoard
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] area;

        public State this[int row, int column]
        {
            get { return area[row, column].State; }
            set
            {
                area[row, column].State = value;
            }
        }

        public Board()
        {
            area = new Cell[8, 8];
            for (int i = 0; i < area.GetLength(0); i++)
                for (int j = 0; j < area.GetLength(1); j++)
                    area[i, j] = new Cell() {CoordinateX = i, CoordinateY = j };
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return area.Cast<Cell>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return area.GetEnumerator();
        }
    }
}