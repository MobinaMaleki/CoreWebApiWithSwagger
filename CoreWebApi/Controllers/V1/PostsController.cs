 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Contacts;
using CoreWebApi.Contacts.Request;
using CoreWebApi.Contacts.V1.Requests;
using CoreWebApi.Contacts.V1.Responses;
using CoreWebApi.Domain;
using CoreWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace CoreWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IPostServise _postservise;

        public PostsController(IPostServise postServise)
        {
            _postservise = postServise;
        }
        [HttpGet(ApiRoutes.posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postservise.GetPostsAsync());
        }

        [HttpPut(ApiRoutes.posts.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid postId,[FromBody] UpdatePostRequest request)
        {
            var post = new Post
            {
                Id = postId,
                Name = request.Name
            };
            var updated = await _postservise.UpdatePostAsync(post);
            if (updated)
            {
                return Ok(post);
            }
            else
            {
                return NotFound();
            }
            
           
        }
        [HttpDelete(ApiRoutes.posts.Delete)]        
        public async Task<IActionResult> Delete([FromRoute]Guid postId)
        {
            var deleted =await _postservise.DeletePostAsync(postId);
            if (deleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet(ApiRoutes.posts.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid postId)
        {
            Post post = await _postservise.GetPostByIdAsync(postId);
            if (post==null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost(ApiRoutes.posts.Create)]
        public async Task<IActionResult> Create([FromBody]CreatePostRequest postRequest)
        {
            var post = new Post() { Name = postRequest.Name};
            
              await _postservise.CreatePostAsync(post);
            
            var baseUrl = $"{ HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.posts.Get.Replace("{postId}",post.Id.ToString());
            var response = new PostResponse() { Id = post.Id };
            return Created(locationUrl, response);
        }
    }
}
