using Bgs.Live.Bll.Abstract;
using BgsLiveBackend.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BgsLiveBackend.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : BgsLiveController
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("addImage")]
        public async Task<IActionResult> AddImage([FromBody]AddImageModel model)
        {
            await _imageService.AddImage(model.Name, model.ImageUrl);
            return Ok();
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var objs = await _imageService.GetAll();
            return Ok(objs);
        }

        [HttpPost("deleteImage")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            await _imageService.DeleteImage(id);
            return Ok();
        }

        [HttpPost("setPrimaryImage")]
        public async Task<IActionResult> SetPrimary(int id)
        {
            await _imageService.SetPrimary(id);
            return Ok();
        }



    }
}
