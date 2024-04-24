using Application.Dtos.UserDto.Request;
using Application.Services.Interfaces;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.UserRequestValidation;

/// <summary>
///     Класс валидации запроса обновления дто пользователя
/// </summary>
public class UserUpdateRequestValidation : AbstractValidator<UserUpdateRequest>
{
    private readonly IUserService _userService;

    public UserUpdateRequestValidation(IUserService service)
    {
        _userService = service;


        RuleFor(x => x.Id)
            .Cascade(CascadeMode.StopOnFirstFailure)
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