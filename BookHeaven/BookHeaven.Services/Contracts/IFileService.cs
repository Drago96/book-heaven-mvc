using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookHeaven.Services.Contracts
{
    public interface IFileService : IServce
    {
        Task<byte[]> GetByteArrayFromFormFileAsync(IFormFile file);
    }
}