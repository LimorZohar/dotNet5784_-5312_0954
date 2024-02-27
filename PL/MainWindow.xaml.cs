using BlApi;
using PL.Engineer;
using PL.Task;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for TheMainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        IBl bl = Factory.Get();
        private void Maneger_Click(object sender, RoutedEventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Please enter your Id:", "Enter Id", "214125312");

            // בדיקה אם המשתמש הזין ערך ולא עזב אותו ריק
            if (!string.IsNullOrEmpty(userInput))
            {
                // פתיחת חלון המנהל
                new ManagerWindow().Show();
            }

        }
        private void Engineer_Click(object sender, RoutedEventArgs e)
        {
            //// בקשה מהמשתמש להזין את המזהה שלו
            //string userInput = Microsoft.VisualBasic.Interaction.InputBox("Please enter your Id:", "Enter Id", "214125312");

            //// בדיקה אם המשתמש הזין ערך ולא עזב אותו ריק
            //if (!string.IsNullOrEmpty(userInput))
            //{
                // פתיחת חלון המהנדס עם המזהה שהמשתמש הזין
                new EngineerWindow().Show();
            

        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
