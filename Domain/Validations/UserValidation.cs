using Domain.Enities;
using Domain.Primitives;
using FluentValidation;
namespace Domain.Validations;
public class UserValidation:AbstractValidator<User>
{
    private const int MaxNameLength = 100;
    public UserValidation()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Name)))
            .Matches(@"^[a-zа-я]+$").WithMessage(x => string.Format(ValidationMessages.IsValidString, nameof(x.Name)))
            .Must(x=>x.GetType() == typeof(string)).WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Name)))
            .MaximumLength(MaxNameLength).WithMessage(x=>string.Format(ValidationMessages.IsMaxLength,nameof(x.Name)));

        RuleFor(x => x.Admin)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Admin)))
            .Must(x=>x.GetType() == typeof(bool)).WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Admin)));
    }
    
}