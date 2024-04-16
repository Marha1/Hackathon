namespace Application.Dtos.AdsDto.Responce;
public class AdsDescendingFiltrationResponce
{
    public Guid Id { get;set; }
    public int Number { get; set; } 
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime Created { get; set; }
    public DateTime ExpirationDate { get; set; }
}