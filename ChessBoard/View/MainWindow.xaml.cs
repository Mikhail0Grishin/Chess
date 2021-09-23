using ChessBoard.Model;

namespace ChessBoard
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }     
    }
}
