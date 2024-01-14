using Backend.CodeAuth;
using Backend.Models;
using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{

    private readonly IApiConfig _apiConfig;
    private readonly IUserService userService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(
        IApiConfig apiConfig,
        IUserService userService,
        UserManager<ApplicationUser> userManager)
    {
        _apiConfig = apiConfig;
        this.userService = userService;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ActionResult<AuthenticatedModel>> AuthenticateAsync([FromBody] LoginModel model, CancellationToken cancellationToken = default)
    {
        ApplicationUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email, cancellationToken: cancellationToken);

        if (user != null)
        {
            

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                AuthenticatedModel result = new();
                await MapAuthenticatedModelAsync(user.Id, result, cancellationToken);
                return result;

            }

            throw new AppBadDataException("Password is incorrect");
        }

        throw new AppNotFoundException();
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ActionResult<AuthenticatedModel>> RefreshTokenAsync([FromBody] RefreshTokenModel model, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(model.RefreshToken))
        {
            TokenValidationParameters validationParameters = TokenHelper.GetValidationParameters(_apiConfig.ApiSecret);

            JwtSecurityTokenHandler handler = new();

            AuthenticatedModel result = new();

            try
            {
                ClaimsPrincipal claims = handler.ValidateToken(model.RefreshToken, validationParameters, out SecurityToken? validatedToken);

                if (claims != null && validatedToken != null)
                {
                    Guid userId = Guid.Parse(claims.Claims.First(c => c.Type == ClaimTypes.Name).Value);

                    var additionalClaims = new List<Claim>();

                    await MapAuthenticatedModelAsync(userId, result, cancellationToken);

                    return Ok(result);
                }
            }
            catch
            {

            }
        }

        return Ok(null);
    }

    private async Task MapAuthenticatedModelAsync(Guid userId, AuthenticatedModel result, CancellationToken cancellationToken = default)
    {
        UserModel user = await userService.GetUser(userId, cancellationToken);

        if (user != null)
        {
            result.Id = user.Id;
            result.UserData = user;
            var claims = TokenHelper.CreateUserTokenClaims(user);
            result.Token = TokenHelper.CreateToken(claims!,
                _apiConfig.MinutesTillAuthorizationTokenExpires,
                _apiConfig.ApiSecret,
                _apiConfig.TokenIssuer,
                _apiConfig.TokenAudience);

            result.RefreshToken = TokenHelper.CreateToken(claims!,
                _apiConfig.MinutesTillRefreshTokenExpires,
                _apiConfig.ApiSecret,
                _apiConfig.TokenIssuer,
                _apiConfig.TokenAudience);
        }
    }
}
