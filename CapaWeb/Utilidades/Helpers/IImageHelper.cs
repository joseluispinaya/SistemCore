namespace CapaWeb.Utilidades.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
        string UploadImage(byte[] pictureArray, string folder);
        Task DeleteImage(string imagePath);
        //Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
