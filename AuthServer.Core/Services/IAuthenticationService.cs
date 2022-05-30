using AuthServer.Core.DTO;
using AuthServer.Shared;
using AuthServer.Shared.DTO;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);

        Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);

        Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken); //Eger bu kullancii hakkinda refresh token varsa null'a set et.

        Task<Response<ClientTokenDto>> CreateTokenByClientAsync(ClientLoginDto clientLoginDto);
    }
}
