using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookHeaven.Services.UtilityServices.Contracts
{
    public interface IFileService : IServce
    {
        Task<byte[]> GetByteArrayFromFormFileAsync(IFormFile file);

        byte[] ResizeImageAsync(byte[] image, int width, int height, string pictureType);
    }
}