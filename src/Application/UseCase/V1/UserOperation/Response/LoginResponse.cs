using SecurityApi.Grain.Interfaces.Dto;

namespace SecurityApi.Application.UseCase.V1.UserOperation.Response
{
    public record struct LoginResponse(string tokenId);

    public record struct UsersResponse(List<UserDto> Data, string? Next, int limit);
}
