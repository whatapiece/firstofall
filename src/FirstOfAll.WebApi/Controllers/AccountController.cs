using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FirstOfAll.Infra.CrossCutting.Identity.Models;
using FirstOfAll.Infra.CrossCutting.Identity.Models.AccountViewModels;
using FirstOfAll.WebApi.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FirstOfAll.WebApi.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger _logger;
        
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("account/login")]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Response(model);

            if (model != null)
            {
                var userIdentity = await _userManager
                    .FindByNameAsync(model.Email);

                if (userIdentity != null)
                {
                    var resultadoLogin = _signInManager
                        .CheckPasswordSignInAsync(userIdentity, model.Password, false)
                        .Result;

                    if (resultadoLogin.Succeeded)
                    {
                        var token = GetJwtSecurityToken(userIdentity);

                        return Response(new
                        {
                            authenticated = true,
                            message = "User logged in",
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }

            return Response(new
            {
                authenticated = false,
                message = "Invalid Credentials"
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("account/register")]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return Response(model);
 
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");

                return Response(new
                {
                    message = "User created a new account with password"
                });
            }

            model.ConfirmPassword = null;
            model.Password = null;

            return Response(new
            {
                message = "User created a new account with password",
                user = model
            });
        }
                
        private JwtSecurityToken GetJwtSecurityToken(ApplicationUser user)
        {
            var issuer = _jwtSettings.Issuer;
            var expireDays = _jwtSettings.ExpireDays;
            var key = _jwtSettings.Key;

            return new JwtSecurityToken(
               issuer,
                issuer,
                GetTokenClaims(user).Result,
                expires: DateTime.UtcNow.AddDays(Convert.ToInt64(expireDays)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256)
            );
        }

        private async Task<List<Claim>> GetTokenClaims(ApplicationUser user)
        {
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        
                new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.AddRange(userClaims);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }
    }
}
