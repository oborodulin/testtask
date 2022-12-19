using TestTask.Classes.ViewModels;
using TestTask.Classes.ViewModels.Business;
using TestTask.Classes.ViewModels.Interfaces;
using TestTask.Classes.DataModels.Business;

namespace TestTask.Classes.Views.ViewModelLocators
{
    /// <summary>
    /// Supplies different User View Models at runtime and design time.
    /// </summary>
    public class DepartmentsViewModelLocator : BaseViewModelLocator<IDepartmentsViewModel>
    {
        /// <summary>
        /// Initializes a new UserViewModelLocator control with default values.
        /// </summary>
        public DepartmentsViewModelLocator()
        {
            DesignTimeViewModel = new DepartmentsViewModel();
        }
    }
}