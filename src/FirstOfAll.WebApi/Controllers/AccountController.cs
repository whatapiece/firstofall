using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FirstOfAll.Infra.CrossCutting.Identity.Models;
using FirstOfAll.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace FirstOfAll.WebApi.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("account/login")]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return Response(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

            if (result.Succeeded)
            {
                var token = GetJwtSecurityToken(model.Email);
                _logger.LogInformation(1, "User logged in.");

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Response(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("account/logout")]
        public async Task<object> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return Response("User logged out.");
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("account/register")]
        public async Task<object> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return Response(model);
 
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");

                _logger.LogInformation(3, "User created a new account with password.");
                return Response("User created a new account with password.");
            }

            return Response(model);
        }
                
        private JwtSecurityToken GetJwtSecurityToken(string user)
        {
            return new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                GetTokenClaims(user),
                expires: DateTime.UtcNow.AddDays(Convert.ToInt64(_configuration["JwtExpireDays"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"])),
                    SecurityAlgorithms.HmacSha256)
            );
        }

        private static IEnumerable<Claim> GetTokenClaims(string user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user),
            };
        }

    }
}
