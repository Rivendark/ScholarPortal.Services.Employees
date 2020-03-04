using System;

namespace ScholarPortal.Services.Employees.Application.Exceptions
{
	public class UserNotFound : AppException
	{
		public override string Code => "user_not_found";
		
		public Guid IdentityId { get; }
		
		public UserNotFound(Guid identityId) : base($"User with id: {identityId} does not exist")
		{
			IdentityId = identityId;
		}
	}
}