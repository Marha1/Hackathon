using Application.Dtos.AdsDto.Request;
using Application.Services.Interfaces;
using Domain.Primitives;
using FluentValidation;

namespace Application.Validations.AdsRequestValidation;

/// <summary>
///     Класс валидации запроса обновления дто объявления
/// </summary>
public class AdsUpdateRequestValidation : AbstractValidator<AdsUpdateRequest>
{
    private readonly IAdsService _adsService;

    public AdsUpdateRequestValidation(IAdsService adsService)
    {
        _adsService = adsService;


        RuleFor(x => x.Id)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .NotEmpty().WithMessage(x => string.Format(ValidationMessages.IsNullOrEmpty, nameof(x.Id)))
            .Must(x => x.GetType() == typeof(Guid))
            .WithMessage(x => string.Format(ValidationMessages.IsValidType, nameof(x.Id)))
            .MustAsync(async (id, _) =>
            {
                var ad = await _adsService.FindById(id);
                return ad != null;
            })
            .WithMessage("Объявление не найдено");
    }
}