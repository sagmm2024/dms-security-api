namespace SecurityApi.Grain.Interfaces.Dto
{
    [GenerateSerializer]
    public record struct UserDto(string Username, PersonDto Info, DateTime? LastLogin);
}
