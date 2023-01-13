using BlogWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        public ImagesController(IImageRepository imageRepository)
        {
            ImageRepository = imageRepository;
        }

        public IImageRepository ImageRepository { get; }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        { 
           var imageUrl = await ImageRepository.UploadAsync(file);
            if (imageUrl == null)
            {
                return Problem("Something wemt rong!", null, (int)HttpStatusCode.InternalServerError);
            }
            return Json(new { link = imageUrl });
        
        }
    }
}
