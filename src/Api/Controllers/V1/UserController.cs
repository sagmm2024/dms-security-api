using Andreani.Arq.Pipeline.Clases;
using Andreani.Arq.WebHost.Controllers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SecurityApi.Application.UseCase.V1.UserOperation.Commands.Login;
using SecurityApi.Application.UseCase.V1.UserOperation.Commands.UserRegister;
using SecurityApi.Application.UseCase.V1.UserOperation.Queries.GetList;
using SecurityApi.Application.UseCase.V1.UserOperation.Queries.GetUser;
using SecurityApi.Application.UseCase.V1.UserOperation.Response;
using SecurityApi.Domain.Dtos;
using SecurityApi.Grain.Interfaces.Dto;

namespace SecurityApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ApiControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegisterDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegisterDto body) => Result(await Mediator.Send(new UserRegisterRequest(body)));

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login(UserLoginDto body) => Result(await Mediator.Send(new UserLoginRequest(body)));

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInfo(string username) => Result(await Mediator.Send(new GetUserRequest(username)));

        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Notify>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll(string? offset = null, int limit = 10) => Result(await Mediator.Send(new GetListUserRequest(offset, limit)));
    }
}
