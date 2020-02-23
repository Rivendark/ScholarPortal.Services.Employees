using System;

namespace ScholarPortal.Services.Employees.Core.Entities
{
	public class User
	{
		public Guid Id { get; private set; }
		
		public UserStatus Status { get; private set; }

		public User(Guid id, UserStatus status = UserStatus.Invalid)
		{
			Id = id;
			Status = status;
		}
	}
}