using System;
using System.Threading.Tasks;
using ScholarPortal.Services.Employees.Core.Entities;

namespace ScholarPortal.Services.Employees.Core.Repositories
{
	public interface IEmployeeRepository
	{
		Task<bool> ExistsAsync(Guid id);
		Task<Employee> GetAsync(Guid id);
		Task CreateAsync(Employee employee);
		Task UpdateAsync(Employee employee);
		Task DeleteAsync(Guid id);
	}
}