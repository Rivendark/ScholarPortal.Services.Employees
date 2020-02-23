using ScholarPortal.Services.Employees.Core.Entities;

namespace ScholarPortal.Services.Employees.Core.Repositories
{
	public interface IEmployeeDocument
	{
		public string Title { get; set; }
		public EmployeeState State { get; set; }
	}
}