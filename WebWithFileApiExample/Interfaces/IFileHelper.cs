using Microsoft.AspNetCore.Mvc;

namespace WebWithFileApiExample.Interfaces
{
    public interface IFileHelper
    {
        Task<Guid> WriteFile(IFormFile formFile);
        Task<Stream> GetFileById(Guid id);
    }
}
