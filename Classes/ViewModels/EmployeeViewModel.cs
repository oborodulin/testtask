using log4net;
using System;
using System.Data;
using System.Windows.Input;
using TestTask.Classes.DataModels.Business;
using TestTask.Classes.ViewModels.Business;
using TestTask.Classes.ViewModels.Commands;
using TestTask.Classes.ViewModels.Interfaces;
using TestTask.DataAccess;

namespace TestTask.Classes.ViewModels
{
    internal class EmployeeViewModel : BaseBusinessViewModel, IEmployeeViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Employee _model;
        private bool _isSaved = false;

        public EmployeeViewModel(Employee model)
        {
            Model = model;
        }

        public Employee Model
        {
            get { return this._model; }
            set { this._model = value; NotifyPropertyChanged(); }
        }

        public int? EmployeeId
        {
            get { return this.Model.EmployeeId; }
            set { if (this.Model.EmployeeId.GetValueOrDefault() != value.GetValueOrDefault()) { this.Model.EmployeeId = value; NotifyPropertyChanged(); } }
        }

        public Department Department
        {
            get { return this.Model.Department; }
            set { if (!this.Model.Department.PropertiesEqual(value)) { Model.Department = value; NotifyPropertyChanged(); } }
        }

        public string Name
        {
            get { return this.Model.Name; }
            set { if (this.Model.Name != value) { this.Model.Name = value; NotifyPropertyChanged(); } }
        }

        public DateTime? BirthDate
        {
            get { return this.Model.BirthDate; }
            set { if (this.Model.BirthDate != value) { this.Model.BirthDate = value; NotifyPropertyChanged(); } }
        }

        public string Gender
        {
            get { return this.Model.Gender; }
            set { if (this.Model.Gender != value) { this.Model.Gender = value; NotifyPropertyChanged(); } }
        }

        public bool IsSaved
        {
            get { return this._isSaved; }
            set { this._isSaved = value; }
        }

        /// <summary>
        /// The fictional save command to demonstrate the use of commanding with.
        /// </summary>
        public ICommand SaveCommand
        {
            get { return new ActionCommand(action => Save(action), canExecute => CanSave(canExecute)); }
        }

        private bool CanSave(object parameter)
        {
            return true; // !String.IsNullOrEmpty(this.Name) || this.DepartmentId == null;
        }

        private void Save(object parameter)
        {
            log.Debug("Command: Save called");
            var dataAccess = App.DBManager;
            if (this.EmployeeId == null)
            {
                IDbDataParameter[] param = new[] {
                    dataAccess.CreateParameter("@DepartmentId", this.Department.DepartmentId.GetValueOrDefault(), DbType.Int32),
                    dataAccess.CreateParameter("@Name", 50, this.Name, DbType.String),
                    dataAccess.CreateParameter("@BirthDate", 50, this.BirthDate.GetValueOrDefault(), DbType.DateTime),
                    dataAccess.CreateParameter("@Gender", 1, this.Gender, DbType.String),
                };
                this.EmployeeId = dataAccess.Insert(SqlStatements.INSERT_EMPLOYEE, CommandType.Text, param, out int id);
                log.Debug($"New department '{this.Name}' inserted: DepartmentId = {this.EmployeeId}");
            }
            else
            {
                IDbDataParameter[] param = new[] {
                    dataAccess.CreateParameter("@EmployeeId", this.EmployeeId.GetValueOrDefault(), DbType.Int32),
                    dataAccess.CreateParameter("@DepartmentId", this.Department.DepartmentId.GetValueOrDefault(), DbType.Int32),
                    dataAccess.CreateParameter("@Name", 50, this.Name, DbType.String),
                    dataAccess.CreateParameter("@BirthDate", 50, this.BirthDate.GetValueOrDefault(), DbType.DateTime),
                    dataAccess.CreateParameter("@Gender", 1, this.Gender, DbType.String),
                };
                dataAccess.Insert(SqlStatements.UPDATE_EMPLOYEE, CommandType.Text, param);
                log.Debug($"Employee [EmployeeId = {this.EmployeeId}]' updated");
            }
            this.IsSaved = true;
            log.Debug($"Command: Save executed");
            if (parameter != null)
            {
                /*switch (parameter.GetType().ToString())
                {
                    case "Window": ((Window)parameter).Close(); break;
                }*/
            }
        }
    }
}
