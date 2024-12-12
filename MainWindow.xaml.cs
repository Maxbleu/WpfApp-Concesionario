using System.Windows;
using WpfApp_Concesionario.ViewModels;

namespace WpfApp_Concesionario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            this.DataContext = mainWindowViewModel;
        }
    }
}     