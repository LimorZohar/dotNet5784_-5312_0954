using BlApi;
using PL.Engineer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PL;

/// <summary>
/// Interaction logic for TheMainWindow.xaml
/// </summary>
/// 
public partial class MainWindow : Window
{
    IBl bl = Factory.Get();


    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow));

    public MainWindow()
    {
        CurrentTime = bl.Clock;
        InitializeComponent();
    }

    private void ChangeTime(object sender, RoutedEventArgs e)
    {
        try
        {
            string addType = "";
            if (sender is Button btn)
                addType = btn.Content.ToString()!;

            switch (addType)
            {
                case "Reset Clock":
                    CurrentTime = DateTime.Now;
                    break;
                case "Add Day":
                    CurrentTime = CurrentTime.AddDays(1);
                    break;
                case "Add Month":
                    CurrentTime = CurrentTime.AddMonths(1);
                    break;
                case "Add Year":
                    CurrentTime = CurrentTime.AddYears(1);
                    break;
                default:
                    return;

            }
        }
        catch (Exception ex) { }
    }

    private void Engineer_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            int userInput = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Please enter your Id:", "Enter Id", ""));
            bl.Engineer.Read(userInput);
            new EngineerWindow(userInput).Show();
        } 
        catch (Exception ex) { MessageBox.Show("תעודת זהות שגויה, נסה שוב");}
        // בדיקה אם המשתמש הזין ערך ולא עזב אותו ריק
        //if (bl.Engineer.Read(userInput) is not null)
        //{
        //    // פתיחת חלון המנהל
        //    new EngineerWindow(userInput).Show();
        //}
    }
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
}
