using AutoMapper;
using Blog.Contracts;
using Blog.Contracts.Common.Response;
using Blog.Domain.Models;
using Blog.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Blog.Contracts.Blog.Response;

namespace Blog.Controllers
{
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private ResponseDto _response;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _response = new ResponseDto();
        }
        [HttpGet(ApiRoute.Category.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await unitOfWork.Category.GetAllAsync();
                _response.IsSuccess = true;
                _response.Data = mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            }
            catch (Exception ex)
            {
              
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }
        [HttpGet(ApiRoute.Category.GetById + "/{id:int}")]
       
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Category category = await unitOfWork.Category.GetAsync(u => u.Id == id);
                if(category == null)
                {
                  
                    _response.ResponseMessage = "Category doesn't exists";
                    return Ok(_response);
                }
                _response.IsSuccess = true;
                _response.Data = new { Id = category.Id, Name = category.Name };
            }
            catch (Exception ex)
            {
               
                _response.ResponseMessage ="Internal System Error";
            }
            return Ok(_response);
        }
        [HttpPost(ApiRoute.Category.Create)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CategoryResponseDto request)
        {
            try
            {
                Category categoryExists = await unitOfWork.Category.GetAsync(u => u.Name == request.Name);
                if(categoryExists != null)
                {
                    _response.IsSuccess = false;
                    _response.ResponseMessage = "Category already exists";
                }
                Category category =new Category { Name = request.Name };
                await unitOfWork.Category.AddAsync(category);
                await unitOfWork.Commit();
                _response.IsSuccess = true;
                _response.Data = new { Id=category.Id,Name=category.Name };
            }
            catch (Exception ex)
            {
               
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }

        [HttpPut(ApiRoute.Category.Update)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(CategoryResponseDto request)
        {
            try
            {
                Category category = await unitOfWork.Category.GetAsync(u => u.Id == request.Id);
                if (category == null)
                {

                    _response.ResponseMessage = "Category doesn't exists";
                    return Ok(_response);
                }
               category.Name = request.Name;

                unitOfWork.Category.UpdateAsync(category);
                await unitOfWork.Commit();
                _response.IsSuccess = true;
                _response.Data = mapper.Map<CategoryResponseDto>(category);
            }
            catch (Exception ex)
            {
               
                _response.ResponseMessage = ex.Message;
            }
            return Ok(_response);
        }
       


    }
}
