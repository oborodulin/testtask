using TestTask.Classes.DataModels.Business;
using TestTask.Classes.ViewModels.Business;
using TestTask.Classes.ViewModels;
using TestTask.Classes.ViewModels.Interfaces;

namespace TestTask.Classes.Views.ViewModelLocators
{
    /// <summary>
    /// Supplies different User View Models at runtime and design time.
    /// </summary>
    public class DepartmentViewModelLocator : BaseViewModelLocator<IDepartmentViewModel>
    {
        /// <summary>
        /// Initializes a new UserViewModelLocator control with default values.
        /// </summary>
        public DepartmentViewModelLocator()
        {
            DesignTimeViewModel = new DepartmentViewModel(new Department());
        }
    }
}