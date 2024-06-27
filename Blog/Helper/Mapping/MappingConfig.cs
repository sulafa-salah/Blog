using AutoMapper;
using Blog.Contracts.Blog;
using Blog.Contracts.Blog.Response;
using Blog.Contracts.Identity.Request;
using Blog.Domain.Models;
using static Azure.Core.HttpHeader;

namespace Blog.Helper.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // post request 
            CreateMap<Post, PostRequestDto>();
            CreateMap<PostRequestDto, Post>()
            .ForMember(p => p.Id, x => x.Ignore());
            // post response 
            CreateMap<Post, PostResponseDto>();
            CreateMap<PostRequestDto, Post>();  
            // category 
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<CategoryResponseDto, Category>();

            // user
            CreateMap<UpdateAccountRequestDto, ApplicationUser>();
            CreateMap<ApplicationUser,UpdateAccountRequestDto > ();


        }
    }
}
