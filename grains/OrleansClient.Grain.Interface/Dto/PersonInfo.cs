namespace SecurityApi.Grain.Interfaces.Dto
{
    [GenerateSerializer]
    public record struct PersonInfo(string Nombre, string Apellido);
    [GenerateSerializer]
    public record struct PersonDto(Guid Id, string Nombre, string Apellido);

}
