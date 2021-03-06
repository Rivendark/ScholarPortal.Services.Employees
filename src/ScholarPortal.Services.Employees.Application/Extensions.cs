using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;

namespace ScholarPortal.Services.Employees.Application
{
	public static class Extensions
	{
		public static IConveyBuilder AddApplication(this IConveyBuilder builder)
		{
			return builder
				.AddCommandHandlers()
				.AddEventHandlers()
				.AddInMemoryCommandDispatcher()
				.AddInMemoryEventDispatcher();
		}
	}
}