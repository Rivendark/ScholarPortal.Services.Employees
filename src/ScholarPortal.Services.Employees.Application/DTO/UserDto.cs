using System;
using System.Collections.Generic;

namespace ScholarPortal.Services.Employees.Application.DTO
{
	public class UserDto
	{
		private ISet<string> _roles = new HashSet<string>();
		public Guid Id { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string SocialSecurityNumber { get; private set; }
		public DateTime Birthdate { get; private set; }
		public string Email { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public int Status { get; private set; }
		public Guid EmployeeId { get; private set; }

		public UserDto()
		{
		}

		public UserDto(
			Guid id,
			string firstName,
			string lastName,
			string socialSecurityNumber,
			DateTime birthdate,
			string email,
			DateTime createdAt,
			int status,
			Guid employeeId
		) {
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			SocialSecurityNumber = socialSecurityNumber;
			Birthdate = birthdate;
			Email = email;
			CreatedAt = createdAt;
			Status = status;
			EmployeeId = employeeId;
		}
	}
}