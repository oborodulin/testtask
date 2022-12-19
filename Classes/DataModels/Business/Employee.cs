using log4net;
using System;
using System.ComponentModel;
using System.Text.Json;
using TestTask.Classes.DataModels.Interfaces;

namespace TestTask.Classes.DataModels.Business
{
    public class Employee : BaseSynchronizableDataModel<Employee>, ISynchronizableDataModel<Employee>, IEditableObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        struct EmployeeData
        {
            public EmployeeData() { }
            internal int? employeeId = null;
            internal Department department = new();
            internal string name = string.Empty;
            internal DateTime? birthDate = DateTime.Now;
            internal string gender = "M";
        }

        private EmployeeData _employeeData = new();
        private EmployeeData _backupData;
        private bool inTxn = false;

        public Employee(int employeeId, string name, Department department, DateTime birthDate, string gender)
        {
            this.EmployeeId = employeeId;
            this.Department = department;
            this.Name = name;
            this.BirthDate = birthDate;
            this.Gender = gender;
        }

        /// <summary>
        /// Initializes a new empty Employee object with default values.
        /// </summary>
        public Employee() { }

        public int? EmployeeId
        {
            get { return _employeeData.employeeId; }
            set { if (_employeeData.employeeId != value) { _employeeData.employeeId = value; NotifyPropertyChanged(); } }
        }

        public Department Department
        {
            get { return _employeeData.department; }
            set { if (!_employeeData.department.PropertiesEqual(value)) { _employeeData.department = value; NotifyPropertyChanged(); } }
        }

        public string Name
        {
            get { return _employeeData.name; }
            set { if (_employeeData.name != value) { _employeeData.name = value; NotifyPropertyChanged(); } }
        }

        public DateTime? BirthDate
        {
            get { return _employeeData.birthDate; }
            set { if (_employeeData.birthDate != value) { _employeeData.birthDate = value; NotifyPropertyChanged(); } }
        }

        public string Gender
        {
            get { return _employeeData.gender; }
            set { if (_employeeData.gender != value) { _employeeData.gender = value; NotifyPropertyChanged(); } }
        }

        /// <summary>
        /// Copies all of the values from the input parameter to this object.
        /// </summary>
        /// <param name="Employee">The Employee object to copy the values from.</param>
        public override void CopyValuesFrom(Employee employee)
        {
            this.EmployeeId = employee.EmployeeId;
            this.Department = new Department(employee.Department.DepartmentId, employee.Department.Name);
            this.Name = employee.Name;
            this.BirthDate = employee.BirthDate;
            this.Gender = employee.Gender;
        }

        /// <summary>
        /// Specifies whether the property values of the Employee object are equal to the property values of the object specified by the otherEmployee input parameter, or not.
        /// </summary>
        /// <param name="otherEmployee">The object to compare with the current object.</param>
        /// <returns>True if the property values of the specified object are equal to the property values of the object specified by the otherEmployee input parameter, otherwise false.</returns>
        public override bool PropertiesEqual(Employee otherEmployee)
        {
            if (otherEmployee == null) return false;
            // && !Employees.Except(otherDepartment.Employees).Any() 
            return this.EmployeeId.GetValueOrDefault() == otherEmployee.EmployeeId.GetValueOrDefault() &&
                   this.Name == otherEmployee.Name && this.Department.PropertiesEqual(otherEmployee.Department) &&
                   this.BirthDate.GetValueOrDefault() == otherEmployee.BirthDate.GetValueOrDefault() && 
                   this.Gender == otherEmployee.Gender;
        }

        // Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            log.Debug("Start BeginEdit");
            if (!this.inTxn)
            {
                this._backupData = this._employeeData;
                this.inTxn = true;
                log.Debug($"BeginEdit - {JsonSerializer.Serialize<EmployeeData>(this._backupData)}");
            }
            log.Debug("End BeginEdit");
        }

        void IEditableObject.CancelEdit()
        {
            log.Debug("Start CancelEdit");
            if (inTxn)
            {
                this._employeeData = this._backupData;
                inTxn = false;
                log.Debug($"CancelEdit - {JsonSerializer.Serialize<EmployeeData>(this._employeeData)}");
            }
            log.Debug("End CancelEdit");
        }

        void IEditableObject.EndEdit()
        {
            log.Debug($"Start EndEdit {JsonSerializer.Serialize<EmployeeData>(this._employeeData)}");
            if (this.inTxn)
            {
                this._backupData = new EmployeeData();
                this.inTxn = false;
                log.Debug($"Done EndEdit - {JsonSerializer.Serialize<EmployeeData>(this._employeeData)}");
            }
            log.Debug("End EndEdit");
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize<Employee>(this);
        }
    }
}
