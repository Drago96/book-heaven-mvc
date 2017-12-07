using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookHeaven.Services.UtilityServices.Contracts
{
    public interface IFileService : IServce
    {
        Task<byte[]> GetByteArrayFromFormFileAsync(IFormFile file);
    }
}