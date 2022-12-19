using log4net;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TestTask.Classes.DataModels.Business;
using TestTask.Classes.DataModels.Collections;
using TestTask.Classes.ViewModels.Commands;
using TestTask.Classes.ViewModels.Interfaces;
using TestTask.Classes.Views;
using TestTask.Classes.Views.ViewModelLocators;
using TestTask.DataAccess;
using TestTask.Extensions;

namespace TestTask.Classes.ViewModels
{
    public class DepartmentsViewModel : BaseViewModel, IDepartmentsViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Departments _departments = new();
        private Department _selectedDepartment = new();

        /// <summary>
        /// Initializes a new ProductNotifyViewModelGeneric object with default values.
        /// </summary>
        public DepartmentsViewModel()
        {
            log.Debug("constructor called");
            this.LoadCommand.Execute(null);
        }

        /// <summary>
        /// Gets or sets the ProductsNotifyGeneric collection of validatable ProductNotifyGeneric objects to be edited in the View.
        /// </summary>
        public Departments Departments
        {
            get { return _departments; }
            set { if (_departments != value) { _departments = value; NotifyPropertyChanged(); } }
        }

        public Department SelectedDepartment
        {
            get { return _selectedDepartment; }
            set { if (_selectedDepartment != value) { _selectedDepartment = value; NotifyPropertyChanged(); } }
        }

        private void Departments_CurrentItemChanged(Department oldDepartment, Department newDepartment)
        {
            if (newDepartment != null) newDepartment.PropertyChanged += Department_PropertyChanged;
            if (oldDepartment != null) oldDepartment.PropertyChanged -= Department_PropertyChanged;
        }

        private void Department_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Departments.CurrentItem.Name)) ValidateUniqueName(Departments.CurrentItem);
        }

        private void ValidateUniqueName(Department department)
        {
            string errorMessage = "The product name must be unique.";
            //if (!IsDepartmentNameUnique(department)) department.ExternalErrors.Add(errorMessage);
            //else department.ExternalErrors.Remove(errorMessage);
        }

        private bool IsDepartmentNameUnique(Department department) => Departments.Count(d => d.DepartmentId != department.DepartmentId && d.Name != string.Empty && d.Name == department.Name) == 0;

        public Departments AllDepartments() 
        {
            Departments departments = new();
            var dataAccess = App.DBManager;

            DataTable dt = dataAccess.GetDataTable(SqlStatements.READ_ALL_DEPARTMENTS, CommandType.Text);
            foreach (DataRow row in dt.Rows)
            {
                departments.Add(new Department(Convert.ToInt32(row["DepartmentId"]), row["Name"].ToString()));
            }
            return departments;
        }

        public ICommand LoadCommand
        {
            get { return new ActionCommand(action => Load(action), canExecute => CanLoad(canExecute)); }
        }

        private bool CanLoad(object parameter)
        {
            return true;
        }

        private void Load(object parameter)
        {
            log.Debug("Command: Load called");
            try
            {
                this.Departments = this.AllDepartments();
                this.Departments.Synchronize();
                this.Departments.CurrentItemChanged += Departments_CurrentItemChanged;
                this.Departments.CurrentItem = this.Departments.Last();
                log.Debug($"Command: Loaded {this.Departments.Count} items");
                //Departments.CurrentItem.Validate(nameof(Departments.CurrentItem.Name), nameof(Departments.CurrentItem.Price));
                //ValidateUniqueName(Departments.CurrentItem);
                log.Debug("Command: Load executed");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public ICommand AddCommand
        {
            get { return new ActionCommand(action => Add(action), canExecute => CanAdd(canExecute)); }
        }

        private bool CanAdd(object parameter)
        {
            return true;
        }

        private void Add(object parameter)
        {
            log.Debug("Command: Add called");

            //MessageBoxButtonSelection result = DialogService.OpenDialog();
            try
            {
                Window w = new DepartmentView();
                log.Debug("Department dialog created");
                DepartmentViewModel viewModel = new(new Department());
                w.DataContext = viewModel;
                log.Debug("DepartmentViewModel with new Department created and assigned to DataContext");
                w.ShowDialog();
                if (viewModel.IsSaved)
                {
                    this.Departments.Add(viewModel.Model);
                    this.Departments.CurrentItem = viewModel.Model;
                }
                log.Debug("Command: Add executed");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public ICommand EditCommand
        {
            get { return new ActionCommand(action => Edit(action), canExecute => CanEdit(canExecute)); }
        }

        private bool CanEdit(object parameter)
        {
            return this.SelectedDepartment != null && this.SelectedDepartment.DepartmentId != null;
        }

        private void Edit(object parameter)
        {
            log.Debug($"Command: Edit called for '{this.SelectedDepartment}'");
            try
            {
                Window w = new DepartmentView();
                log.Debug("Department dialog created");
                DepartmentViewModel viewModel = new(this.SelectedDepartment);
                w.DataContext = viewModel;
                log.Debug("DepartmentViewModel with SelectedDepartment created and assigned to DataContext");
                w.ShowDialog();
                if (viewModel.IsSaved)
                {
                    Department found = this.Departments.FirstOrDefault(x => x.DepartmentId == viewModel.DepartmentId);
                    found.Name = viewModel.Name;
                }
                log.Debug("Command: Edit executed");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public ICommand DeleteCommand
        {
            get { return new ActionCommand(action => Delete(action), canExecute => CanDelete(canExecute)); }
        }

        private bool CanDelete(object parameter)
        {
            return this.SelectedDepartment != null && this.SelectedDepartment.DepartmentId != null;
        }

        private void Delete(object parameter)
        {
            log.Debug($"Command: Delete called for '{this.SelectedDepartment}'");
            try
            {
                MessageBoxResult result = MessageBox.Show($"Удалить подразделение '{this.SelectedDepartment.Name}'?", "Подверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    var dataAccess = App.DBManager;
                    IDbDataParameter[] param = new[] { dataAccess.CreateParameter("@DepartmentId", this.SelectedDepartment.DepartmentId, DbType.Int32) };
                    dataAccess.Delete(SqlStatements.DELETE_DEPARTMENT, CommandType.Text, param);
                    log.Debug($"Department [DepartmentId = {this.SelectedDepartment.DepartmentId}]' deleted");
                    Departments.Remove(this.SelectedDepartment);
                }
                log.Debug("Command: Delete executed");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public ICommand ExpandCommand
        {
            get { return new ActionCommand(action => Expand(action), canExecute => CanExpand(canExecute)); }
        }

        private bool CanExpand(object parameter)
        {
            return this.SelectedDepartment != null && this.SelectedDepartment.DepartmentId != null;
        }

        private void Expand(object parameter)
        {
            log.Debug($"Command: Expand called for '{this.SelectedDepartment}'");
            try
            {
                ViewModelLocator.EmployeesViewModel.LoadCommand.Execute(this.SelectedDepartment);
                log.Debug("Command: Expand executed");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public ICommand CollapseCommand
        {
            get { return new ActionCommand(action => Collapse(action), canExecute => CanCollapse(canExecute)); }
        }

        private bool CanCollapse(object parameter)
        {
            return true;
        }

        private void Collapse(object parameter)
        {
            log.Debug($"Command: Collapse called for '{this.SelectedDepartment}'");
        }
    }
}
