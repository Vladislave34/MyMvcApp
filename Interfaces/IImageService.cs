using MyMvcApp.Data.Entities;

namespace MyMvcApp.Interfaces;

public interface IImageService
{
    public Task<string> UploadImageAsync(IFormFile file); 
    
    public Task<bool> DeleteImageAsync(string fileName);

    public Task<string> UpdateImageAsync(IFormFile newFile, string? oldFileName);
}