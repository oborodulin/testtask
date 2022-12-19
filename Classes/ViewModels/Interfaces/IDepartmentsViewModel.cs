using System.Windows.Input;
using TestTask.Classes.DataModels.Business;
using TestTask.Classes.DataModels.Collections;

namespace TestTask.Classes.ViewModels.Interfaces
{
    /// <summary>
    /// Provides data for the UserView class.
    /// </summary>
    public interface IDepartmentsViewModel
    {
        /// <summary>
        /// Gets or sets the User object in the related View.
        /// </summary>
        Departments Departments { get; set; }

        /// <summary>
        /// Gets or sets the User object in the related View.
        /// </summary>
        Department SelectedDepartment { get; set; }

        Departments AllDepartments();

        /// <summary>
        /// Gets or sets the ICommand object for the Save command in the related View.
        /// </summary>
        ICommand LoadCommand { get; }

        /// <summary>
        /// Gets or sets the ICommand object for the Save command in the related View.
        /// </summary>
        ICommand AddCommand { get; }

        /// <summary>
        /// Gets or sets the ICommand object for the Save command in the related View.
        /// </summary>
        ICommand EditCommand { get; }

        /// <summary>
        /// Gets or sets the ICommand object for the Save command in the related View.
        /// </summary>
        ICommand DeleteCommand { get; }

        ICommand ExpandCommand { get; }

        ICommand CollapseCommand { get; }

    }
}
