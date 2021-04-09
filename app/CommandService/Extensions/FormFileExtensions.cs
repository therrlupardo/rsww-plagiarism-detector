using System.IO;
using Commands;
using Microsoft.AspNetCore.Http;

namespace CommandService.Extensions
{
    public static class FormFileExtensions
    {
        public static FileModel ToFileModel(this IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            return new FileModel(file.FileName, ms.ToArray());
        }
    }
}