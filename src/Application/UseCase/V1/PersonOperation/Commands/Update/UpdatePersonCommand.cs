using Andreani.Arq.Orleans.Abstractions.Directories;
using Andreani.Arq.Pipeline.Clases;
using MediatR;
using SecurityApi.Domain.Common;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Update;

public class UpdatePersonCommand : IRequest<Response<PersonDto>>
{
    public string PersonId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
}
public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, Response<PersonDto>>
{
    private readonly IClusterClient _client;

    public UpdatePersonHandler(IClusterClient client)
    {
        _client = client;
    }

    public async Task<Response<PersonDto>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<PersonDto>();

        Guid _id = Guid.Parse(request.PersonId);
        var exist = await _client.GetGrain<IBigDirectoryGuidGrain>(nameof(IPersonGrains)).Exist(_id);

        if (!exist)
        {
            response.AddNotification("#3123", nameof(request.PersonId), string.Format(ErrorMessage.NOT_FOUND_RECORD, "Person", request.PersonId));
            response.StatusCode = System.Net.HttpStatusCode.NotFound;
            return response;
        }
        var info = new PersonInfo()
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido
        };
        var result = await _client.GetGrain<IPersonGrains>(_id)
                    .CreateOrUpdate(info);

        response.Content = result;
        return response;
    }
}
