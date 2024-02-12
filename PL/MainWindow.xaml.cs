using PL.Engineer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YourNamespace;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
        private void OpenEngineeWindow(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        private void OpenAdminWindow(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().Show();

        }


        private void InitData(object sender, RoutedEventArgs e)
        {

        }
    }
}