using AutoMapper;
using Dotz.Common.Api;
using Dotz.Common.Api.Base;
using Dotz.Data.Entities;
using Dotz.Dto;
using Dotz.WebApi.Interfaces;
using Dotz.WebApi.VM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotz.WebApi.Controllers
{                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
    [ApiController]
    [Route("User")]
    //[Authorize(Roles="Admin")]
    public class UserController : ApiBase<ApplicationUser, ApplicationUserDto, UserController>, IUserController
    {
        #region Variables

        private readonly UserManager<ApplicationUser> _userManager;

        #endregion Variables

        #region Contructor

        public UserController(IServiceProvider service) : base(service)
        {
            _userManager = service.GetService<UserManager<ApplicationUser>>();
        }

        #endregion Contructor

        public override async Task<ApiResult<ApplicationUserDto>> AddAsync(ApplicationUserDto item)
        {
            return await AddUser(item);
        }

        [AllowAnonymous]
        public override ApiResult<ApplicationUserDto> Add([FromBody] ApplicationUserDto item)
        {
            return AddUser(item).Result;
        }

        private async Task<ApiResult<ApplicationUserDto>> AddUser(ApplicationUserDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = Mapper.Map<ApplicationUserDto, ApplicationUser>(item);
                user.CreateTimestamp = DateTime.UtcNow;
                identityResult = await _userManager.CreateAsync(user, password: user.PasswordHash).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<ApplicationUserDto>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Added Successfully." : sbErrors.ToString()),
                    Body = (identityResult.Succeeded ? Mapper.Map<ApplicationUser, ApplicationUserDto>(GetQueryable().FirstOrDefault(x => x.Id == user.Id)) : null)
                };

                _logger.LogInformation($"Add User with userid:{user.Id } mail:{item.Email} username:{item.UserName} result:{result.Message}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"User Add : mail:{item.Email} username:{item.UserName} result:Error - {ex.Message}");

                return new ApiResult<ApplicationUserDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        public override async Task<ApiResult<ApplicationUserDto>> UpdateAsync([FromBody] ApplicationUserDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString()).ConfigureAwait(false);

                _logger.LogInformation($"Update User : userid:{user.Id} oldusername:{item.UserName} oldphonenumber:{item.PhoneNumber} oldtitle:{user.Title}");

                user.UserName = item.UserName;
                user.PhoneNumber = item.PhoneNumber;
                user.Title = item.Title;
                user.Email = item.Email;

                identityResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<ApplicationUserDto>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Update User Success" : sbErrors.ToString()),
                    Body = Mapper.Map<ApplicationUser, ApplicationUserDto>(GetQueryable().Include(x => x.UserRoles).FirstOrDefault(x => x.Id == item.Id))
                };

                _logger.LogInformation($"Update User : userid:{user.Id} newusername:{item.UserName} newphonenumber:{item.PhoneNumber} newtitle:{user.Title} result :{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Update User : newusername:{item.UserName} newphonenumber:{item.PhoneNumber} newtitle:{item.Title} result:{ex.Message}");
                return new ApiResult<ApplicationUserDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = null
                };
            }
        }

        public override ApiResult<ApplicationUserDto> Update([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<ApplicationUserDto>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to Update a user",
                Body = null
            };
        }

        public override ApiResult<string> DeleteById(Guid id)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Body = null
            };
        }

        public override async Task<ApiResult<string>> DeleteByIdAsync(Guid id)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
                identityResult = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<string>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Deleted Successfully." : sbErrors.ToString()),
                    Body = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User deleted. userid:{id} email:{user.Email} username:{user.UserName}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Delete user error. userid:{id} error:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = sbErrors.ToString()
                };
            }
        }

        public override ApiResult<string> Delete([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Body = null
            };
        }

        public override Task<ApiResult<string>> DeleteAsync([FromBody] ApplicationUserDto item)
        {
            return DeleteByIdAsync(item.Id);
        }

        [HttpPost("ChangePasswordAsAdminAsync")]
        public async Task<ApiResult> ChangePasswordAsAdminAsync([FromBody] ChangePasswordModel model)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString()).ConfigureAwait(false);

                var validator = new PasswordValidator<ApplicationUser>();

                var validatorResult = await validator.ValidateAsync(_userManager, user, model.NewPassword).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", validatorResult.Errors.Select(x => x.Code).ToList()));

                if (validatorResult.Succeeded)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                    identityResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Change Password Success" : sbErrors.ToString()),
                    Body = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"Change Password : userid:{user.Id } mail:{user.Email} username:{user.UserName} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Change Password : id:{model.UserId} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = sbErrors.ToString()
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("ChangePasswordAsync")]
        public async Task<ApiResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString()).ConfigureAwait(false);

                var oldPasswordCheck = await _userManager.CheckPasswordAsync(user, model.OldPassword).ConfigureAwait(false);

                if (oldPasswordCheck)
                {
                    var validatorResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", validatorResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("Old Password Invalid");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Change Password Success" : sbErrors.ToString()),
                    Body = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"Change Password : userid:{user.Id } mail:{user.Email} username:{user.UserName} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Change Password : UserId:{model.UserId} Error:{ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = sbErrors.ToString()
                };
            }
        }

        [HttpPost("AddUserRoleAsync")]
        public async Task<ApiResult> AddUserRoleAsync(Guid userid, Guid roleid)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            var roleManager = _service.GetService<RoleManager<ApplicationRole>>();
            try
            {
                var user = await _userManager.FindByIdAsync(userid.ToString()).ConfigureAwait(false);
                var role = await roleManager.FindByIdAsync(roleid.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    identityResult = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("No Such Role!");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Role Added Successfully." : sbErrors.ToString()),
                    Body = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User Role Add to userid:{userid} role:{roleid} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User Role Add to userid:{userid} role:{roleid} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = sbErrors.ToString()
                };
            }
        }

        [HttpPost("DeleteUserRoleAsync")]
        public async Task<ApiResult> DeleteUserRoleAsync(Guid userid, Guid roleid)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            var roleManager = _service.GetService<RoleManager<ApplicationRole>>();
            try
            {
                var user = await _userManager.FindByIdAsync(userid.ToString()).ConfigureAwait(false);
                var role = await roleManager.FindByIdAsync(roleid.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("No Such Role!");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Role Deleted Successfully." : sbErrors.ToString()),
                    Body = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User Role Delete to userid:{userid} role:{roleid} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User Role Delete to userid:{userid} role:{roleid} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Body = sbErrors.ToString()
                };
            }
        }
    }
}