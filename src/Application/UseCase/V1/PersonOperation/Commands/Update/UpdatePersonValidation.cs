using FluentValidation;

namespace SecurityApi.Application.UseCase.V1.PersonOperation.Commands.Update;

public class UpdatePersonValidation : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonValidation()
    {
        RuleFor(x => x.PersonId)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("PersonId is required")
            .Must(x => Guid.TryParse(x, out Guid _))
            .WithMessage("PersonId must be a uuid");

        RuleFor(x => x.Apellido)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Apellido is invalid")
            .MaximumLength(255)
            .WithMessage("Apellido solo puede tener 255 caracteres");
        RuleFor(x => x.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Nombre is invalid")
            .MaximumLength(255)
            .WithMessage("Nombre solo puede tener 255 caracteres");
    }
}
