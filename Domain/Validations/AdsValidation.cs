using Domain.Enities;
using Domain.Primitives;
using FluentValidation;

namespace Domain.Validations;

public class AdsValidation: AbstractValidator<Ads>
{
    public AdsValidation()
    {
        RuleFor(x => x.Number)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Number)));
        RuleFor(x => x.Number).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure).IsInEnum()
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Number)));
        RuleFor(x=>x.Text).Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Text)));
        RuleFor(x => x.Text).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure).IsInEnum()
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Text)));
        RuleFor(x => x.Rating)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Rating)));
        RuleFor(x => x.Rating).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure).IsInEnum()
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Rating)));
        RuleFor(x => x.Created)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Created)));
        RuleFor(x => x.Created).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure).IsInEnum()
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Created)));
        RuleFor(x => x.ExpirationDate)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.ExpirationDate)));
        RuleFor(x => x.ExpirationDate).Cascade(FluentValidation.CascadeMode.StopOnFirstFailure).IsInEnum()
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.ExpirationDate)));
    }
    
}