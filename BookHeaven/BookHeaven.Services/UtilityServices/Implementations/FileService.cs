using BookHeaven.Services.UtilityServices.Contracts;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System.IO;
using System.Threading.Tasks;

namespace BookHeaven.Services.UtilityServices.Implementations
{
    public class FileService : IFileService

    {
        public async Task<byte[]> GetByteArrayFromFormFileAsync(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
        }

        public byte[] ResizeImage(byte[] image, int width, int height, string pictureType)
        {
            var currentImage = Image.Load(image);
            currentImage.Mutate(x => x.Resize(width, height));

            var memoryStream = new MemoryStream();
            this.SaveImageToStream(currentImage, memoryStream, pictureType);

            return memoryStream.ToArray();
        }

        private void SaveImageToStream(Image<Rgba32> image, MemoryStream memoryStream, string pictureType)
        {
            if (pictureType == "jpeg")
            {
                image.SaveAsJpeg(memoryStream);
            }

            if (pictureType == "gif")
            {
                image.SaveAsGif(memoryStream);
            }

            if (pictureType == "bmp")
            {
                image.SaveAsBmp(memoryStream);
            }

            image.SaveAsPng(memoryStream);
        }
    }
}