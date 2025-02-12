using Andreani.Arq.Pipeline.Clases;
using Andreani.Arq.WebHost.Controllers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Create;
using SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Update;
using SecurityApi.Application.UseCase.V1.PersonOperation.Queries.GetList;
using SecurityApi.Grain.Interfaces.Dto;
using WebApi.Models;

namespace SecurityApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PersonController : ApiControllerBase
{
    /// <summary>
    /// Creación de nueva persona
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreatePersonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreatePersonCommand body) => Result(await Mediator.Send(body));

    /// <summary>
    /// Listado de persona de la base de datos
    /// </summary>
    /// <remarks>en los remarks podemos documentar información más detallada</remarks>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<PersonDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get() => Result(await Mediator.Send(new ListPerson()));

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, UpdatePersonVm body)
    {
        return Result(await Mediator.Send(new UpdatePersonCommand
        {
            PersonId = id,
            Apellido = body.Apellido,
            Nombre = body.Nombre
        }));
    }

}
