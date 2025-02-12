using Andreani.Arq.Orleans.Abstractions.Directories;
using Andreani.Arq.Pipeline.Clases;
using MediatR;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.PersonOperation.Queries.GetList
{

    public record struct ListPerson : IRequest<Response<List<PersonDto>>>
    {
    }

    public class ListPersonHandler : IRequestHandler<ListPerson, Response<List<PersonDto>>>
    {
        private readonly IClusterClient _client;

        public ListPersonHandler(IClusterClient client)
        {
            _client = client;
        }

        public async Task<Response<List<PersonDto>>> Handle(ListPerson request, CancellationToken cancellationToken)
        {
            var directory = _client.GetGrain<IBigDirectoryGuidGrain>(nameof(IPersonGrains));
            var grains = await directory.GetAll<IPersonGrains, PersonDto>(Guid.Empty.ToString());

            return new Response<List<PersonDto>>
            {

                Content = grains.Content,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}

