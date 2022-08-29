using AuthServer.Core.Configuration;
using AuthServer.Core.DTO;
using AuthServer.Core.Entity;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Shared;
using AuthServer.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

        public AuthenticationService(IOptions<List<Client>> options,ITokenService tokenService,UserManager<UserApp> userManager,
            IUnitOfWork unitOfWork,IGenericRepository<UserRefreshToken> userRefreshTokenService
            )
        {
            _clients = options.Value;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _userManager = userManager;
            _userRefreshTokenService = userRefreshTokenService;
        }


        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Response<TokenDto>.Failure("Email or Password wrong!", 400, true);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Response<TokenDto>.Failure("Password is wrong!", 400, true);

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            //Kullanıcının daha önce refresh token'ı yoksa yeni bir refresh token oluştururuz.
            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    Code = token.RefreshToken,
                    ExpirationDate = token.RefreshTokenExpirationDate
                });
            }
            else
            {
                //Refresh token'ı daha önce oluşmuşsa yeni bir refresh token göndeririz.
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.ExpirationDate = token.RefreshTokenExpirationDate;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token,200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            if (clientLoginDto == null) throw new ArgumentNullException(nameof(clientLoginDto));

            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null) return Response<ClientTokenDto>.Failure("ClientId or ClientSecret Not Found!", 404,false);

            var clientToken = _tokenService.CreateTokenByClient(client);

            return Response<ClientTokenDto>.Success(clientToken, 200);
        }

      

        public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var existedRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existedRefreshToken == null) return Response<TokenDto>.Failure("Refresh token not found!", 404, true);



           var user = await _userManager.FindByIdAsync(existedRefreshToken.UserId);

            if (user == null) return Response<TokenDto>.Failure("User Id Not Found!", 404, true);

            var tokenDto = _tokenService.CreateToken(user);

            existedRefreshToken.Code = tokenDto.RefreshToken;
            existedRefreshToken.ExpirationDate = tokenDto.RefreshTokenExpirationDate;


            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);



        }

        //Refresh token'ı null yapma
        public async Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var existedRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existedRefreshToken == null) return Response<NoDataDto>.Failure("Refresh token not found!", 404, true);


            _userRefreshTokenService.Remove(existedRefreshToken);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
