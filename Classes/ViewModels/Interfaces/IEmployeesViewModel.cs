using System.Windows.Input;
using TestTask.Classes.DataModels.Business;
using TestTask.Classes.DataModels.Collections;

namespace TestTask.Classes.ViewModels.Interfaces
{
    public interface IEmployeesViewModel
    {
        Employees Employees { get; set; }

        public Employee SelectedEmployee { get; set; }

        ICommand LoadCommand { get; }

        ICommand AddCommand { get; }

        ICommand EditCommand { get; }

        ICommand DeleteCommand { get; }

        ICommand RowEditEndingCommand { get; }
    }
}
