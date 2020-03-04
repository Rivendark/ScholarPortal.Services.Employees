using System;
using System.Threading.Tasks;
using ScholarPortal.Services.Employees.Application.DTO;

namespace ScholarPortal.Services.Employees.Application.Services
{
	public interface IUserServiceClient
	{
		Task<UserDto> GetUserAsync(Guid id);
	}
}