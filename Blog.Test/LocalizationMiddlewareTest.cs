using Blog.Helper.Middlewares;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Test
{
    public class LocalizationMiddlewareTest
    {
        [Fact]
        public async Task InvokeAsync_Invoke_SetsExpectedResponseHeaders()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept-Language"] = "ar-SA";
            RequestDelegate next = (HttpContext httpContext) => Task.CompletedTask;
          
            var middleware = new LocalizationMiddleware();

            // Act
            await middleware.InvokeAsync(httpContext, next);

            // Assert
          
          //  Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
            Assert.Equal("ar-SA", Thread.CurrentThread.CurrentCulture.Name);
         


        }
       
    }

}