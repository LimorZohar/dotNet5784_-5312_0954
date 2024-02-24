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


    public BO.Engineer engineer
    {
        get { return (BO.Engineer)GetValue(EngineerProperty); }
        set { SetValue(EngineerProperty, value); }
    }

    public static readonly DependencyProperty EngineerProperty =
        DependencyProperty.Register(nameof(engineer), typeof(BO.Engineer), typeof(EngineerWindow));

    public EngineerWindow(int id = 0)
    {
        addMode = id == 0;
        InitializeComponent();
        try
        {
            if (!addMode)
                engineer = bl.Engineer.Read(id)!;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }

    }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (addMode)
                bl.Engineer.Create(engineer);
            else
                bl.Engineer.Update(engineer);

            MessageBox.Show("succseful");
            this.Close();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
}
