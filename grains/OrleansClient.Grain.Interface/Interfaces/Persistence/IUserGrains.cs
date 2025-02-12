using Andreani.Arq.Orleans.Abstractions.Directories;
using Orleans.CodeGeneration;
using SecurityApi.Grain.Interfaces.Dto;

namespace SecurityApi.Grain.Interfaces.Interfaces.Persistence
{
    [Version(1)]
    public interface IUserGrains : IGrainWithStringKey, IGrainWithDirectory<UserDto>
    {
        Task<string> Login(string Password);
        Task Register(string Password, PersonInfo info);

    }
}
