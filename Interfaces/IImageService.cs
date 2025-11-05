using MyMvcApp.Data.Entities;

namespace MyMvcApp.Interfaces;

public interface IImageService
{
    public Task<string> UploadImageAsync(IFormFile file); 
}