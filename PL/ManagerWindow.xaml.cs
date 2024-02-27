using BlApi;
using DalTest;
using PL.Engineer;
using PL.Task;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        IBl bl = Factory.Get();

        private void OpenEngineerListWindow(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        private void OpenTaskListWindow(object sender, RoutedEventArgs e)
        {
            new TaskListWindows2().Show();
        }
        private void InitData(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("האם תרצה לאתחל את הנתונים?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // הרצת פונקציה ליצירת נתונים ראשוניים
                Initialization.Do();
            }

        }
        public ManagerWindow()
        {
            InitializeComponent();
        }


    }
}
