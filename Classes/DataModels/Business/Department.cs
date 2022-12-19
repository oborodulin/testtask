using System.Text.Json;
using TestTask.Classes.DataModels.Interfaces;

namespace TestTask.Classes.DataModels.Business
{
    public class Department : BaseSynchronizableDataModel<Department>, ISynchronizableDataModel<Department>, IAuditable
    {
        struct DepartmentData
        {
            public DepartmentData() { }
            internal int? departmentId = null;
            internal string name = string.Empty;
            internal Auditable auditable;
        }

        private DepartmentData _departmentData = new();

        public Department(int? departmentId, string name)
        {
            this.DepartmentId = departmentId;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new empty User object with default values.
        /// </summary>
        public Department() { }

        public int? DepartmentId
        {
            get { return this._departmentData.departmentId; }
            set { if (this._departmentData.departmentId != value) { this._departmentData.departmentId = value; NotifyPropertyChanged(); } }
        }

        public string Name
        {
            get { return this._departmentData.name; }
            set { if (this._departmentData.name != value) { this._departmentData.name = value; NotifyPropertyChanged(); } }
        }

        /// <summary>
        /// Gets or sets the Auditable object of the User object that represents its auditable attributes.
        /// </summary>
        public Auditable Auditable
        {
            get { return this._departmentData.auditable; }
            set { this._departmentData.auditable = value; }
        }

        /// <summary>
        /// Copies all of the values from the input parameter to this object.
        /// </summary>
        /// <param name="user">The User object to copy the values from.</param>
        public override void CopyValuesFrom(Department department)
        {
            this.DepartmentId = department.DepartmentId;
            this.Name = department.Name;
            //Employees.AddRange(department.Employees);
        }

        /// <summary>
        /// Specifies whether the property values of the User object are equal to the property values of the object specified by the otherUser input parameter, or not.
        /// </summary>
        /// <param name="otherUser">The object to compare with the current object.</param>
        /// <returns>True if the property values of the specified object are equal to the property values of the object specified by the otherUser input parameter, otherwise false.</returns>
        public override bool PropertiesEqual(Department otherDepartment)
        {
            if (otherDepartment == null) return false;
            return this.DepartmentId.GetValueOrDefault() == otherDepartment.DepartmentId.GetValueOrDefault() && 
                    this.Name == otherDepartment.Name;// && !Employees.Except(otherDepartment.Employees).Any();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize<Department>(this);
        }

    }
}
