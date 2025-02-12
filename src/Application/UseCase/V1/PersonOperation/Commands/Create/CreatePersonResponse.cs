namespace SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Create
{
    public record struct CreatePersonResponse(Guid PersonId, string Message) { }
}
