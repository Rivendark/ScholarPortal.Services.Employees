using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using ScholarPortal.Services.Employees.Application.DTO;
using ScholarPortal.Services.Employees.Core.Entities;
using ScholarPortal.Services.Employees.Infrastructure.Mongo.Documents;
using ScholarPortal.Services.Employees.Core.Repositories;

namespace ScholarPortal.Services.Employees.Infrastructure.Mongo.Repositories
{
	public class EmployeeMongoRepository : IEmployeeRepository
	{
		private readonly IMongoRepository<EmployeeDocument, Guid> _repository;

		public EmployeeMongoRepository(IMongoRepository<EmployeeDocument, Guid> repository)
		{
			_repository = repository;
		}

		public Task<bool> ExistsAsync(Guid id)
			=> _repository.ExistsAsync(r => r.Id == id);

		public async Task<Employee> GetAsync(Guid id)
			=> await _repository.GetAsync(id).AsEmployeeAsync();

		public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
		{
			var employees = await _repository.FindAsync(_ => true);
			return employees.Select(p => p.AsDto());
		}

		public async Task CreateAsync(Employee employee)
			=> await _repository.AddAsync(employee.AsDocument());

		public async Task UpdateAsync(Employee employee)
			=> await _repository.UpdateAsync(employee.AsDocument());

		public async Task DeleteAsync(Employee employee)
			=> await DeleteAsync(employee.Id);
		public async Task DeleteAsync(Guid id)
			=> await _repository.DeleteAsync(id);
	}
}