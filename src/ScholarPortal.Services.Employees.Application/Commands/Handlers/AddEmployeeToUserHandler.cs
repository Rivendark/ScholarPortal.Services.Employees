using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using ScholarPortal.Services.Employees.Application.Events;
using ScholarPortal.Services.Employees.Application.Exceptions;
using ScholarPortal.Services.Employees.Application.Services;
using ScholarPortal.Services.Employees.Core.Entities;
using ScholarPortal.Services.Employees.Core.Repositories;

namespace ScholarPortal.Services.Employees.Application.Commands.Handlers
{
	public class AddEmployeeToUserHandler : ICommandHandler<AddEmployeeToUser>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ILogger<AddEmployeeToUserHandler> _logger;
		private readonly IMessageBroker _broker;
		private readonly IUserServiceClient _userServiceClient;

		public AddEmployeeToUserHandler(
			IEmployeeRepository employeeRepository,
			IDateTimeProvider dateTimeProvider,
			ILogger<AddEmployeeToUserHandler> logger,
			IMessageBroker broker,
			IUserServiceClient userServiceClient
		)
		{
			_employeeRepository = employeeRepository;
			_dateTimeProvider = dateTimeProvider;
			_logger = logger;
			_broker = broker;
			_userServiceClient = userServiceClient;
		}
		public async Task HandleAsync(AddEmployeeToUser command)
		{
			if (await _employeeRepository.ExistsAsync(command.EmployeeId))
			{
				_logger.LogError($"Attempt to create already existing Employee. ID: {command.EmployeeId}");
				throw new EmployeeAlreadyExists(command.EmployeeId);
			}

			var user = await _userServiceClient.GetUserAsync(command.IdentityId);
			if (user is null)
			{
				_logger.LogError($"Attempt to create Employee on non existent User. ID: {command.IdentityId}");
				throw new UserNotFound(command.IdentityId);
			}
			if (user.Status != 1)
			{
				_logger.LogError($"Attempt to create Employee on invalid User. ID: {command.IdentityId}");
				throw new InvalidUser(command.IdentityId);
			}
			
			var employee = new Employee(
				command.EmployeeId,
				"",
				_dateTimeProvider.Now,
				EmployeeState.Incomplete,
				command.IdentityId
			);
			await _employeeRepository.CreateAsync(employee);
			_logger.LogDebug($"Employee created. EmployeeID: {command.EmployeeId} UserID: {command.IdentityId}");
			await _broker.PublishAsync(
				new EmployeeAdded(
					command.EmployeeId,
					command.IdentityId
			));
		}
	}
}