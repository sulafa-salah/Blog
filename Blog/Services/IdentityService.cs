using AutoMapper;
using Blog.Contracts;
using Blog.Contracts.Blog;
using Blog.Contracts.Common.Response;
using Blog.Contracts.Identity.Request;
using Blog.Domain.Models;
using Blog.Helper.Security.Tokens;
using Blog.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.Services
{
    public interface IIdentityService
    {

        Task<ResponseDto> UpdateAccount( UpdateAccountRequestDto req, string currentUser);
        Task<ResponseDto> ChangePassword(ChangePasswordRequestDto request);
        Task<ResponseDto> AddUserToRolesAsync(UserRolesRequestDto request, string currentUserId);
        Task<ResponseDto> RemoveUserToRolesAsync(UserRolesRequestDto request, string currentUserId);


    }
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStringLocalizer<IdentityService> localizer;
        private readonly IMapper mapper;
        private readonly RoleManager<ApplicationRole> roleManger;
        private ResponseDto _response;

        public IdentityService(UserManager<ApplicationUser> userManager,IStringLocalizer<IdentityService> localizer,IMapper mapper,RoleManager<ApplicationRole> roleManger)
        {
            this.userManager = userManager;
            this.localizer = localizer;
            this.mapper = mapper;
            this.roleManger = roleManger;
            _response = new ResponseDto();
        }


        public async Task<ResponseDto> UpdateAccount(UpdateAccountRequestDto req, string currentUser)
        {
            // Get the user
           
            try
            {
                var user = await userManager.FindByIdAsync(req.Id.ToString());
                if (user == null)
                {
                    _response.ResponseMessage = "User doesn't exist";
                    return _response;
                }
                mapper.Map<UpdateAccountRequestDto, ApplicationUser>(req, user);
                user.UpdatedBy = new Guid(currentUser);
                user.UpdatedDate=DateTime.Now;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _response.IsSuccess = true;
                    _response.ResponseMessage = "The data has been updated successfully";
                    _response.Data = new { Id=user.Id };
                    return _response;
                }
                _response.ResponseMessage = result.Errors.FirstOrDefault().Description;
                return _response;
            }
            catch (Exception ex)
            {        
                _response.ResponseMessage = "INTERNAL SYSTEM ERROR";
                return _response;
            }
        }

        public async Task<ResponseDto> ChangePassword(ChangePasswordRequestDto request)
        {
            try
            {
              
                // Find user by Id
                var user = await userManager.FindByIdAsync(request.Id);
                if (user == null)
                {
                    _response.ResponseMessage = "User doesn't exist";
                    return _response;
                }
                // Validate the new password
                var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, request.CurrentPassword);
                if (!isCurrentPasswordValid)
                {
                    _response.ResponseMessage = "Invalid password";
                    return  _response;
                }
                // Change user password
                var changeResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
               
               
                if (changeResult.Succeeded)
                {
                    _response.ResponseMessage = "Password has been changed successfully";
                    _response.IsSuccess = true;
                    return _response;
                }
                else
                {
                    _response.ResponseMessage = changeResult.Errors.FirstOrDefault().Description;
                    return _response;
                }
            }
            catch (Exception ex)
            {

                _response.ResponseMessage = "INTERNAL SYSTEM ERROR";
                return _response;
            }

        }

        public async Task<ResponseDto> AddUserToRolesAsync(UserRolesRequestDto request, string currentUserId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                  
                    return new ResponseDto(false, ApiStatusCode.BadRequest, localizer["IdentityErrorInvalidUserId"], null);
                }
                var roleNames = request.Roles.Select(x => x.RoleName);
                foreach (var roleName in roleNames)
                {
                    var role = await roleManger.FindByNameAsync(roleName);
                    if (role == null)
                    {
                      
                        return new ResponseDto(false, ApiStatusCode.BadRequest, localizer["IdentityErrorInvalidRoleName"], null);
                    }
                    if (await userManager.IsInRoleAsync(user, roleName))
                    {
                        return new ResponseDto(false, ApiStatusCode.BadRequest, localizer["IdentityErrorRoleNameAlreadyAddToUser"], null);
                    }
                  

                }

                var result = await userManager.AddToRolesAsync(user, roleNames);

                return new ResponseDto(true, ApiStatusCode.SUCCESS, localizer["SUCCESS"], null);
            }
            catch (Exception ex)
            {

                _response.ResponseMessage = "INTERNAL SYSTEM ERROR";
                return _response;
            }

           
        }
        public async Task<ResponseDto> RemoveUserToRolesAsync(UserRolesRequestDto request, string currentUserId)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new ResponseDto(false, ApiStatusCode.BadRequest, localizer["IdentityErrorInvalidUserId"], null);
            }
            var roleNames = request.Roles.Select(x => x.RoleName);
            foreach (var roleName in roleNames)
            {
                var role = await roleManger.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new ResponseDto(false, ApiStatusCode.BadRequest, localizer["IdentityErrorInvalidRoleName"], null);
                }
                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    return new ResponseDto(false, ApiStatusCode.BadRequest, localizer["IdentityErrorRoleNameDoesn'tNotAddToUser"], null);
                }
            }

            var result = await userManager.RemoveFromRolesAsync(user, roleNames);
            return new ResponseDto(true, ApiStatusCode.SUCCESS, localizer["SUCCESS"], null);
        }
    }
}
