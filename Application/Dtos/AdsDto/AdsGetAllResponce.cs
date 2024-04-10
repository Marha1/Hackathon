using Domain.Enities;

namespace Application.Dtos.AdsDto;

public class AdsGetAllResponce
{
    
    public Guid Id { get;set; }
    public int Number { get; set; } 
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime Created { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Images { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<BaseUserDto> User { get; set; }
    public IEnumerable<Ads> Ads { get; set; }
}