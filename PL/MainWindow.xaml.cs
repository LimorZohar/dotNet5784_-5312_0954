using BlApi;
using PL.Engineer;
using PL.Task;
using System;
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
        private void Manager_Click(object sender, RoutedEventArgs e)
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
        public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

        private DateTime dateTime = DateTime.Now;

        private void ResetClock(object sender, RoutedEventArgs e)
        {
            bl.ResetClock();
            CurrentTimeLabel.Content = bl.Clock;

        }

        private void AddYear(object sender, RoutedEventArgs e)
        {
            bl.AddYear();
            CurrentTimeLabel.Content = bl.Clock;
        }

        private void AddMonth(object sender, RoutedEventArgs e)
        {
            bl.AddMonth();
            CurrentTimeLabel.Content = bl.Clock;
        }

        private void AddDay(object sender, RoutedEventArgs e)
        {
            bl.AddDay();
            CurrentTimeLabel.Content = bl.Clock;

        }



        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
