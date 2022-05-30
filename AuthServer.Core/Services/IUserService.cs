using AuthServer.Core.DTO;
using AuthServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto userDto);

        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
    }
}
