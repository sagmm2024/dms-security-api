
using Andreani.Arq.Orleans.Abstractions.Directories;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using SecurityApi.Grain.Interfaces.Dto;
using SecurityApi.Grain.Interfaces.Interfaces.Persistence;

namespace SecurityApi.Grain.Persistence.Person
{
    public class PersonGrains : IGrainBase, IPersonGrains
    {
        private readonly IPersistentState<PersonStateGrain> _state;
        private Guid _id;
        private readonly IGrainFactory _grainFactory;
        private readonly ILogger<PersonGrains> _logger;

        public IGrainContext GrainContext { get; }

        public PersonGrains([PersistentState("Person")] IPersistentState<PersonStateGrain> state,
            IGrainFactory grainFactory,
            IGrainContext context,
            ILogger<PersonGrains> logger)
        {
            _state = state;
            GrainContext = context;
            _grainFactory = grainFactory;
            _logger = logger;
        }

        public Task OnActivateAsync(CancellationToken _)
        {

            _id = this.GetPrimaryKey();

            _logger.LogInformation("{GrainType} {GrainKey} activated.", GrainContext.GrainId.Type, GrainContext.GrainId.Key);

            return Task.CompletedTask;
        }


        public Task<PersonDto> GetState() => Task.FromResult(MapState());


        public async Task<PersonDto> CreateOrUpdate(PersonInfo info)
        {
            _state.State = new PersonStateGrain()
            {
                Apellido = info.Apellido,
                Nombre = info.Nombre,
            };

            await _state.WriteStateAsync();

            await _grainFactory.GetGrain<IBigDirectoryGuidGrain>(nameof(IPersonGrains)).AddAsync(_id);

            return MapState();
        }

        public async Task Delete()
        {
            await _grainFactory.GetGrain<IBigDirectoryGuidGrain>(nameof(IPersonGrains)).RemoveAsync(_id);
            await _state.ClearStateAsync();
            this.DeactivateOnIdle();
        }

        private PersonDto MapState()
        {
            return new()
            {
                Id = _id,
                Nombre = _state.State.Nombre,
                Apellido = _state.State.Apellido
            };
        }
    }
}
