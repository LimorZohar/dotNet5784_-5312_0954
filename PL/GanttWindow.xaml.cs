using BlApi;
using System;
using BO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;


public partial class GanttWindow : Window
{
    private readonly IBl bl = Factory.Get();

    public IEnumerable<TaskInGantt> TasksList
    {
        get { return (IEnumerable<TaskInGantt>)GetValue(ListProperty); }
        set { SetValue(ListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TasksList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ListProperty =
        DependencyProperty.Register("TasksList", typeof(IEnumerable<TaskInGantt>), typeof(GanttWindow));


    public GanttWindow()
    {
        try
        {
            TasksList = bl.Task.GantList();
        }
        catch { throw new Exception(); }
        InitializeComponent();
    }
}
