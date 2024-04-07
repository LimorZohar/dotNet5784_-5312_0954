using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public ObservableCollection<BO.TaskInList> Tasks
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TasksDep); }
        set { SetValue(TasksDep, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TasksDep =
        DependencyProperty.Register(nameof(Tasks), typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindows2));

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

        Tasks = new(bl.Task.ReadAll());
        SelectList = Enum.GetValues(typeof(BO.Expertise));

        //collectionView = CollectionViewSource.GetDefaultView(Tasks);
        //this.collectionView.GroupDescriptions.Add(new PropertyGroupDescription("Complexity"));
    }

    private void DataChange(BO.Task task, bool addMode)
    {

    }


    private void Select(object sender, SelectionChangedEventArgs e)
    {
        BO.TComplexity selected = (BO.TComplexity)((ComboBox)sender).SelectedValue;
        Tasks = new(bl.Task.ReadAll(x => x.Complexity == selected));
    }

    private void EditTask(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is ListView list)
            new EditTask(DataChange, ((TaskInList)list.SelectedItem).Id).Show();
        else
            new EditTask(DataChange).Show();
    }
}
