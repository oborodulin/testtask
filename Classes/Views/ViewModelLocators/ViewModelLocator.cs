using System.ComponentModel;
using System.Windows;
using TestTask.Classes.ViewModels.Interfaces;
using TestTask.Managers;

namespace TestTask.Classes.Views.ViewModelLocators
{
    /// <summary>
    /// Supplies View Models that implement particular View Model interfaces.
    /// </summary>
    public class ViewModelLocator
    {
        private DependencyObject dummy = new DependencyObject();

        private static IDepartmentsViewModel? _departmentsViewModel = null;
        private static IDepartmentViewModel? _departmentViewModel = null;
        private static IEmployeesViewModel? _employeesViewModel = null;
        private static IEmployeeViewModel? _employeeViewModel = null;

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(dummy);
        }

        /// <summary>
        /// Gets a View Model that implements the IUserViewModel interface.
        /// </summary>
        public static IDepartmentsViewModel DepartmentsViewModel
        {
            get
            {
                if (_departmentsViewModel != null) return _departmentsViewModel;
                _departmentsViewModel = DependencyManager.Instance.Resolve<IDepartmentsViewModel>();
                return _departmentsViewModel;
            }
        }

        /// <summary>
        /// Gets a View Model that implements the IUserViewModel interface.
        /// </summary>
        public static IDepartmentViewModel DepartmentViewModel
        {
            get
            {
                if (_departmentViewModel != null) return _departmentViewModel;
                _departmentViewModel = DependencyManager.Instance.Resolve<IDepartmentViewModel>();
                return _departmentViewModel;
            }
        }

        /// <summary>
        /// Gets a View Model that implements the IUserViewModel interface.
        /// </summary>
        public static IEmployeesViewModel EmployeesViewModel
        {
            get
            {
                if (_employeesViewModel != null) return _employeesViewModel;
                _employeesViewModel = DependencyManager.Instance.Resolve<IEmployeesViewModel>();
                return _employeesViewModel;
            }
        }

        /// <summary>
        /// Gets a View Model that implements the IUserViewModel interface.
        /// </summary>
        public static IEmployeeViewModel EmployeeViewModel
        {
            get
            {
                if (_employeeViewModel != null) return _employeeViewModel;
                _employeeViewModel = DependencyManager.Instance.Resolve<IEmployeeViewModel>();
                return _employeeViewModel;
            }
        }
    }
}