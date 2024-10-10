using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Takerman.Backup.Data;
using Takerman.Backup.Models.DTOs;

namespace Takerman.Backup.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemplateController(ILogger<TemplateController> _logger) : ControllerBase
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplateEntityDto, TemplateEntity>();
                cfg.CreateMap<TemplateEntity, TemplateEntityDto>();
            }).CreateMapper();

        [HttpPost("Index")]
        public async Task<IActionResult> Index(TemplateEntityDto model)
        {
            var entity = _mapper.Map<TemplateEntityDto>(model);
            return Ok(entity);
        }
    }
}