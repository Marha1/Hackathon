using Application.Dtos.AdsDto.Request;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.AdsRequestValidation;

/// <summary>
///     Класс валидации запроса обновления дто объявления
/// </summary>
public class AdsUpdateRequestValidation : AbstractValidator<AdsUpdateRequest>
{
    public AdsUpdateRequestValidation()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .Must(x => x.GetType() == typeof(Guid))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Id)));
    }
}