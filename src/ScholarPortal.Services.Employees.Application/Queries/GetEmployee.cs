using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using ScholarPortal.Services.Employees.Application.DTO;

namespace ScholarPortal.Services.Employees.Application.Queries
{
	public class GetEmployee : IQuery<EmployeeDto>
	{
		public Guid EmployeeId { get; set; }
	}
}