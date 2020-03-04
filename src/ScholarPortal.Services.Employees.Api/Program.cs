using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ScholarPortal.Services.Employees.Application;
using ScholarPortal.Services.Employees.Application.Commands;
using ScholarPortal.Services.Employees.Application.DTO;
using ScholarPortal.Services.Employees.Application.Queries;
using ScholarPortal.Services.Employees.Infrastructure;

namespace ScholarPortal.Services.Employees.Api
{
	public class Program
	{
		public static async Task Main(string[] args)
			=> await CreateWebHostBuilder(args)
				.Build()
				.RunAsync();

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
			=> WebHost.CreateDefaultBuilder(args)
				.ConfigureServices(services => services
					.AddConvey()
					.AddWebApi()
					.AddApplication()
					.AddInfrastructure()
					.Build()
				)
				.Configure(app => app
					.UseInfrastructure()
					.UseDispatcherEndpoints(endpoints => endpoints
						.Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
						.Get<GetEmployee, EmployeeDto>("employees/{employeeId}")
						.Post<CreateEmployee>("employees/new",
							afterDispatch: (cmd, ctx) => ctx.Response.Created($"employees/{cmd.EmployeeId}"))
						.Post<AddEmployeeToUser>("employees/add",
							afterDispatch: (cmd, ctx) => ctx.Response.Created($"employees/{cmd.EmployeeId}"))
					)
				)
				.UseLogging();
	}
}