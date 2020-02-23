using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using ScholarPortal.Services.Employees.Application.Events;
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

		public CreateEmployeeHandler(
			IEmployeeRepository employeeRepository,
			IDateTimeProvider dateTimeProvider,
			ILogger<CreateEmployeeHandler> logger,
			IMessageBroker broker
		)
		{
			_employeeRepository = employeeRepository;
			_dateTimeProvider = dateTimeProvider;
			_logger = logger;
			_broker = broker;
		}

		public async Task HandleAsync(CreateEmployee command)
		{
			if (await _employeeRepository.ExistsAsync(command.EmployeeId))
			{
				throw new Exception($"Employee already exists. ({command.EmployeeId})");
			}
			var employee = new Employee(
				command.EmployeeId,
				"",
				EmployeeState.Incomplete,
				command.IdentityId
			);

			await _employeeRepository.CreateAsync(employee);
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