using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ScholarPortal.Services.Employees.Application.Events;
using ScholarPortal.Services.Employees.Application.Exceptions;
using ScholarPortal.Services.Employees.Application.Services;
using ScholarPortal.Services.Employees.Core.Entities;
using ScholarPortal.Services.Employees.Core.Repositories;

namespace ScholarPortal.Services.Employees.Application.Commands.Handlers
{
	public class CreateEmployeeHandler : ICommandHandler<CreateEmployee>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly ILogger<CreateEmployeeHandler> _logger;
		private readonly IMessageBroker _broker;
		private readonly IUserServiceClient _userServiceClient;

		public CreateEmployeeHandler(
			IEmployeeRepository employeeRepository,
			IDateTimeProvider dateTimeProvider,
			ILogger<CreateEmployeeHandler> logger,
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

		public async Task HandleAsync(CreateEmployee command)
		{
			if (await _employeeRepository.ExistsAsync(command.EmployeeId))
			{
				_logger.LogError($"Attempt to create already existing Employee. ID: {command.EmployeeId}");
				throw new EmployeeAlreadyExists(command.EmployeeId);
			}

			
			var user = await _userServiceClient.GetUserAsync(command.IdentityId);
			if (user is null) {
				var employee = new Employee(
					command.EmployeeId,
					"",
					_dateTimeProvider.Now,
					EmployeeState.Incomplete,
					command.IdentityId
				);

				await _employeeRepository.CreateAsync(employee);
				_logger.LogError($"Employee created. EmployeeID: {command.EmployeeId} UserID: {command.IdentityId}");
				await _broker.PublishAsync(
					new EmployeeCreated(
						command.EmployeeId,
						command.IdentityId,
						command.FirstName,
						command.LastName,
						command.Email,
						command.Password,
						command.Birthday
					));
			}
		}
	}
}