using Domain.Enities;
using Domain.Primitives;
using FluentValidation;

namespace Domain.Validations;

public class UserValidation:AbstractValidator<User>
{
    public UserValidation()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Name)));
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().Matches(@"^[a-zа-я]+$").WithMessage(x=>string.Format(ValidationMessages.IsValidString,nameof(x.Name)));
        RuleFor(x => x.Admin)
            .NotNull().WithMessage(x=>string.Format(ValidationMessages.IsValidString,nameof(x.Admin)))
            .IsInEnum().WithMessage(x=>string.Format(ValidationMessages.IsValidType,nameof(x.Admin)));
    }
}