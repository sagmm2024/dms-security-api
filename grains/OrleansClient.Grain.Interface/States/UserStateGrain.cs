namespace SecurityApi.Grain.Interfaces.States
{
    [GenerateSerializer]
    public record struct UserStateGrain(string Password, Guid PersonId, DateTime? LastLogin);
}
