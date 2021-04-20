using IdentityPosgre.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityPosgre.Servise
{
    public interface ITokenServise
    {
        Task<UserManagerResponce> CreateUser(RegisterViewModel model);
        Task<UserManagerResponce> LoginUser(LoginViewModel login);
    }
    public class TokenServise : ITokenServise
    {
        private readonly UserManager<IdentityUser> _user;
        private readonly IConfiguration _con;

        public TokenServise(UserManager<IdentityUser> user,IConfiguration configuration)
        {
            _user = user;
            _con = configuration;
        }
        public async Task<UserManagerResponce> CreateUser(RegisterViewModel model)
        {
            if (model == null)
                return new UserManagerResponce() { IsSuccsess = false, Message = "model can not be null" };
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.EmailAddress,
            };
            var result = await _user.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                await _user.AddToRoleAsync(user, Role.Admin);
                    return new UserManagerResponce() { IsSuccsess = true, Message = "User Create Successfuly" };
            }
            return new UserManagerResponce
            {
                Message = "User didn't created",
                IsSuccsess = false,
                Errors = result.Errors.Select(op => op.Description).ToList()
            };
        }

        public async Task<UserManagerResponce> LoginUser(LoginViewModel login)
        {
            
            if (login == null)
                return new UserManagerResponce() { IsSuccsess = false, Message = "login can not be null" };
            var user = await _user.FindByEmailAsync(login.EmailAddress);
            if (user == null)
                return new UserManagerResponce() { IsSuccsess = false, Message = "invalid email address" };
            var res = await _user.CheckPasswordAsync(user, login.Password);
            if (!res)
                return new UserManagerResponce() { IsSuccsess = false, Message = "invalid password" };
            var role = await _user.GetRolesAsync(user);
            var claim = new[]
            {
                new Claim("Email",login.EmailAddress),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Role,role[0])
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_con["JWT:Key"]));
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return new UserManagerResponce() { IsSuccsess = true, Message = new JwtSecurityTokenHandler().WriteToken(token), ExireDate = token.ValidTo };
        }
    }
}
