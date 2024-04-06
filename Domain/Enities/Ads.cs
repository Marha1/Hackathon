using System.ComponentModel.DataAnnotations;
using Domain.Validations;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace Domain.Enities;

public class Ads: BaseEntity
{
    public Ads(int number,string text,int rating,DateTime created,DateTime expirationDate)
    {
        var validationResult = Validate();
        if (!validationResult.IsValid)
        {
            string errorMessages = string.Join("\n", validationResult.Errors);
            throw new ValidationException(errorMessages);
        }
        Number = number;
        Text = text;
        Rating = rating;
        Created = created;
        ExpirationDate = expirationDate;
    }
    public int Number { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime Created { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ValidationResult Validate()
    {
        var validator = new AdsValidation();
        return validator.Validate(this);
    }
    
}
