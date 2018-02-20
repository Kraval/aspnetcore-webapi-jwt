using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreWebApiJwt.Data;
using AspNetCoreWebApiJwt.Infrastructure;
using AspNetCoreWebApiJwt.Models;
using AspNetCoreWebApiJwt.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCoreWebApiJwt.Controllers
{
   [Route("api/[controller]")] 
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtIssuerOptions _jwtOptions;

        public AccountsController(UserManager<ApplicationUser> userManager, IMapper mapper,
                                  ApplicationDbContext appDbContext,IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
         }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
            var userIdentity = _mapper.Map<ApplicationUser>(model);
            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            if (!result.Succeeded) return new BadRequestObjectResult("Error creating account");
            return new OkObjectResult("Account created");
            }
            catch(Exception ex)
            {
                 return new BadRequestObjectResult("Error creating account");
            }
        }

        // POST api/accounts/token
        [HttpPost("token")]
        public async Task<IActionResult> Post([FromBody]LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Find user by Username
            var userToVerify = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (userToVerify == null)
            {
                return BadRequest(ModelState);
            }
            //If User is found, let's verify Username/Password Pari
            var isValid = await _userManager.CheckPasswordAsync(userToVerify, loginViewModel.Password);
            if (isValid)
            {

                var claims = new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, userToVerify.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, userToVerify.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                var jwt = new JwtSecurityToken(
                            issuer: _jwtOptions.Issuer,
                            audience: _jwtOptions.Audience,
                            claims: claims,
                            notBefore: _jwtOptions.NotBefore,
                            expires: _jwtOptions.Expiration,
                            signingCredentials: _jwtOptions.SigningCredentials);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var response = new
                {
                    id = userToVerify.Id,
                    auth_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
                };
                return new OkObjectResult(response);
            }
            else
            {
                return BadRequest("Invalid username or password.");
            }
        }
        
    }
}