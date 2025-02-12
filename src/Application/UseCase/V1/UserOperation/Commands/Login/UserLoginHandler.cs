using Andreani.Arq.Orleans.Abstractions.Directories;
using Andreani.Arq.Pipeline.Clases;
using MediatR;
using SecurityApi.Application.UseCase.V1.UserOperation.Response;
using SecurityApi.Domain.Dtos;
using SecurityApi.Grain.Interface.Exceptions;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.UserOperation.Commands.Login
{
    public record struct UserLoginRequest(UserLoginDto userInfo) : IRequest<Response<LoginResponse>>;
    public class UserLoginHandler : IRequestHandler<UserLoginRequest, Response<LoginResponse>>
    {
        private readonly IClusterClient _client;

        public UserLoginHandler(IClusterClient client)
        {
            _client = client;
        }

        public async Task<Response<LoginResponse>> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var result = new Response<LoginResponse>();
            try
            {
                if (!(await _client.GetGrain<IBigDirectoryStringGrain>(nameof(IUserGrains)).Exist(request.userInfo.Email)))
                {
                    result.AddNotification("#3123", nameof(request.userInfo), "the username and/or password is incorrect");
                    result.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    return result;
                }

                var token = await _client.GetGrain<IUserGrains>(request.userInfo.Email)
                    .Login(request.userInfo.Password);

                result.Content = new(token);
            }
            catch (InvalidLoginException ex)
            {
                result.AddNotification(new Notify
                {
                    Code = "1",
                    Message = ex.Message,
                    Property = nameof(request.userInfo)
                });
            }
            return result;
        }
    }
}
