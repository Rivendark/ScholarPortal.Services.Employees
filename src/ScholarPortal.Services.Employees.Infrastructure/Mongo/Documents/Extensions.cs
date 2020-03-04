using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ScholarPortal.Services.Employees.Application.DTO;
using ScholarPortal.Services.Employees.Core.Entities;

namespace ScholarPortal.Services.Employees.Infrastructure.Mongo.Documents
{
	public static class Extensions
	{
		public static EmployeeDocument AsDocument(this Employee entity)
			=> new EmployeeDocument(
				entity.Id,
				entity.Title,
				entity.CreatedAt,
				entity.State,
				entity.User.Id
			);

		public static EmployeeDocument AsDocument(this EmployeeDto dto)
			=> new EmployeeDocument(
				dto.Id,
				dto.Title,
				dto.CreatedAt,
				dto.State,
				dto.User
			);
		
		public static Employee AsEmployee(this EmployeeDocument document)
			=> new Employee(
				document.Id,
				document.Title,
				document.CreatedAt,
				document.State,
				document.UserId
			);

		public static async Task<Employee> AsEmployeeAsync(this Task<EmployeeDocument> task)
			=> (await task).AsEmployee();
		
		public static Employee AsEmployee(this EmployeeDto dto)
			=> new Employee(
				dto.Id,
				dto.Title,
				dto.CreatedAt,
				dto.State,
				dto.User
			);
		
		public static EmployeeDto AsDto(this Employee entity)
			=> new EmployeeDto
			{
				Id = entity.Id,
				Title = entity.Title,
				CreatedAt = entity.CreatedAt,
				State = entity.State,
				User = entity.User.Id
			};
		
		public static EmployeeDto AsDto(this EmployeeDocument document)
			=> new EmployeeDto
			{
				Id = document.Id,
				Title = document.Title,
				CreatedAt = document.CreatedAt,
				State = document.State,
				User = document.UserId
			};
	}
}