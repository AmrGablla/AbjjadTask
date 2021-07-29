using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepAPI.DTOs;
using WepAPI.Interfaces;
using WepAPI.Models;
using WepAPI.Services;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IFileManagerService _fileManagerService;

        public PostsController(IPostService postService,
            IAuthenticatedUserService authenticatedUserService,
            IFileManagerService fileManagerService)
        {
            _postService = postService;
            _authenticatedUserService = authenticatedUserService;
            _fileManagerService = fileManagerService;
        }

        [HttpGet]
        public ActionResult<List<Post>> Get() =>
            _postService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPost")]
        public ActionResult<Post> Get(string id)
        {
            var post = _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpPost]
        public async Task<ActionResult<Post>> Create([FromBody] PostCreateDTO postDto)
        {
            postDto.UserId = _authenticatedUserService.UserId;
            if (postDto.Image != null)
            {
                await _fileManagerService.Upload(postDto.Image);
            }

            Post post = _postService.Create(postDto);

            return CreatedAtRoute("GetPost", new { id = post.Id.ToString() }, post);
        }


        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var post = _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            _postService.Remove(post.Id);

            return NoContent();
        }
    }
}
