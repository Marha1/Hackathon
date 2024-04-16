namespace Application.Dtos.AdsDto;

public class AdsCreateResponse 
{
    public Guid Id { get;set; }
    public int Number { get; set; } 
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime Created { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<string>? Images { get; set; }
    public Guid UserId { get; set; }
}
