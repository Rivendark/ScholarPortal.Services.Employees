using System;

namespace ScholarPortal.Services.Employees.Application.Exceptions
{
	public class UserAlreadyExists : AppException
	{
		public override string Code => "user_already_exists";
		
		public Guid IdentityId { get; }
		
		public UserAlreadyExists(Guid identityId) : base($"User with id: {identityId} already exists")
		{
			IdentityId = identityId;
		}
	}
}