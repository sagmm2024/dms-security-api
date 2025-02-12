using Andreani.Arq.Orleans.Abstractions.Directories;
using Andreani.Arq.Pipeline.Clases;
using MediatR;
using SecurityApi.Domain.Dtos;
using SecurityApi.Grain.Interface.Exceptions;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.UserOperation.Commands.UserRegister
{
    public record struct UserRegisterRequest(UserRegisterDto userInfo) : IRequest<Response<UserRegisterDto>>;
    public class UserRegisterHandler : IRequestHandler<UserRegisterRequest, Response<UserRegisterDto>>
    {
        private readonly IClusterClient _client;

        public UserRegisterHandler(IClusterClient client)
        {
            _client = client;
        }

        public async Task<Response<UserRegisterDto>> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            var result = new Response<UserRegisterDto>();
            try
            {

                if (await _client.GetGrain<IBigDirectoryStringGrain>(nameof(IUserGrains)).Exist(request.userInfo.Email))
                {
                    result.AddNotification("#3123", nameof(request.userInfo), "the email has already been used");
                    result.StatusCode = System.Net.HttpStatusCode.Conflict;
                    return result;
                }
                await _client.GetGrain<IUserGrains>(request.userInfo.Email)
                    .Register(request.userInfo.Password, new(request.userInfo.Name, request.userInfo.Lastname));

                result.Content = request.userInfo;
                result.StatusCode = System.Net.HttpStatusCode.Created;
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
