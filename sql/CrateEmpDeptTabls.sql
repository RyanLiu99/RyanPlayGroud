/*
drop table Dept
drop TABLE [dbo].[Employee]
*/


CREATE TABLE [dbo].[Dept](
	[DeptId] [int] IDENTITY(1,1) NOT NULL,
	[DeptName] [nvarchar](50) NULL,
	 CONSTRAINT [PK_Dept] PRIMARY KEY CLUSTERED 
	(
		[DeptId] ASC
	)
)
GO

CREATE TABLE [dbo].[Employee](
	[EmpId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[NickName] [nvarchar](50) NULL,
	[DeptId] [int] NULL 
	 CONSTRAINT FK_Employee_DeptId FOREIGN KEY REFERENCES Dept(DeptId),
	 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
	(
		[EmpId] ASC
	)
)
GO

insert into [dbo].[Dept] (DeptName) values 
('Dept1'), ('Dept2')


insert into Employee (Name, DeptId) values
('Emp1', 1), ('Emp2', null)

GO

select * from Dept
select * from Employee

select * from Employee e inner join Dept d 
	on e.DeptId = d.DeptId and ( EmpId =1 and 1=2)
where NickName is null
