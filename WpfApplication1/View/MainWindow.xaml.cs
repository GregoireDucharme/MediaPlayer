using System.Windows;

namespace mediaPlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelWMP();
        }
    }
}
