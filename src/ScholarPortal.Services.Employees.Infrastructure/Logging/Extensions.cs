using Convey;
using Convey.Logging.CQRS;
using Microsoft.Extensions.DependencyInjection;
using ScholarPortal.Services.Employees.Application.Commands;

namespace ScholarPortal.Services.Employees.Infrastructure.Logging
{
	internal static class Extensions
	{
		public static IConveyBuilder AddHandlersLogging(this IConveyBuilder builder)
		{
			var assembly = typeof(CreateEmployee).Assembly;
            
			builder.Services.AddSingleton<IMessageToLogTemplateMapper>(new MessageToLogTemplateMapper());
            
			return builder
				.AddCommandHandlersLogging(assembly)
				.AddEventHandlersLogging(assembly);
		}
	}
}