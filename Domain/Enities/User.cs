using Domain.Validations;
using FluentValidation;
using FluentValidation.Results;
namespace Domain.Enities;
public class User: BaseEntity
{
    
    public string Name { get; set; }
    public bool Admin { get; set; }
    public ICollection<Ads> Ads { get; set; }
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
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is User))
        {
            return false;
        }
        User other = (User)obj;
        return this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public User()
    {
            
    }
    public ValidationResult Validate()
    {
        var validator = new UserValidation();
        return validator.Validate(this);
    }
}