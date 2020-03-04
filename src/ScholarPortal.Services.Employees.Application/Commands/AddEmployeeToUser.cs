using System;
using Convey.CQRS.Commands;

namespace ScholarPortal.Services.Employees.Application.Commands
{
	public class AddEmployeeToUser : ICommand
	{
		public Guid EmployeeId { get; }
		public Guid IdentityId { get; }

		public AddEmployeeToUser(Guid identityId, Guid employeeId)
		{
			IdentityId = identityId;
			EmployeeId = employeeId;
		}
	}
}