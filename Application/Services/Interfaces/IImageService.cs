namespace Application.Services.Interfaces;

public interface IImageService
{
  public Task<string> SaveImages(IFormFile file);
  public Task<string> UploadImages(IFormFile file, Guid adsId);
}