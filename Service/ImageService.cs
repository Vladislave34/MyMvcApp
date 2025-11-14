using MyMvcApp.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace MyMvcApp.Service;

public class ImageService(IConfiguration configuration) : IImageService
{
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        try
        {
            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            string filename = Path.GetRandomFileName() + ".webp";
            var bytes = memoryStream.ToArray();
            using var image = Image.Load(bytes);
            image.Mutate(imgc =>
            {
                imgc.Resize(new ResizeOptions()
                {
                    Size = new Size(600, 600),
                    Mode = ResizeMode.Max
                });
            });
            var dirImageName = configuration["DirImagePath"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), dirImageName, filename);
            await image.SaveAsync(path, new WebpEncoder());
            return filename;
        }
        catch 
        {
            return string.Empty;
        }
    }
    public async Task<bool> DeleteImageAsync(string fileName)
    {
        try
        {
            var dirImageName = configuration["DirImagePath"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), dirImageName, fileName);

            if (File.Exists(path))
            {
                await Task.Run(() => File.Delete(path));
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
    public async Task<string> UpdateImageAsync(IFormFile newFile, string? oldFileName)
    {
        
        if (newFile == null || newFile.Length == 0)
            return oldFileName ?? string.Empty;

       
        string newFileName = await UploadImageAsync(newFile);

       
        if (string.IsNullOrEmpty(newFileName))
            return oldFileName ?? string.Empty;

        
        if (!string.IsNullOrEmpty(oldFileName))
            await DeleteImageAsync(oldFileName);

        return newFileName;
    }
}