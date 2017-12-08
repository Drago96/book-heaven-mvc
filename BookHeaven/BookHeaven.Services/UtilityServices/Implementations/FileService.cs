using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using BookHeaven.Services.UtilityServices.Contracts;

using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Helpers;


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


        public byte[] ResizeImageAsync(byte[] image, int width, int height, string pictureType)
        {
            var currentImage = Image.Load(image);
            currentImage.Mutate(x => x.Resize(width, height));

            var memoryStream = new MemoryStream();
            this.SaveImageToStream(currentImage, memoryStream, pictureType);

            return memoryStream.ToArray();
        }

        private void SaveImageToStream(Image<Rgba32> image,MemoryStream memoryStream,string pictureType)
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
