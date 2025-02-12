using Andreani.Arq.Orleans.Abstractions.Directories;
using Andreani.Arq.Pipeline.Clases;
using MediatR;
using SecurityApi.Domain.Common;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Delete
{
    public record struct DeletePersonRequest(Guid Id) : IRequest<Response<PersonDto>>;

    public class GetPersonHandler : IRequestHandler<DeletePersonRequest, Response<PersonDto>>
    {
        private readonly IClusterClient _client;

        public GetPersonHandler(IClusterClient client)
        {
            _client = client;
        }

        public async Task<Response<PersonDto>> Handle(DeletePersonRequest request, CancellationToken cancellationToken)
        {
            var response = new Response<PersonDto>();
            if (!(await _client.GetGrain<IBigDirectoryGuidGrain>(nameof(IPersonGrains)).Exist(request.Id)))
            {
                response.AddNotification("#3123", nameof(request.Id), string.Format(ErrorMessage.NOT_FOUND_RECORD, "Person", request.Id));
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }
            var grain = _client.GetGrain<IPersonGrains>(request.Id);
            response.Content = await grain.GetState();

            await grain.Delete();

            return response;
        }
    }
}
