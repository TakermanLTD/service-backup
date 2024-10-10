using Takerman.Backup.Models.DTOs;

namespace Takerman.Backup.Services.Abstraction
{
    public interface ITemplateService
    {
        Task<TemplateEntityDto> Get(int id);
    }
}