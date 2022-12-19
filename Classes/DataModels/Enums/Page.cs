using System.ComponentModel;

namespace TestTask.Classes.DataModels.Enums
{
    /// <summary>
    /// Represents a page or View in the related Mastering Windows Presentaion Foundation demonstration application.
    /// </summary>
    public enum Page
    {
        /// <summary>
        /// Represents the Bit Rate page in the related Mastering Windows Presentaion Foundation demonstration application.
        /// </summary>
        [Description("Departments")]
        Departments,
        /// <summary>
        /// Represents the Weight Measurement page in the related Mastering Windows Presentaion Foundation demonstration application.
        /// </summary>
        [Description("Department")]
        Department,
        /// <summary>
        /// Represents the User page in the related Mastering Windows Presentaion Foundation demonstration application.
        /// </summary>
        [Description("Employees")]
        Employees,
        /// <summary>
        /// Represents the Panel page in the related Mastering Windows Presentaion Foundation demonstration application.
        /// </summary>
        [Description("Employee")]
        Employee,
    }
}