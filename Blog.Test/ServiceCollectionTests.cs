using Blog.Domain.Models;
using Blog.Helper.Security.Tokens;
using Blog.Installers;
using Blog.Persistence;
using Blog.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Blog.Test
{
    public  class ServiceCollectionTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public  ServiceCollectionTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void InstallServices_Execute_RegistersExpectedServices()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new Mock<IConfiguration>();
            var environment = new Mock<IWebHostEnvironment>();        

            var installer = new DataInstaller();         

            // Act
            installer.InstallServices(serviceCollection, configuration.Object, environment.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();

           
            // Assert
            Assert.Contains(serviceCollection, descriptor => descriptor.ServiceType == typeof(AppDbContext));
            Assert.Contains(serviceCollection, descriptor => descriptor.ServiceType == typeof(IUnitOfWork));
            Assert.Contains(serviceCollection, descriptor => descriptor.ServiceType == typeof(IIdentityService));
            Assert.Contains(serviceCollection, descriptor => descriptor.ServiceType == typeof(IAuthService));
            Assert.Contains(serviceCollection, descriptor => descriptor.ServiceType == typeof(ITokenHandler));
            Assert.Contains(serviceCollection, descriptor => descriptor.ServiceType == typeof(IHttpContextAccessor));
        }
        [Fact]
        public void InstallServices_TestConnString_Should_ReturnTrueForValidConnectionString()
        {
            var serviceCollection = new ServiceCollection();
         
            var environment = new Mock<IWebHostEnvironment>();

            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "DefaultConnection")]).Returns("mock value");
          
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings"))).Returns(mockConfSection.Object);

            _testOutputHelper.WriteLine(configuration.Object.GetConnectionString("DefaultConnection"));
            var installer = new DataInstaller();
            // Act
            installer.InstallServices(serviceCollection, configuration.Object, environment.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert
            var dbContext = serviceProvider.GetService<AppDbContext>();
            Assert.NotNull(dbContext);

        }
    }
}
