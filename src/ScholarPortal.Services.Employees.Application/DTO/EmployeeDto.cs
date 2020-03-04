using System;
using ScholarPortal.Services.Employees.Core.Entities;

namespace ScholarPortal.Services.Employees.Application.DTO
{
	public class EmployeeDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime CreatedAt { get; set; }
		public EmployeeState State { get; set; }
		public Guid User { get; set; }
	}
}