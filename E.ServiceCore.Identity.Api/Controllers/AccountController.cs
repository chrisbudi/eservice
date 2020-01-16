using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using E.ServiceCore.Identity.Api.Models;
using E.ServiceCore.Identity.Api.Service.Client;
using E.ServiceCore.Identity.Api.Service.Interfaces;
using E.ServiceCore.Identity.Data.DbContexts;
using E.ServiceCore.Identity.Data.Enities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace E.ServiceCore.Identity.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly RoleManager<ApplicationRoles> roleManager;
        readonly IConfiguration configuration;
        readonly ILogger<AccountController> logger;
        readonly ApplicationUserDbContext _db;


        IAuthService authService;

        SSOClient ssoCLient;

        const string LDAP_PATH = "ldap://10.255.141.134:389";
        const string LDAP_DOMAIN = "gagas.co.id";



        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IConfiguration configuration,
          IAuthService authService,
           ILogger<AccountController> logger, ApplicationUserDbContext db,
           RoleManager<ApplicationRoles> roleManager,
           SSOClient ssoClient)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.logger = logger;
            this.roleManager = roleManager;
            this.ssoCLient = ssoClient;
            this.authService = authService;
            _db = db;
        }


        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, isPersistent: false, lockoutOnFailure: false);

                if (!loginResult.Succeeded)
                { 
                    return BadRequest();
                }

                var user = await userManager.FindByNameAsync(loginModel.Username);

                if(user.IsEnabled == false)
                {
                    return BadRequest();
                }


                return Ok(GetToken(user));


            }
            return BadRequest(ModelState);

        }


        [HttpPost]
        [Route("auth")]
        public async Task<IActionResult> auth([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = authService.Login(loginModel.Username, loginModel.Password);
                    if (null != user)
                    {
                        var loginResult = await signInManager.PasswordSignInAsync(loginModel.Username, "123456", isPersistent: false, lockoutOnFailure: false);

                        var users = await userManager.FindByNameAsync(loginModel.Username);
                        
                        if (user.IsEnabled == false)
                        {
                            return BadRequest();
                        }



                        return Ok(GetToken(users));

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return BadRequest(ModelState);

        }


        [HttpPost]
        [Route("auth/sso")]
        public async Task<IActionResult> authSSO([FromBody] LoginSSOModel loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = authService.LoginSSO(loginModel.Username);
                    if (null != user)
                    {
                        var loginResult = await signInManager.PasswordSignInAsync(loginModel.Username, "123456", isPersistent: false, lockoutOnFailure: false);

                        var users = await userManager.FindByNameAsync(loginModel.Username);


                        if (user.IsEnabled == false)
                        {
                            return BadRequest();
                        }

                        return Ok(GetToken(users));

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return BadRequest(ModelState);

        }

        [Authorize]
        [HttpPost]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
                );
            return Ok(GetToken(user));

        }


        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    //TODO: Use Automapper instaed of manual binding

                    UserName = registerModel.Username,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    Email = registerModel.Email
                };



                var identityResult = await userManager.CreateAsync(user, "123456");


                if (identityResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    var newuser = new Users()
                    {
                        JabatanId = registerModel.JabatanId,
                        DepartmentId = registerModel.DepartmentId,
                        UserId = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        CreatedAt = DateTime.Now,
                        LocationId = registerModel.LocationId
                    };

                    _db.Users.Add(newuser);

                    _db.SaveChanges();


                    return Ok(GetToken(user));
                }
                else
                {
                    return BadRequest(identityResult.Errors);
                }
            }
            return BadRequest(ModelState);


        }



        [HttpPost]
        [Route("Role")]
        [AllowAnonymous]
        public async Task<IActionResult> Role([FromBody] ApplicationRoles roles)
        {
            if (ModelState.IsValid)
            {


                var roleResult = await roleManager.CreateAsync(roles);


                if (roleResult.Succeeded)
                {


                    return Ok(roleResult);
                }
                else
                {
                    return BadRequest(roleResult);
                }
            }
            return BadRequest(ModelState);


        }
        private string GetToken(IdentityUser user)
        {
            var utcNow = DateTime.UtcNow;


            var claims = new Claim[]
            {
                        new Claim(ClaimTypes.NameIdentifier, user.Id)
                        //new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        //new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(configuration.GetValue<int>("Tokens:Lifetime")),
                audience: configuration.GetValue<string>("Tokens:Audience"),
                issuer: configuration.GetValue<string>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }

    }
}