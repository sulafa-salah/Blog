using AutoMapper;
using Blog.Contracts.Blog;
using Blog.Contracts.Blog.Response;
using Blog.Domain.Models;
using static Azure.Core.HttpHeader;

namespace Blog.Helper.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Post, PostRequestDto>();
            CreateMap<PostRequestDto, Post>()
            .ForMember(p => p.Id, x => x.Ignore());
            CreateMap<Post, PostResponseDto>();
            CreateMap<PostRequestDto, Post>();  

            CreateMap<Category, CategoryResponseDto>();
            CreateMap<CategoryResponseDto, Category>();

        }
    }
}
