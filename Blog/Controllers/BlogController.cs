using Blog.Contracts.Identity.Request;
using Blog.Contracts;
using Blog.Persistence;
using Blog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure;
using AutoMapper;
using Blog.Contracts.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Blog.Domain.Models;
using Blog.Domain.Enums;
using Blog.Contracts.Blog;
using Blog.Contracts.Blog.Response;

namespace Blog.Controllers
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize]
    public class BlogController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private ResponseDto _response;
        const int maxCitiesPageSize = 10;

        public BlogController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet(ApiRoute.Blog.GetAll)]
        public async Task<IActionResult> GetAll(int? CategoryId,int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageSize > maxCitiesPageSize)
                {
                    pageSize = maxCitiesPageSize;
                }
                var postList =await unitOfWork.Post.GetPostsAsync(CategoryId,pageNumber,pageSize);
                _response.Data = mapper.Map<IEnumerable<PostRequestDto>>(postList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }
   
        [HttpGet(ApiRoute.Blog.GetById + "/{id:Guid}")]
        public async Task<IActionResult> GetById( Guid id)
        {
            try
            {
                Post post = await unitOfWork.Post.GetAsync(u => u.Id == id);
                _response.IsSuccess = true;
                _response.Data = mapper.Map<PostResponseDto>(post);
            }
            catch (Exception ex)
            {
               
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }
        [HttpPost(ApiRoute.Blog.Create)]
        [Authorize(Roles = nameof(ApplicationRoleEnums.Administrator))]
        public async Task<IActionResult> Create(PostRequestDto request )
        {
            try
            {
                Post post = mapper.Map<Post>(request);
              
               var categoryExist=await unitOfWork.Category.GetAsync(c => c.Id == Convert.ToInt32(request.CategoryId));
                if (request.Image != null)
                {

                    string fileName = post.Id + Path.GetExtension(request.Image.FileName);
                    string filePath = @"wwwroot\PostImages\" + fileName;

                  //  I have added the if condition to remove the any image with same name if that exist in the folder by any change
                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    FileInfo file = new FileInfo(directoryLocation);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        request.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    post.ImageUrl = baseUrl + "/PostImages/" + fileName;
                    post.ImageLocalPath = filePath;
                }

                await unitOfWork.Post.AddAsync(post);
                await unitOfWork.Commit();
                _response.IsSuccess = true;
                _response.Data = mapper.Map<PostResponseDto>(post);
            }
            catch (Exception ex)
            {
               
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }

        [HttpPut(ApiRoute.Blog.Update)]
        [Authorize(Roles = nameof(ApplicationRoleEnums.Administrator))]

        public async Task<IActionResult> Update(PostRequestDto request)
        {
            try
            {
                Post post = mapper.Map<Post>(request);

                if (request.Image != null)
                {
                    if (!string.IsNullOrEmpty(post.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), post.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = post.Id + Path.GetExtension(request.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        request.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    post.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    post.ImageLocalPath = filePath;
                }


                unitOfWork.Post.UpdateAsync(post);
                await unitOfWork.Commit();
                _response.IsSuccess = true;
                _response.Data = mapper.Map<PostResponseDto>(post);
            }
            catch (Exception ex)
            {
               
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }
        [HttpDelete(ApiRoute.Blog.Delete)]
       
        [Authorize(Roles = nameof(ApplicationRoleEnums.Administrator))]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            try
            {
                Post post = await unitOfWork.Post.GetAsync(u => u.Id == id);
                if (!string.IsNullOrEmpty(post.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), post.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                post.IsDeleted= true;
                post.DeletedDate = DateTime.Now;
               // post.DeletedBy=
                unitOfWork.Post.UpdateAsync(post);
                await unitOfWork.Commit();
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
              
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }
    
    
    }
}
