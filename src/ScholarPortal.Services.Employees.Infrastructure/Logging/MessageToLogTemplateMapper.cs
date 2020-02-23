using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using ScholarPortal.Services.Employees.Application.Commands;

namespace ScholarPortal.Services.Employees.Infrastructure.Logging
{
	public class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
	{
		private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
			=> new Dictionary<Type, HandlerLogTemplate>
			{
				{
					typeof(CreateEmployee),     
					new HandlerLogTemplate
					{
						After = "Created Employee with id: {EmployeeId}."
					}
				},
			};
        
		public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
		{
			var key = message.GetType();
			return MessageTemplates.TryGetValue(key, out var template) ? template : null;
		}
	}
}