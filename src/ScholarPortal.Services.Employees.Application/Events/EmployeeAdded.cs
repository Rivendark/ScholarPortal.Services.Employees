using System;
using Convey.CQRS.Events;

namespace ScholarPortal.Services.Employees.Application.Events
{
	public class EmployeeAdded : IEvent
	{
		public Guid EmployeeId { get; }
		public Guid IdentityId { get; }
		
		public EmployeeAdded(
			Guid employeeId,
			Guid identityId
		) {
			EmployeeId = employeeId;
			IdentityId = identityId;
		}
	}
}