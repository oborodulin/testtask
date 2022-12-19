CREATE TABLE [dbo].[Employees]
(
	[EmployeeID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [DepartmentID] INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[BirthDate] datetime NOT NULL,
	[Gender] nchar(1) NOT NULL
)
ALTER TABLE [dbo].[Employees] ADD CONSTRAINT [FK_Employees_Departments_DepartmentID] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Departments] ([DepartmentID]) ON DELETE CASCADE ON UPDATE CASCADE;
