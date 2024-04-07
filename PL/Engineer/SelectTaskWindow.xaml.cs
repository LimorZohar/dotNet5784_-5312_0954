using BlApi;
using BO;
using System;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for SelectTaskWindow.xaml
    /// </summary>
    public partial class SelectTaskWindow : Window
    {
        private readonly IBl bl = Factory.Get();

        public IEnumerable<TaskInList> TasksList { get; set; }

        BO.Engineer engineer {  get; set; }
        public SelectTaskWindow(int id)
        {
            engineer = bl.Engineer.Read(id)!;
            TasksList = bl.Task.ReadAll(t => (Expertise)t.Complexity <= engineer.Level && t.Engineer is null);
            InitializeComponent();
        }

        private void SelectTask(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ListView list = sender as ListView;
                int taskId = ((TaskInList)list.SelectedItem).Id;
                engineer.Task = new() { Id = taskId };
                bl.Engineer.Update(engineer);
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
