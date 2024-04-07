using System;
using System.Windows;
using BlApi;
using BO;
namespace PL.Task
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        private readonly IBl bl = BlApi.Factory.Get();

        public bool AddMode
        {
            get { return (bool)GetValue(AddModeProperty); }
            set { SetValue(AddModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddModeProperty =
            DependencyProperty.Register(nameof(AddMode), typeof(bool), typeof(EditTask));

        private Action<BO.Task, bool> _dataChange { set; get; }


        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register(nameof(Task), typeof(BO.Task), typeof(EditTask));


        public EditTask(Action<BO.Task,bool> dataChange, int id = 0)
        {
            _dataChange = dataChange;
            AddMode = id == 0;
            if (!AddMode)
                Task = bl.Task.Read(id) ?? new();

            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AddMode)
                    bl.Task.Create(Task);
                else
                    bl.Task.Update(Task);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
