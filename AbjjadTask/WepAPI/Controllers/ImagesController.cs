using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WepAPI.DTOs;
using WepAPI.Interfaces;

namespace WepAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IFileManagerService _fileManagerService;

        public ImagesController(IFileManagerService fileManagerService)
        {
            _fileManagerService = fileManagerService;
        } 
         
        [HttpGet]
        public async Task<IActionResult> Get(string fileName)
        {
            var imgBytes = await _fileManagerService.Get(fileName);
            return File(imgBytes, "image/webp");
        }
    }
}
