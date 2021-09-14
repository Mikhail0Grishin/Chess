namespace ChessBoard
{
    public class Cell : NotifyPropertyChanged
    {
        private State _state;
        private bool _active;
        private int _coordinateX;
        private int _coordinateY;
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public State State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }

        public int CoordinateX
        {
            get { return _coordinateX; }
            set
            {
                _coordinateX = value;
                OnPropertyChanged();
            }
        }
        public int CoordinateY
        {
            get { return _coordinateY; }
            set
            {
                _coordinateY = value;
                OnPropertyChanged();
            }
        }
    }
}