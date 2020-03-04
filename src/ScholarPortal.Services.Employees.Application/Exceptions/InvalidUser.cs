using System;

namespace ScholarPortal.Services.Employees.Application.Exceptions
{
	public class InvalidUser : AppException
	{
		public override string Code => "invalid_user";
		
		public Guid IdentityId { get; }
		
		public InvalidUser(Guid identityId) : base($"Invalid user with id: {identityId}")
		{
			IdentityId = identityId;
		}
	}
}