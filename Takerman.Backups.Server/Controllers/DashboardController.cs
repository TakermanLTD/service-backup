using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController(ILogger<DashboardController> _logger, IDashboardService _dashboardService) : ControllerBase
    {
        [HttpGet("Get")]
        public async Task<DashboardDto> Get()
        {
            return await _dashboardService.GetDashboard();
        }

        [HttpPost("Execute")]
        public IActionResult ExecuteCommand([FromBody] CommandRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Command))
            {
                return BadRequest("Invalid command.");
            }

            try
            {
                string output = ExecuteShellCommand(request.Command);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error executing command: {ex.Message}");
            }
        }

        private string ExecuteShellCommand(string command)
        {
            var process = new Process();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                process.StartInfo.FileName = "bash";
            }
            else
            {
                process.StartInfo.FileName = "/bin/sh";
            }

            process.StartInfo.Arguments = $"-c \"{command}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                string error = process.StandardError.ReadToEnd();
                throw new Exception($"Command failed with exit code {process.ExitCode}: {error}");
            }

            return result;
        }
    }

    public class CommandRequest
    {
        public string Command { get; set; }
    }
}