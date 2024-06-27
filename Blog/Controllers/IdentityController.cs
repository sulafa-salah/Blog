using Blog.Contracts.Identity.Request;
using Blog.Contracts;
using Blog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Blog.Contracts.Common.Response;
using Blog.Domain.Models;
using System.Data;
using System.Net;
using Blog.Helper.Extensions;

namespace Blog.Controllers
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }    
        [HttpPost(ApiRoute.Identity.UpdateAccount)]
        public async Task<IActionResult> UpdateAccountRequest( [FromBody] UpdateAccountRequestDto req)
        {
            string currentUser = HttpContext.GetUserId();
            return Ok(await identityService.UpdateAccount(req,currentUser));
        }

       
        [HttpPost(ApiRoute.Identity.ChangePassword)]
        public async Task<IActionResult> ChangePasswordRequest([FromBody] ChangePasswordRequestDto request)
        {

            var response = await identityService.ChangePassword(request);
            
            return Ok(response);
        }

       
        [HttpPost(ApiRoute.Identity.AddUserToRoles)]
      
        public async Task<IActionResult> addUserToRolesRequest([FromBody] UserRolesRequestDto request)
        {
            var currentUserId = HttpContext.GetUserId();
            var response = await identityService.AddUserToRolesAsync(request, currentUserId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
     
      
        [HttpPost(ApiRoute.Identity.RemoveUserToRoles)]
       
        public async Task<IActionResult> RemoveUserToRolesRequest([FromBody] UserRolesRequestDto request)
        {
            var currentUserId = HttpContext.GetUserId();
            var response = await identityService.RemoveUserToRolesAsync(request, currentUserId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


    }
}
