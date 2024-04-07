using BlApi;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    private readonly IBl bl = Factory.Get();

    private readonly Array SelectList;

    public IEnumerable<BO.Engineer> engineers
    {
        get { return (List<BO.Engineer>)GetValue(engineersDep); }
        set { SetValue(engineersDep, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty engineersDep =
        DependencyProperty.Register(nameof(engineers), typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow));

    public Array Selection
    {
        get { return (Array)GetValue(SelectionDep); }
        set { SetValue(SelectionDep, value); }
    }

    // Using a DependencyProperty as the backing store for Selection.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectionDep =
        DependencyProperty.Register(nameof(Selection), typeof(Array), typeof(EngineerListWindow));

    /// <summary> EngineerListWindow constructor </summary>
    public EngineerListWindow()
    {
        //initialize the window
        InitializeComponent();
        //get all engineers
        engineers = bl.Engineer.ReadAll();
        //get all expertise
        SelectList = Enum.GetValues(typeof(BO.Expertise));
    }

    private void Select(object sender, SelectionChangedEventArgs e)
    {
        BO.Expertise selected = (BO.Expertise)((ComboBox)sender).SelectedValue;
        engineers = bl.Engineer.ReadAll(x => x.Level == selected);

    }
    private void AddAndUpdate(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is ListView listView)
            {
                var selected = (BO.Engineer)listView.SelectedItem;
                new EngineerWindow(selected.Id).ShowDialog();
                engineers = bl.Engineer.ReadAll();
            }
            else
                new EngineerWindow().ShowDialog();
            engineers =bl.Engineer.ReadAll();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }
}
