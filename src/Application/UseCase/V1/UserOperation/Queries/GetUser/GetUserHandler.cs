using Andreani.Arq.Orleans.Abstractions.Directories;
using Andreani.Arq.Pipeline.Clases;
using MediatR;
using SecurityApi.Grain.Interface.Exceptions;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.UserOperation.Queries.GetUser
{
    public record struct GetUserRequest(string Username) : IRequest<Response<UserDto>>;
    public class GetUserHandler : IRequestHandler<GetUserRequest, Response<UserDto>>
    {
        private readonly IClusterClient _client;

        public GetUserHandler(IClusterClient client)
        {
            _client = client;
        }

        public async Task<Response<UserDto>> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var result = new Response<UserDto>();
            try
            {
                if (!(await _client.GetGrain<IBigDirectoryStringGrain>(nameof(IUserGrains)).Exist(request.Username)))
                {
                    result.AddNotification("#3123", nameof(request.Username), "the username invalid");
                    result.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return result;
                }

                result.Content = await _client.GetGrain<IUserGrains>(request.Username).GetState();
            }
            catch (InvalidLoginException ex)
            {
                result.AddNotification(new Notify
                {
                    Code = "1",
                    Message = ex.Message,
                    Property = nameof(request.Username)
                });
            }
            return result;
        }
    }
}
