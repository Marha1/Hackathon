using Application.Services.Interfaces;
using Domain.Enities;
using Infrastrucure.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Implementations;

public class ImageService: IImageService
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
        var ads = _adsRepository.GetById(adsId);
        if (ads is null)
        {
            return null;
        }

        if (ads.Images == null)
        {
            ads.Images = new List<string>();
        }
        ads.Images.Add(ImageName);
        _adsRepository.Update(ads);
         return ImageName;
    }
}