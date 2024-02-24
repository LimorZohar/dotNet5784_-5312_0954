using BlApi;
using PL.Engineer;
using PL.Task;
using System.Windows;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBl bl = Factory.Get();  
        public MainWindow()
        {
            InitializeComponent();
        }
        //Button_Click method
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();

        }
        private void OpenEngineerWindow(object sender, RoutedEventArgs e) => new EngineerListWindow().Show();
        private void OpenAdminWindow(object sender, RoutedEventArgs e) => new EngineerWindow().Show();

        
        private void InitData(object sender, RoutedEventArgs e)
        {
            DalTest.Initialization.Do();
        }

        private void OpenTaskWindow(object sender, RoutedEventArgs e)=> new TaskListWindows2().Show();
    }
}