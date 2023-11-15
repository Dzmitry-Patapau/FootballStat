namespace FootballStats.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile,string pathImage);
    }
}
