using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CommandService.Services
{
    public interface ISourceService
    {
        public Task<Guid> CreateSource(IFormFile file, Guid userId);
    }
}