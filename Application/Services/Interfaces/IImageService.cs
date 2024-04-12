using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces;

public interface IImageService
{
  public Task<string> SaveImages(IFormFile file);
  public Task<string> UploadImages(IFormFile file, Guid adsId);
  public Task<FileContentResult> GetImage(string fileName);
  public Task<FileContentResult> ResizeAndSaveImage(string fileName, int width, int height);
}