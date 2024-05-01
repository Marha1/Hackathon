using Application.Dtos.AdsDto.Responce;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.AdsRequestValidation;

public class AdsGetByTextRequestValidation : AbstractValidator<AdsGetByTextResponce>
{
    private const int MaxTextLength = 100;

    public AdsGetByTextRequestValidation()
    {
        RuleFor(x => x.Text)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Text)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Text)))
            .Must(x => x.GetType() == typeof(string))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Text)))
            .MaximumLength(MaxTextLength)
            .WithMessage(x => string.Format(ValidationMessages.IsMaxLength, nameof(x.Text)));
    }
}