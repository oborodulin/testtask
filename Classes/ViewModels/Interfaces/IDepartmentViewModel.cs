using System.Windows.Input;
using TestTask.Classes.DataModels.Business;

namespace TestTask.Classes.ViewModels.Interfaces
{
    /// <summary>
    /// Provides data for the UserView class.
    /// </summary>
    public interface IDepartmentViewModel
    {
        /// <summary>
        /// Gets or sets the User object in the related View.
        /// </summary>
        Department Model { get; set; }

        int? DepartmentId { get; set; }

        public string Name { get; set; }

        public bool IsSaved { get; set; }

        /// <summary>
        /// Gets or sets the ICommand object for the Save command in the related View.
        /// </summary>
        ICommand SaveCommand { get; }
    }
}
