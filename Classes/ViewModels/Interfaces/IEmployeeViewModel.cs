using System;
using System.Windows.Input;
using TestTask.Classes.DataModels.Business;

namespace TestTask.Classes.ViewModels.Interfaces
{
    /// <summary>
    /// Provides data for the UserView class.
    /// </summary>
    public interface IEmployeeViewModel
    {
        /// <summary>
        /// Gets or sets the User object in the related View.
        /// </summary>
        Employee Model { get; set; }

        int? EmployeeId { get; set; }

        Department Department { get; set; }

        string Name { get; set; }

        DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public bool IsSaved { get; set; }

        /// <summary>
        /// Gets or sets the ICommand object for the Save command in the related View.
        /// </summary>
        ICommand SaveCommand { get; }
    }
}
