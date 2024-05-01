using Application.Dtos.UserDto.Responce;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.UserRequestValidation;

/// <summary>
///     Класс валидации запроса получения дто пользователя
/// </summary>
public class UserGetByIdRequestValidation : AbstractValidator<UserGetByIdResponse>
{
    public UserGetByIdRequestValidation()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .Must(x => x.GetType() == typeof(Guid))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Id)));
    }
}