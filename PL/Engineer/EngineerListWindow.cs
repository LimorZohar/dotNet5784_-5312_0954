using BlApi;
using System.Windows;
using BO;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Collections.Generic;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Input;
using System.Linq;

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
    private void Select(object sender, TextCompositionEventArgs e)
    {
        BO.Expertise selected = (Expertise)sender;
        engineers = bl.Engineer.ReadAll(x => x.Level == selected);
    }

}
