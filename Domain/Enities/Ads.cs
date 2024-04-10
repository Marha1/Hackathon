using Domain.Validations;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;
namespace Domain.Enities;
public class Ads: BaseEntity
{
    public Ads(int number,string text,int rating,DateTime created,DateTime expirationDate,Guid userId)
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
        UserId = userId;
        Images = new List<string>();
    }
    public Ads()
    {
            
    }
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Ads))
        {
            return false;
        }
        Ads other = (Ads)obj;
        return this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public int Number { get; set; } 
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime Created { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<string>? Images { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ValidationResult Validate()
    {
        var validator = new AdsValidation();
        return validator.Validate(this);
    }
    
}
