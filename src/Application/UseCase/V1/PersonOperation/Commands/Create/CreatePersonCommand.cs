using Andreani.Arq.Pipeline.Clases;
using MediatR;
using Microsoft.Extensions.Logging;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Create
{
    public class CreatePersonCommand : IRequest<Response<CreatePersonResponse>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>Lucas</example>
        public string Nombre { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>Olivera</example>
        public string Apellido { get; set; }
    }

    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Response<CreatePersonResponse>>
    {
        private readonly IClusterClient _client;
        private readonly ILogger<CreatePersonCommandHandler> _logger;

        public CreatePersonCommandHandler(IClusterClient client, ILogger<CreatePersonCommandHandler> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Response<CreatePersonResponse>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var info = new PersonInfo()
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido
            };
            var person = await _client.GetGrain<IPersonGrains>(Guid.NewGuid())
                                .CreateOrUpdate(info);

            _logger.LogDebug("the person was add correctly");

            return new Response<CreatePersonResponse>
            {
                Content = new CreatePersonResponse
                {
                    Message = "Success",
                    PersonId = person.Id,
                },
                StatusCode = System.Net.HttpStatusCode.Created
            };
        }
    }
}
