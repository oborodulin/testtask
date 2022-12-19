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
using TestTask.Classes.Views.ViewModelLocators;
using TestTask.DataAccess;
using TestTask.Extensions;
using TestTask.Managers;

namespace TestTask.Classes.ViewModels
{
    public class EmployeesViewModel : BaseViewModel, IEmployeesViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Employees _employees = new();
        private Employee _selectedEmployee = new();
        private Departments _departments = new();

        /// <summary>
        /// Initializes a new ProductNotifyViewModelGeneric object with default values.
        /// </summary>
        public EmployeesViewModel()
        {
            log.Debug("constructor called");
            this.Departments = ViewModelLocator.DepartmentsViewModel.AllDepartments();
            //DependencyManager.Instance.Resolve<IDepartmentsViewModel>().Departments;
        }

        /// <summary>
        /// Gets or sets the ProductsNotifyGeneric collection of validatable ProductNotifyGeneric objects to be edited in the View.
        /// </summary>
        public Employees Employees
        {
            get { return this._employees; }
            set { if (this._employees != value) { this._employees = value; NotifyPropertyChanged(); } }
        }

        public Employee SelectedEmployee
        {
            get { return this._selectedEmployee; }
            set { if (this._selectedEmployee != value) { this._selectedEmployee = value; NotifyPropertyChanged(); } }
        }

        public Departments Departments
        {
            get { return _departments; }
            set { if (_departments != value) { _departments = value; NotifyPropertyChanged(); } }
        }

        private void Employees_CurrentItemChanged(Employee oldEmployee, Employee newEmployee)
        {
            if (newEmployee != null) newEmployee.PropertyChanged += Employee_PropertyChanged;
            if (oldEmployee != null) oldEmployee.PropertyChanged -= Employee_PropertyChanged;
        }

        private void Employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Employees.CurrentItem.Name)) ValidateUniqueName(Employees.CurrentItem);
        }

        private void ValidateUniqueName(Employee Employee)
        {
            string errorMessage = "The product name must be unique.";
            //if (!IsEmployeeNameUnique(Employee)) Employee.ExternalErrors.Add(errorMessage);
            //else Employee.ExternalErrors.Remove(errorMessage);
        }

        private bool IsEmployeeNameUnique(Employee Employee) => Employees.Count(d => d.EmployeeId != Employee.EmployeeId && d.Name != string.Empty && d.Name == Employee.Name) == 0;

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
            log.Debug($"Command: Load ({parameter.GetType()}) called");
            try
            {
                var dataAccess = App.DBManager;
                string query = SqlStatements.READ_ALL_EMPLOYEES;
                IDbDataParameter[] param = new IDbDataParameter[1];
                switch (parameter.GetType().ToString())
                {
                    case "TestTask.Classes.DataModels.Business.Department":
                        query = SqlStatements.READ_EMPLOYEES_BY_DEPARTMENT;
                        param[0] = dataAccess.CreateParameter("@DepartmentId", ((Department)parameter).DepartmentId.GetValueOrDefault(), DbType.Int32);
                        break;
                }
                //log.Debug($"SQL: '{query}'");
                DataTable dt = dataAccess.GetDataTable(query, CommandType.Text, param);
                this.Employees.Clear();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        this.Employees.Add(new Employee(Convert.ToInt32(row["EmployeeId"]),
                                                    row["Name"].ToString(),
                                                    new Department(Convert.ToInt32(row["DepartmentId"]), row["DepartmentName"].ToString()),
                                                    Convert.ToDateTime(row["BirthDate"]),
                                                    row["Gender"].ToString()));
                    }
                    this.Employees.Synchronize();
                    //this.Employees.CurrentItemChanged += Employees_CurrentItemChanged;
                    this.Employees.CurrentItem = this.Employees.Last();
                    log.Debug($"Command: Load {this.Employees.Count} items");
                }
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
            try
            {
                Employee employee = new Employee();
                if (parameter.GetType().ToString() == "TestTask.Classes.DataModels.Business.Department")
                {
                    employee.Department.CopyValuesFrom((Department)parameter);
                }
                this.Employees.Add(employee);
                this.Employees.CurrentItem = employee;
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
            return this.SelectedEmployee != null && this.SelectedEmployee.EmployeeId != null;
        }

        private void Edit(object parameter)
        {
            log.Debug($"Command: Edit called for '{this.SelectedEmployee.Name}'");
            try
            {
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
            return this.SelectedEmployee != null && this.SelectedEmployee.EmployeeId != null;
        }

        private void Delete(object parameter)
        {
            log.Debug($"Command: Delete called for '{this.SelectedEmployee}'");
            try
            {
                MessageBoxResult result = MessageBox.Show($"Удалить сотрудника '{this.SelectedEmployee.Name}'?", "Подверждение удаления", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    var dataAccess = App.DBManager;
                    IDbDataParameter[] param = new[] { dataAccess.CreateParameter("@EmployeeId", this.SelectedEmployee.EmployeeId, DbType.Int32) };
                    dataAccess.Delete(SqlStatements.DELETE_EMPLOYEE, CommandType.Text, param);
                    log.Debug($"Employee [EmployeeId = {this.SelectedEmployee.EmployeeId}]' deleted");
                    this.Employees.Remove(this.SelectedEmployee);
                }
                log.Debug("Command: Delete executed");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public ICommand DropDownOpenedCommand
        {
            get { return new ActionCommand(action => DropDownOpened(action), canExecute => CanDropDownOpened(canExecute)); }
        }

        private bool CanDropDownOpened(object parameter)
        {
            return this.SelectedEmployee != null;
        }

        private void DropDownOpened(object parameter)
        {
            log.Debug($"Command: DropDownOpened called for {this.SelectedEmployee}");
            try
            {
                this.Departments = ViewModelLocator.DepartmentsViewModel.AllDepartments();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        /// <summary>
        /// The fictional save command to demonstrate the use of commanding with.
        /// </summary>
        public ICommand RowEditEndingCommand
        {
            get { return new ActionCommand(action => RowEditEnding(action), canExecute => CanRowEditEnding(canExecute)); }
        }

        private bool CanRowEditEnding(object parameter)
        {
            return true; // !String.IsNullOrEmpty(this.Name) || this.DepartmentId == null;
        }

        private void RowEditEnding(object parameter)
        {
            log.Debug($"Command: RowEditEnding called"); //this.SelectedEmployee
            try
            {
                foreach (Employee employee in this.Employees.ChangedCollection) {
                    log.Debug($"Command: RowEditEnding for {employee}");
                    EmployeeViewModel viewModel = new(employee);
                    viewModel.SaveCommand.Execute(null);
                    if (viewModel.IsSaved)
                    {
                        log.Debug("Command: RowEditEnding executed");
                    }
                }
                this.Employees.Synchronize();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }
    }
}
