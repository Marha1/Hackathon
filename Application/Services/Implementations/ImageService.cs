using Application.Services.Interfaces;
using Domain.Enities;
using Infrastructure.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Application.Services.Implementations;
public class ImageService:IImageService
{
    private readonly IAdsRepository<Ads> _adsRepository;
    public ImageService(IAdsRepository<Ads>adsRepository)
    {
        _adsRepository = adsRepository;
    }
    public async Task<string> SaveImages(IFormFile file)
    {
        if (file is null)
        {
            return null;
        }

        string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string pathFolder = Path.Combine("wwwroot", "images");
        Directory.CreateDirectory(pathFolder);

        string Filepath = Path.Combine(pathFolder,FileName);

        using (var stream = new FileStream(Filepath,FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("images", FileName);
    }
    
    public async Task<string> UploadImages(IFormFile file,Guid adsId)
    {
        var ImageName = await SaveImages(file);
        var ads = await _adsRepository.GetById(adsId); 
        if (ads is null)
        {
            return null;
        }
        if (ads.Images == null)
        {
            ads.Images = new List<string>();
        }
        ads.Images.Add(ImageName);
        await _adsRepository.Update(ads); 
        return ImageName;
    }
    public Task<FileContentResult> GetImage(string fileName)
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        string imagePath = Path.Combine(folderPath, fileName);
        if (!System.IO.File.Exists(imagePath))
        {
            return Task.FromResult<FileContentResult>(null);
        }
        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
        string mimeType = "image/jpeg";
        return Task.FromResult(new FileContentResult(imageBytes, mimeType));
    }
    public async Task<FileContentResult> ResizeAndSaveImage(string fileName, int width, int height)
    {
        var originalImageResult = await GetImage(fileName);
        if (originalImageResult == null)
        {
            return null;
        }
        using (var stream = new MemoryStream(originalImageResult.FileContents))
        using (var image = Image.Load(stream))
        {
            image.Mutate(x => x.Resize(width, height));

            byte[] resizedImageBytes;
            using (var outputStream = new MemoryStream())
            {
                await image.SaveAsJpegAsync(outputStream);
                resizedImageBytes = outputStream.ToArray();
            }
            string mimeType = "image/jpeg";
            string pathFolder = Path.Combine("wwwroot", "images","resize");
            Directory.CreateDirectory(pathFolder);
            string resizedImagePath=Path.Combine(pathFolder, fileName);
            using (var outputStream = new FileStream(resizedImagePath, FileMode.Create))
            {
                await image.SaveAsJpegAsync(outputStream);
            }
            return new FileContentResult(resizedImageBytes, mimeType);
        }
    }
}