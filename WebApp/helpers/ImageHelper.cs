using System.IO;
using ImageMagick;
using Microsoft.AspNetCore.Http;

namespace WebApp.helpers
{
    public class ImageHelper
    {
        public static string ProcessUploadedFile(IFormFile picture)
        {
            string uniqueFileName = "data:image/jpeg;base64,";

            using (var ms = new MemoryStream())
            {
                picture.CopyTo(ms);
                var fileBytes = ms.ToArray();
                using (MagickImage image = new MagickImage(fileBytes))
                {
                    image.Format = image.Format; // Get or Set the format of the image.
                    image.Resize(300, 300); // fit the image into the requested width and height. 
                    image.Quality = 10; // This is the Compression level.
                    uniqueFileName+= image.ToBase64(MagickFormat.Jpg);
                }
                // += Convert.ToBase64String(fileBytes);
                // act on the Base64 data
            }
            return uniqueFileName;
        }
    }
}