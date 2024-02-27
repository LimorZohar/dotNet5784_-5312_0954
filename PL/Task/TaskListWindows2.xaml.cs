using BlApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindows2.xaml
/// </summary>
public partial class TaskListWindows2 : Window
{
    private readonly IBl bl = Factory.Get();

    private readonly Array SelectList;

    public IEnumerable<BO.Task> Tasks
    {
        get { return (List<BO.Task>)GetValue(TasksDep); }
        set { SetValue(TasksDep, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TasksDep =
        DependencyProperty.Register(nameof(Tasks), typeof(IEnumerable<BO.Task>), typeof(TaskListWindows2));

    public Array Selection
    {
        get { return (Array)GetValue(SelectionDep); }
        set { SetValue(SelectionDep, value); }
    }

    // Using a DependencyProperty as the backing store for Selection.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectionDep =
        DependencyProperty.Register(nameof(Selection), typeof(Array), typeof(TaskListWindows2));

   // ICollectionView collectionView { set; get; }
    public TaskListWindows2()
    {
        InitializeComponent();
     
        Tasks = bl.Task.ReadAll();
        SelectList = Enum.GetValues(typeof(BO.Expertise));

        //collectionView = CollectionViewSource.GetDefaultView(Tasks);
        //this.collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Complexity"));
    }

    private void Select(object sender, SelectionChangedEventArgs e)
    {
        BO.TComplexity selected = (BO.TComplexity)((ComboBox)sender).SelectedValue;
        Tasks = bl.Task.ReadAll(x => x.Complexity == selected);
    }
}
