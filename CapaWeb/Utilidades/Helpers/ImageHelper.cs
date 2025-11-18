
namespace CapaWeb.Utilidades.Helpers
{
    public class ImageHelper : IImageHelper
    {

        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            var guid = Guid.NewGuid().ToString();
            var fileName = $"{guid}{Path.GetExtension(imageFile.FileName)}";

            // wwwroot + folder
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                folder,
                fileName
            );

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/{folder}/{fileName}";
        }

        public string UploadImage(byte[] pictureArray, string folder)
        {
            var guid = Guid.NewGuid().ToString();
            var fileName = $"{guid}.jpg";

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                folder,
                fileName
            );

            File.WriteAllBytes(path, pictureArray);

            return $"/{folder}/{fileName}";
        }

        public Task DeleteImage(string imagePath)
        {
            if (imagePath.StartsWith('/'))
                imagePath = imagePath.Substring(1);

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                imagePath
            );

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
