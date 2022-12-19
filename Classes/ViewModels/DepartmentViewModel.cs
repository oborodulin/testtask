using log4net;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using TestTask.Classes.DataModels.Business;
using TestTask.Classes.ViewModels.Business;
using TestTask.Classes.ViewModels.Commands;
using TestTask.Classes.ViewModels.Interfaces;
using TestTask.Classes.Views;
using TestTask.DataAccess;
using TestTask.Managers;

namespace TestTask.Classes.ViewModels
{
    public class DepartmentViewModel : BaseBusinessViewModel, IDepartmentViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Department _model;
        private bool _isSaved = false;

        public DepartmentViewModel(Department model)
        {
            Model = model;
        }

        public Department Model
        {
            get { return this._model; }
            set { this._model = value; NotifyPropertyChanged(); }
        }
        public int? DepartmentId
        {
            get { return this.Model.DepartmentId; }
            set { if (this.Model.DepartmentId != value) { this.Model.DepartmentId = value; NotifyPropertyChanged(); } }
        }
        public string Name
        {
            get { return this.Model.Name; }
            set { if (this.Model.Name != value) { this.Model.Name = value; NotifyPropertyChanged(); } }
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
            try
            {
                var dataAccess = App.DBManager;
                IDbDataParameter[] param = new IDbDataParameter[2];
                if (this.DepartmentId == null)
                {
                    param[0] = dataAccess.CreateParameter("@Name", 50, this.Name, DbType.String);
                    this.DepartmentId = dataAccess.Insert(SqlStatements.INSERT_DEPARTMENT, CommandType.Text, param, out int id);
                    log.Debug($"New department '{this.Name}' inserted: DepartmentId = {this.DepartmentId}");
                }
                else
                {
                    param[0] = dataAccess.CreateParameter("@Name", 50, this.Name, DbType.String);
                    param[1] = dataAccess.CreateParameter("@DepartmentId", this.DepartmentId, DbType.Int32);
                    dataAccess.Insert(SqlStatements.UPDATE_DEPARTMENT, CommandType.Text, param);
                    log.Debug($"Department [DepartmentId = {this.DepartmentId}]' updated");
                }
                this.IsSaved = true;
                if (parameter != null)
                {
                    switch (parameter.GetType().ToString())
                    {
                        case "TestTask.Classes.Views.DepartmentView": ((DepartmentView)parameter).Close(); break;
                    }
                }
                log.Debug("Command: Save executed");
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }
    }
}
