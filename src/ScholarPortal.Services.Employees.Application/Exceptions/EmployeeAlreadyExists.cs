using System;

namespace ScholarPortal.Services.Employees.Application.Exceptions
{
	public class EmployeeAlreadyExists : AppException
	{
		public override string Code => "employee_already_exists";
		
		public Guid EmployeeId { get; }
		
		public EmployeeAlreadyExists(Guid employeeId) : base($"Employee with id: {employeeId} already exists")
		{
			EmployeeId = employeeId;
		}
	}
}