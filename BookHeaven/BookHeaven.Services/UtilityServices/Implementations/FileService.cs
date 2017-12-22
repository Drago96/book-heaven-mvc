using BookHeaven.Services.UtilityServices.Contracts;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookHeaven.Services.UtilityServices.Implementations
{
    public class FileService : IFileService
    {
        private const string CloudinaryCloudName = "dtw4aopb2";
        private const string CloudinaryApiKey = "546522276337399";
        private const string CloudinaryApiSecret = "bavUWWiNbyMY8tdkjjEDpVJFgnU";

        private readonly Cloudinary cloudinary;

        public FileService()
        {
            this.cloudinary = new Cloudinary(new Account(
                CloudinaryCloudName,
                CloudinaryApiKey,
                CloudinaryApiSecret));
        }

        public async Task<string> UploadImageAndGetUrlAsync(IFormFile image, int width, int height)
        {
            var imageByteArray = await this.GetByteArrayFromFormFileAsync(image);
            var pictureType = image.ContentType.Split('/').Last();
            var resizedImage = this.ResizeImage(imageByteArray,
               width, height, pictureType);
            var nameGuid = Guid.NewGuid().ToString();

            using (MemoryStream stream = new MemoryStream(resizedImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(nameGuid, stream),
                    PublicId = nameGuid,
                };

                var uploadResult = this.cloudinary.Upload(uploadParams);
                return uploadResult.Uri.AbsoluteUri;
            }
        }

        public void DeleteImage(string url)
        {
            try
            {
                var imageId = url.Split('/').Last().Split('.').First();
                this.cloudinary.DeleteResources(imageId);
            }
            catch
            {
            }
        }

        private async Task<byte[]> GetByteArrayFromFormFileAsync(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                return stream.ToArray();
            }
        }

        private byte[] ResizeImage(byte[] image, int width, int height, string pictureType)
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