using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.Outbox.Mongo;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ScholarPortal.Services.Employees.Application;
using ScholarPortal.Services.Employees.Application.Commands;
using ScholarPortal.Services.Employees.Application.Services;
using ScholarPortal.Services.Employees.Core.Repositories;
using ScholarPortal.Services.Employees.Infrastructure.Contexts;
using ScholarPortal.Services.Employees.Infrastructure.Decorators;
using ScholarPortal.Services.Employees.Infrastructure.Jaeger;
using ScholarPortal.Services.Employees.Infrastructure.Logging;
using ScholarPortal.Services.Employees.Infrastructure.Mongo.Documents;
using ScholarPortal.Services.Employees.Infrastructure.Mongo.Repositories;
using ScholarPortal.Services.Employees.Infrastructure.Services;
using ScholarPortal.Services.Employees.Infrastructure.Services.Clients;

namespace ScholarPortal.Services.Employees.Infrastructure
{
	public static class Extensions
	{
		public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
		{
			builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
			builder.Services.AddTransient<IMessageBroker, MessageBroker>();
			builder.Services.AddTransient<IEmployeeRepository, EmployeeMongoRepository>();
			builder.Services.AddTransient<IAppContextFactory, AppContextFactory>();
			builder.Services.AddTransient<IUserServiceClient, UsersServiceClient>();
			builder.Services.AddTransient(ctx => ctx.GetRequiredService<IAppContextFactory>().Create());
			builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(OutboxCommandHandlerDecorator<>));
			builder.Services.TryDecorate(typeof(IEventHandler<>), typeof(OutboxEventHandlerDecorator<>));

			return builder
				.AddQueryHandlers()
				.AddInMemoryQueryDispatcher()
				.AddHttpClient()
				.AddConsul()
				.AddFabio()
				.AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
				.AddMessageOutbox(o => o.AddMongo())
				.AddMongo()
				.AddRedis()
				.AddMetrics()
				.AddJaeger()
				.AddJaegerDecorators()
				.AddHandlersLogging()
				.AddMongoRepository<EmployeeDocument, Guid>("employees");
		}

		public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
		{
			app
				.UseConvey()
				.UsePublicContracts<ContractAttribute>()
				.UseMetrics()
				.UseRabbitMq()
				.SubscribeCommand<CreateEmployee>();
			return app;
		}
		
		internal static CorrelationContext GetCorrelationContext(this IHttpContextAccessor accessor)
			=> accessor.HttpContext?.Request.Headers.TryGetValue("Correlation-Context", out var json) is true
				? JsonConvert.DeserializeObject<CorrelationContext>(json.FirstOrDefault())
				: null;
		
		internal static IDictionary<string, object> GetHeadersToForward(this IMessageProperties messageProperties)
		{
			const string sagaHeader = "Saga";
			if (messageProperties?.Headers is null || !messageProperties.Headers.TryGetValue(sagaHeader, out var saga))
			{
				return null;
			}

			return saga is null
				? null
				: new Dictionary<string, object>
				{
					[sagaHeader] = saga
				};
		}
        
		internal static string GetSpanContext(this IMessageProperties messageProperties, string header)
		{
			if (messageProperties is null)
			{
				return string.Empty;
			}

			if (messageProperties.Headers.TryGetValue(header, out var span) && span is byte[] spanBytes)
			{
				return Encoding.UTF8.GetString(spanBytes);
			}

			return string.Empty;
		}
	}
}