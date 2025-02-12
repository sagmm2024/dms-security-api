using Andreani.Arq.Orleans.Abstractions.Directories;
using Andreani.Arq.Pipeline.Clases;
using MediatR;
using SecurityApi.Application.UseCase.V1.UserOperation.Response;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.UserOperation.Queries.GetList
{
    public record struct GetListUserRequest(string? Offset, int Limit) : IRequest<Response<UsersResponse>>;


    public class GetListUserHandler : IRequestHandler<GetListUserRequest, Response<UsersResponse>>
    {
        private readonly IClusterClient _client;

        public GetListUserHandler(IClusterClient client)
        {
            _client = client;
        }

        public async Task<Response<UsersResponse>> Handle(GetListUserRequest request, CancellationToken cancellationToken)
        {
            var grains = await _client.GetGrain<IBigDirectoryStringGrain>(nameof(IUserGrains))
                                      .GetAll<IUserGrains, UserDto>(request.Offset, limit: request.Limit);

            return new Response<UsersResponse>
            {
                Content = new UsersResponse
                {
                    Data = grains.Content,
                    limit = grains.Limit,
                    Next = grains.Next == null || Guid.Parse(grains.Next) == Guid.Empty ? null : grains.Next,
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
