using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookHeaven.Services.UtilityServices.Contracts
{
    public interface IFileService : IService
    {
        Task<string> UploadImageAndGetUrlAsync(IFormFile image, int width, int height);

        void DeleteImage(string url);
    }
}