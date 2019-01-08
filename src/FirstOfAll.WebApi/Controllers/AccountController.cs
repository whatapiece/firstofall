using System.Security.Claims;
using System.Threading.Tasks;
using FirstOfAll.Infra.CrossCutting.Identity.Models;
using FirstOfAll.Infra.CrossCutting.Identity.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FirstOfAll.WebApi.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("account/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //NotifyModelStateErrors();
                return Response(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
            //if (!result.Succeeded)
                //NotifyError(result.ToString(), "Login failure");

            _logger.LogInformation(1, "User logged in.");
            return Response("User logged in.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("account/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //NotifyModelStateErrors();
                return Response(model);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // User claim for write customers data
                await _userManager.AddClaimAsync(user, new Claim("Customers", "Write"));
                await _userManager.AddClaimAsync(user, new Claim("Customers", "Remove"));

                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation(3, "User created a new account with password.");
                return Response(model);
            }

            //AddIdentityErrors(result);
            return Response("User created a new account with password.");
        }
    }
}
