using Application.Dtos.UserDto.Responce;
using Application.Services.Interfaces;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.UserRequestValidation;

/// <summary>
///     Класс валидации запроса получения дто пользователя
/// </summary>
public class UserGetByIdRequest : AbstractValidator<UserGetByIdResponse>
{
    private readonly IUserService _userService;

    public UserGetByIdRequest(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .Must(x => x.GetType() == typeof(Guid))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Id)))
            .MustAsync(async (id, _) =>
            {
                var us = await _userService.FindById(id);
                return us != null;
            })
            .WithMessage(x => string.Format(ValidationMessages.NotFound, nameof(x.Id)));
    }
}