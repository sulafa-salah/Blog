using Blog.Contracts.ServiceModels;
using Blog.Domain.Models;
using Blog.Persistence;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net.Mail;

namespace Blog.Services
{
    public interface IEmailService
    {
        Task RegisterUserEmailAndLog(string email);
    }
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork unitOfWork;
      

        public EmailService(IUnitOfWork unitOfWork)
        {
           
            this.unitOfWork = unitOfWork;
        }
        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User Registeration Successful. <br/> Email : " + email;
            await LogAndEmail(message, email);
        }
        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
             
                await unitOfWork.EmailLogger.AddAsync(emailLog);
                await unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
     
    }
}
