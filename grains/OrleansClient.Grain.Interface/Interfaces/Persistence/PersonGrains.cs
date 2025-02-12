using Andreani.Arq.Orleans.Abstractions.Directories;
using Orleans.CodeGeneration;
using SecurityApi.Grain.Interfaces.Dto;

namespace SecurityApi.Grain.Interfaces.Interfaces.Persistence
{
    [Version(1)]
    public interface IPersonGrains : IGrainWithDirectory<PersonDto>, IGrainWithGuidKey
    {
        Task<PersonDto> CreateOrUpdate(PersonInfo info);
        Task Delete();
    }
}
