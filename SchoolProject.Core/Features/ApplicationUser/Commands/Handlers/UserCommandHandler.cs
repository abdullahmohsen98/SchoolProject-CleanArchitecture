using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
                                      IRequestHandler<AddUserCommand, Response<string>>,
                                      IRequestHandler<EditUserCommand, Response<string>>,
                                      IRequestHandler<DeleteUserCommand, Response<string>>,
                                      IRequestHandler<ChangeUserPasswordCommand, Response<string>>

    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserCommandHandler> _logger;
        #endregion

        #region Constructors
        public UserCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IMapper mapper,
                                  UserManager<User> userManager,
                                  ILogger<UserCommandHandler> logger) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling AddUserCommand");

            //if (request.Password != request.ConfirmPassword)
            //{
            //    _logger.LogWarning("Password and ConfirmPassword do not match");
            //    return BadRequest<string>(_stringLocalizer[SharedResourceKeys.PasswordNotEqualConfirmPassword]);
            //}

            // Check email existence
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                _logger.LogWarning("Email already exists: {Email}", request.Email);
                return BadRequest<string>(_stringLocalizer[SharedResourceKeys.EmailIsExist]);
            }

            // Check username existence
            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName != null)
            {
                _logger.LogWarning("Username already exists: {UserName}", request.UserName);
                return BadRequest<string>(_stringLocalizer[SharedResourceKeys.UserNameIsExist]);
            }

            // Map AddUserCommand(request) obj to appropriate model(User)
            var identityUser = _mapper.Map<User>(request);

            var createResult = await _userManager.CreateAsync(identityUser, request.Password);

            // Check Conditions and Return appropriate response
            if (!createResult.Succeeded)
            {
                _logger.LogError("Failed to add user: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                //return BadRequest<string>(_stringLocalizer[SharedResourceKeys.FailedToAddUser]);
                return BadRequest<string>(string.Join(",", createResult.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(identityUser, "User");
            _logger.LogInformation("User added successfully: {UserName}", request.UserName);
            return Success("User Added Successfully");
        }
        public async Task<Response<string>> Handle(EditUserCommand command, CancellationToken cancellationToken)
        {
            // Check if user exist
            var oldUser = await _userManager.FindByIdAsync(command.Id.ToString());
            if (oldUser == null)
                return NotFound<string>();

            //Mapping
            var newUser = _mapper.Map(command, oldUser);

            //check if username is exist
            var userByUserName = _userManager.Users.FirstOrDefaultAsync(u => u.UserName == newUser.UserName && u.Id != newUser.Id);
            if (userByUserName != null) return BadRequest<string>(_stringLocalizer[SharedResourceKeys.UserNameIsExist]);

            //Update
            var result = await _userManager.UpdateAsync(newUser);

            //check if result isnot success
            if (!result.Succeeded) return BadRequest<string>(_stringLocalizer[SharedResourceKeys.UpdateFailed]);
            //message
            return Success((string)_stringLocalizer[SharedResourceKeys.Updated]);
        }
        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Check if the user is exit or not
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return NotFound<string>();

            // Call service that make edit
            var result = await _userManager.DeleteAsync(user);

            // Return appropriate response
            if (!result.Succeeded) return BadRequest<string>(_stringLocalizer[SharedResourceKeys.DeletedFailed]);
            return Success((string)_stringLocalizer[SharedResourceKeys.Deleted]);
        }
        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            // get user by id, and if not exist return notFound 
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return NotFound<string>();

            // change user password
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            // or 
            //var user = _userManager.HasPasswordAsync(user);
            //await _userManager.RemovePasswordAsync(user);
            //await _userManager.AddPasswordAsync(user, request.NewPassword);

            // return appropraite response
            //if (!result.Succeeded) return BadRequest<string>(_stringLocalizer[SharedResourceKeys.ChangingPasswordFailed]);
            if (!result.Succeeded) return BadRequest<string>(result.Errors.FirstOrDefault().Description);
            return Success((string)_stringLocalizer[SharedResourceKeys.Success]);
        }
        #endregion
    }
}