using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using ScholarPortal.Services.Employees.Application.DTO;
using ScholarPortal.Services.Employees.Application.Queries;
using ScholarPortal.Services.Employees.Infrastructure.Mongo.Documents;

namespace ScholarPortal.Services.Employees.Infrastructure.Mongo.Queries.Handlers
{
	public class GetEmployeeHandler : IQueryHandler<GetEmployee, EmployeeDto>
	{
		private readonly IMongoRepository<EmployeeDocument, Guid> _repository;
		private readonly ILogger<GetEmployeeHandler> _logger;

		public GetEmployeeHandler(IMongoRepository<EmployeeDocument, Guid> repository, ILogger<GetEmployeeHandler> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		public async Task<EmployeeDto> HandleAsync(GetEmployee query)
		{
			var document = await _repository.GetAsync(e => e.Id == query.EmployeeId);
			var all = await _repository.FindAsync(_ => true);
			var allDoc = all.Select(d => d.AsDto());
			_logger.LogInformation("Employee Documents found: " + allDoc.First().Id);
			return document?.AsDto();
		}
	}
}