using Andreani.Arq.Orleans.Abstractions.Directories;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using SecurityApi.Grain.Interface.Exceptions;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;
using SecurityApi.Grain.Interfaces.States;
using SecurityApi.Grain.Persistence.Person;

namespace SecurityApi.Grain.Persistence.User
{
    public class UserGrain : IGrainBase, IUserGrains
    {
        private readonly IPersistentState<UserStateGrain> _userState;
        private string _username = string.Empty;
        private readonly IGrainFactory _grainFactory;
        private readonly ILogger<PersonGrains> _logger;

        public UserGrain([PersistentState("User")] IPersistentState<UserStateGrain> userState,
            IGrainFactory grainFactory,
            ILogger<PersonGrains> logger,
            IGrainContext grainContext)
        {
            _userState = userState;
            _grainFactory = grainFactory;
            _logger = logger;
            GrainContext = grainContext;
        }

        public IGrainContext GrainContext { get; }

        public Task OnActivateAsync(CancellationToken _)
        {
            _username = this.GetPrimaryKeyString();

            _logger.LogInformation("{GrainType} {GrainKey} activated.", GrainContext.GrainId.Type, GrainContext.GrainId.Key);

            return Task.CompletedTask;
        }

        public async Task<string> Login(string Password)
        {
            if (Password == _userState.State.Password)
            {
                _userState.State = _userState.State with { LastLogin = DateTime.Now };

                await _userState.WriteStateAsync();

                return "Token";
            }
            throw new InvalidLoginException("invalid Login");
        }

        public async Task Register(string Password, PersonInfo info)
        {
            if (!string.IsNullOrEmpty(_userState.State.Password))
                throw new InvalidLoginException("El usuario ya existe");

            var person = await _grainFactory.GetGrain<IPersonGrains>(Guid.NewGuid())
                .CreateOrUpdate(info);

            await _grainFactory.GetGrain<IBigDirectoryStringGrain>(nameof(IUserGrains)).AddAsync(_username);

            _userState.State = new UserStateGrain
            {
                PersonId = person.Id,
                LastLogin = null,
                Password = Password,
            };

            await _userState.WriteStateAsync();
        }

        public async Task<UserDto> GetState()
        {
            return new UserDto(_username, await _grainFactory.GetGrain<IPersonGrains>(_userState.State.PersonId).GetState(), _userState.State.LastLogin);
        }
    }
}
