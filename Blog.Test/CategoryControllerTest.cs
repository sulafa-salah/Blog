using AutoMapper;
using Azure;
using Blog.Contracts.Blog;
using Blog.Contracts.Common.Response;
using Blog.Controllers;
using Blog.Domain.Models;
using Blog.Helper.Mapping;
using Blog.Persistence;
using Blog.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test
{
    public class CategoryControllerTest
    {
     

        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork.Setup(u => u.Category).Returns(_mockCategoryRepository.Object);

            var mapperConfiguration = new MapperConfiguration(
             cfg => cfg.AddProfile<Helper.Mapping.MappingConfig>());
            var mapper = new Mapper(mapperConfiguration);
            _controller = new CategoryController(_mockUnitOfWork.Object,mapper);
        }

        [Fact]
       // [Trait("Category", "Category_GetById")]
        public async Task CategoryGetById_CategoryExists_ReturnsOkWithCategoryData()
        {
            // Arrange
            int categoryId = 2;
            var category = new Category { Id = 1, Name = "Category" };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(category);
            // Act
            var result = await _controller.GetById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(response.IsSuccess);
          
            
        }
        [Fact]
        //[Trait("Category", "Category_GetById")]
        public async Task CategoryGetById_CategoryDoesNotExist_ReturnsOkWithErrorMessage()
        {
            // Arrange
            int categoryId = 1;

            _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null);

            // Act
            var result = await _controller.GetById(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal("Category doesn't exists", response.ResponseMessage);
        }

        // [Fact(Skip = "Skipping this one for demo reasons.")]
        [Fact]
        public async Task CategoryGetById_CategoryExceptionThrown_ReturnsOkWithErrorMessage()
        {
           // Arrange
                int categoryId = 1;

                _mockCategoryRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                    .ThrowsAsync(new Exception("Internal System Error"));

                // Act
                var result = await _controller.GetById(categoryId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsType<ResponseDto>(okResult.Value);
                Assert.False(response.IsSuccess);
                Assert.Equal("Internal System Error", response.ResponseMessage);
            
        }


    }
}
