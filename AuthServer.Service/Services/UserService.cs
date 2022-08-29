using AuthServer.Core.DTO;
using AuthServer.Core.Entity;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Shared;
using AuthServer.Shared.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(UserManager<UserApp> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new UserApp
            {
                Email = userDto.Email,
                UserName = userDto.UserName
            };
            byte[] hashedPassword;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())

            {
                hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            }
            user.PasswordHash = Convert.ToBase64String(hashedPassword);

            var isExistedUser = await _userManager.FindByEmailAsync(userDto.Email);
            if (isExistedUser != null) return Response<UserAppDto>.Failure("Such a user already exist!", 400, true);

           IdentityResult result =  await _userManager.CreateAsync(user,userDto.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return Response<UserAppDto>.Failure(new ErrorDto(errors,true),400);
            }
            await _unitOfWork.CommitAsync();
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
           

        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var isExistedUser = await _userManager.FindByNameAsync(userName);

            if (isExistedUser == null) return Response<UserAppDto>.Failure("There is no such a user!", 404,true);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(isExistedUser), 200);
        }
    }
}
