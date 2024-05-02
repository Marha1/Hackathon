using Application.Dtos.UserDto.Request;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.UserRequestValidation;

/// <summary>
///     Класс валидации запроса Добавления дто пользователя
/// </summary>
public class UserCreateRequestValidation : AbstractValidator<UserCreateRequest>
{
    private const int MaxNameLength = 100;
    public UserCreateRequestValidation()
    {
        CascadeMode = CascadeMode.Continue;

        RuleFor(x => x.Name)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Name)))
            .Matches(@"^[a-zа-я]+$").WithMessage(x => string.Format(ValidationMessages.IsValidString, nameof(x.Name)))
            .Must(x => x.GetType() == typeof(string))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Name)))
            .MaximumLength(MaxNameLength)
            .WithMessage(x => string.Format(ValidationMessages.IsMaxLength, nameof(x.Name)));

        RuleFor(x => x.Admin)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Admin)))
            .Must(x => x.GetType() == typeof(bool))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Admin)));
    }
}