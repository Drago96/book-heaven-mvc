using System.IO;
using System.Threading.Tasks;
using BookHeaven.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace BookHeaven.Services.Implementations
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
    }
}
