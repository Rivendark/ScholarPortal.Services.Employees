using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace ScholarPortal.Services.Employees.Application.Commands
{
	[Contract]
	public class CreateEmployee : ICommand
	{
		public Guid EmployeeId { get; }
		public Guid IdentityId { get; }
		public string FirstName { get; }
		public string LastName { get; }
		public string Email { get; }
		public string Password { get; }
		public DateTime Birthday { get; }
		public IEnumerable<string> Roles { get; }

		public CreateEmployee(
			Guid employeeId,
			Guid identityId,
			string firstName,
			string lastName,
			string email,
			string password,
			DateTime birthday,
			IEnumerable<string> roles
		) {
			EmployeeId = employeeId;
			IdentityId = identityId;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Password = password;
			Birthday = birthday;
			Roles = roles;
		}
	}
}