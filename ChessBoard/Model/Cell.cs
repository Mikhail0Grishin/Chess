namespace ChessBoard
{
    public class Cell : NotifyPropertyChanged
    {
        private State state;
        private bool active;
        private int coordinateX;
        private int coordinateY;
        private int id;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public State State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }
        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                OnPropertyChanged();
            }
        }

        public int CoordinateX
        {
            get { return coordinateX; }
            set
            {
                coordinateX = value;
                OnPropertyChanged();
            }
        }
        public int CoordinateY
        {
            get { return coordinateY; }
            set
            {
                coordinateY = value;
                OnPropertyChanged();
            }
        }
    }
}