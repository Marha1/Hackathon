using Domain.Enities;
using Domain.Primitives;
using FluentValidation;

namespace Domain.Validations;

/// <summary>
///     Класс валидации объявления
/// </summary>
public class AdsValidation : AbstractValidator<Ads>
{
    private const int MaxTextLength = 100;

    public AdsValidation()
    {
        RuleFor(x => x.Number)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Number)))
            .Must(x => x.GetType() == typeof(int))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Number)));

        RuleFor(x => x.Text)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Text)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Text)))
            .Must(x => x.GetType() == typeof(string))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Text)))
            .MaximumLength(MaxTextLength)
            .WithMessage(x => string.Format(ValidationMessages.IsMaxLength, nameof(x.Text)));

        RuleFor(x => x.Rating)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Rating)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Rating)))
            .Must(x => x.GetType() == typeof(int))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Rating)));

        RuleFor(x => x.Created)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Created)))
            .Must(x => x.GetType() == typeof(DateTime))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Created)));

        RuleFor(x => x.ExpirationDate)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.ExpirationDate)))
            .Must(x => x.GetType() == typeof(DateTime)).WithMessage(x =>
                string.Format(ValidationMessages.IsValidType, nameof(x.ExpirationDate)));

        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Continue)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.UserId)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.UserId)))
            .Must(x => x.GetType() == typeof(Guid))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.UserId)));
    }
}