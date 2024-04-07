using Domain.Validations;
using FluentValidation;
using FluentValidation.Results;
namespace Domain.Enities;
public class User: BaseEntity
{
    public User(string name,bool isAdmin)
    {
        var validationResult = Validate();
        if (!validationResult.IsValid)
        {
            string errorMessages = string.Join("\n", validationResult.Errors);
            throw new ValidationException(errorMessages);
        }
        this.Name = name;
        this.Admin = isAdmin;
    }

    public User()
    {
        
    }
    public string Name { get; set; }
    public bool Admin { get; set; }
    public ICollection<Ads> Ads { get; set; }
    public ValidationResult Validate()
    {
        var validator = new UserValidation();
        return validator.Validate(this);
    }
}