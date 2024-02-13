using BlApi;
using System;
using System.Windows;

namespace PL.Engineer
{
    public partial class EngineerWindow : Window
    {
        private readonly IBl bl = Factory.Get();


        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register(nameof(CurrentEngineer), typeof(BO.Engineer), typeof(EngineerWindow));

        public EngineerWindow(int id = 0)
        {
            InitializeComponent();

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // ניתן להוסיף כאן לוגיקה נוספת לשמירת הנתונים למאגר הנתונים
        }
    }
}
