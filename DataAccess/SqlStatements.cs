using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.DataAccess
{
    static class SqlStatements
    {
        public static readonly string READ_ALL_DEPARTMENTS = "SELECT DepartmentId, [Name] FROM Departments ORDER BY [Name]";
        public static readonly string INSERT_DEPARTMENT = "INSERT INTO Departments([Name]) OUTPUT Inserted.DepartmentId VALUES(@Name)";
        public static readonly string UPDATE_DEPARTMENT = "UPDATE Departments SET [Name] = @Name WHERE DepartmentId = @DepartmentId";
        public static readonly string DELETE_DEPARTMENT = "DELETE FROM Departments WHERE DepartmentId = @DepartmentId";

        public static readonly string READ_ALL_EMPLOYEES = @"SELECT e.EmployeeId, e.DepartmentId, d.[Name] DepartmentName, e.[Name], e.BirthDate, e.Gender
                                                            FROM Employees e INNER JOIN Departments d ON d.DepartmentId = e.DepartmentId
                                                            ORDER BY e.[Name]";
        public static readonly string READ_EMPLOYEES_BY_DEPARTMENT = @"SELECT e.EmployeeId, e.DepartmentId, d.[Name] DepartmentName, e.[Name], e.BirthDate, e.Gender
                                                                        FROM Employees e INNER JOIN Departments d ON d.DepartmentId = e.DepartmentId
                                                                        WHERE e.DepartmentId = @DepartmentId
                                                                        ORDER BY e.[Name]";
        public static readonly string INSERT_EMPLOYEE = @"INSERT INTO Employees(DepartmentId, [Name], BirthDate, Gender) 
                                                        OUTPUT Inserted.EmployeeId 
                                                        VALUES(@DepartmentId, @Name, @BirthDate, @Gender)";
        public static readonly string UPDATE_EMPLOYEE = @"UPDATE Employees SET DepartmentId = @DepartmentId, [Name] = @Name, BirthDate = @BirthDate, Gender = @Gender
                                                          WHERE EmployeeId = @EmployeeId";
        public static readonly string DELETE_EMPLOYEE = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
    }
}
