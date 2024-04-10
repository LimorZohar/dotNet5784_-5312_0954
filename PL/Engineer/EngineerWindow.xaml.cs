using BlApi;
using System;
using System.Windows;

namespace PL.Engineer;

public partial class EngineerWindow : Window
{
    private readonly IBl bl = Factory.Get();

    public bool addMode
    {
        get { return (bool)GetValue(addModeProperty); }
        set { SetValue(addModeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for addMode.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty addModeProperty =
        DependencyProperty.Register("addMode", typeof(bool), typeof(EngineerWindow));


    public BO.Engineer Engineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register(nameof(Engineer), typeof(BO.Engineer), typeof(EngineerWindow));

    private int _id;
    public EngineerWindow(int id = 0)
    {
        _id = id;
        // addMode is true if id is 0
        addMode = id == 0;
        InitializeComponent();
        try
        {
            if (!addMode)
                Engineer = bl.Engineer.Read(id)!;
            else
                Engineer = new();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }

    }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (addMode)
                bl.Engineer.Create(Engineer);
            else
                bl.Engineer.Update(Engineer);

            MessageBox.Show("נשמר");
            this.Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }

    private void ChooseTask(object sender, RoutedEventArgs e) => new SelectTaskWindow(_id).Show();
}
