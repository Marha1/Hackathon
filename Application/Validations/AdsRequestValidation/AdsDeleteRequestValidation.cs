using Application.Dtos.AdsDto.Request;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.AdsRequestValidation;

/// <summary>
///     Класс валидации запроса удаления дто объявления
/// </summary>
public class AdsDeleteRequestValidation : AbstractValidator<AdsDeleteRequest>
{
    public AdsDeleteRequestValidation()
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .Must(x => x.GetType() == typeof(Guid))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Id)));
    }
}