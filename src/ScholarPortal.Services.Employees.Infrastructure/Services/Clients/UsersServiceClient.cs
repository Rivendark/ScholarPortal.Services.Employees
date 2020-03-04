using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ScholarPortal.Protos.Users;
using ScholarPortal.Services.Employees.Application.DTO;
using ScholarPortal.Services.Employees.Application.Services;

namespace ScholarPortal.Services.Employees.Infrastructure.Services.Clients
{
	public class UsersServiceClient : IUserServiceClient
	{
		private readonly ILogger<UsersServiceClient> _logger;
		private readonly string _url;

		public UsersServiceClient(ILogger<UsersServiceClient> logger, HttpClientOptions options)
		{
			_logger = logger;
			_url = options.Services["identity"];
			_logger.LogInformation($"URI: {_url}");
		}

		public async Task<UserDto> GetUserAsync(Guid id)
		{
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
			using var channel = GrpcChannel.ForAddress(_url);
			var client = new QueryUsersService.QueryUsersServiceClient(channel);
			var userRequest = new UserRequest {IdentityId = id.ToString()};
			try
			{
				var userModel = await client.GetUserAsync(userRequest);
				if (!(userModel is {})) return null;
				_logger.LogInformation($"UserModel found. ID: {userModel.IdentityId}");
				return new UserDto(
					Guid.Parse(userModel.IdentityId),
					userModel.FirstName,
					userModel.LastName,
					userModel.SocialSecurityNumber,
					DateTimeOffset.FromUnixTimeSeconds(userModel.Birthdate.Seconds).DateTime,
					userModel.Email,
					DateTimeOffset.FromUnixTimeSeconds(userModel.Created.Seconds).DateTime,
					(int) userModel.Status,
					Guid.Parse(userModel.EmployeeId)
				);

			}
			catch (RpcException e)
			{
				_logger.LogError($"gRPC failed: {e.Message}");
				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}