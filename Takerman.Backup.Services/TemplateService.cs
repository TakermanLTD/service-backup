using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Takerman.Backup.Data;
using Takerman.Backup.Models.DTOs;
using Takerman.Backup.Services.Abstraction;

namespace Takerman.Backup.Services
{
    public class TemplateService(DefaultContext _context, ILogger<TemplateService> _logger) : ITemplateService
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TemplateEntityDto, TemplateEntity>();
            cfg.CreateMap<TemplateEntity, TemplateEntityDto>();
        }).CreateMapper();

        public async Task<TemplateEntityDto> Get(int id)
        {
            var result = await _context.Templates.FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<TemplateEntityDto>(result);
        }
    }
}