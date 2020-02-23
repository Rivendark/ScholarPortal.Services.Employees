using System;

namespace ScholarPortal.Services.Employees.Core.Entities
{
	public class Employee
	{
		public Guid Id { get; private set; }
		public string Title { get; private set; }
		public EmployeeState State { get; private set; }
		public User User { get; private set; }

		public Employee(Guid id, string title, EmployeeState state, Guid identityId)
		{
			Id = id;
			Title = title ?? throw new ArgumentNullException(nameof(title));
			State = state;
			User = new User(identityId);
		}
	}
}