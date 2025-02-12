namespace SecurityApi.Grain.Persistence.Person
{
    [GenerateSerializer]
    public record struct PersonStateGrain(string Nombre, string Apellido);

}
