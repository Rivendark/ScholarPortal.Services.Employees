using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Convey.Types;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ScholarPortal.Services.Employees.Application.DTO;
using ScholarPortal.Services.Identity.Infrastructure;

namespace ScholarPortal.Services.Employees.Infrastructure.Services.Clients
{
	
	public class UsersServiceClient
	{
		private readonly ILogger<UsersServiceClient> _logger;
		private readonly string _url;

		public UsersServiceClient(ILogger<UsersServiceClient> logger, HttpClientOptions options)
		{
			_logger = logger;
			_url = options.Services["identity"];
		}

		public async Task<UserDto> GetUserAsync(Guid id)
		{
			using var channel = GrpcChannel.ForAddress($"{_url}");
			var client = new QueryUsersService.QueryUsersServiceClient(channel);
			var userRequest = new UserRequest {IdentityId = id.ToString()};
			var userReply = await client.GetUserAsync(userRequest);

			return new UserDto(
				Guid.Parse(userReply.IdentityId),
				userReply.FirstName,
				userReply.LastName,
				userReply.SocialSecurityNumber,
				DateTimeOffset.FromUnixTimeSeconds(userReply.Birthdate.Seconds).DateTime,
				userReply.Email,
				DateTimeOffset.FromUnixTimeSeconds(userReply.Created.Seconds).DateTime,
				userReply.Status,
				Guid.Parse(userReply.EmployeeId)
			);
		}
	}
}