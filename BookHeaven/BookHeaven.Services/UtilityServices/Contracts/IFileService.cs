using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookHeaven.Services.UtilityServices.Contracts
{
    public interface IFileService : IService
    {
        Task<byte[]> GetByteArrayFromFormFileAsync(IFormFile file);

        string UploadImage(byte[] image);

        void DeleteImage(string url);

        byte[] ResizeImage(byte[] image, int width, int height, string pictureType);
    }
}