using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityPosgre.Servise;
using IdentityPosgre.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityPosgre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _role;
        private readonly IRepository _rep;
        private readonly ITokenServise _servise;

        public MainController(ITokenServise servise,IRepository repository,RoleManager<IdentityRole> role)
        {
            _role = role;
            _rep = repository;
            _servise = servise;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome !");
        }
        [Authorize(Roles =Role.Admin)]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePerson([FromBody] Person person)
        {
            var res = await _rep.CreateAsync(person);
            if (res.IsSuccsess)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("createIdentityUser")]
        public async Task<IActionResult> CreateIdentityUser([FromBody] RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var res = await _servise.CreateUser(model);
                if (res.IsSuccsess)
                    return Ok(res);
                return BadRequest(res);
            }
            return BadRequest(new UserManagerResponce() { IsSuccsess = false, Message = "model is not valid please check data" });
        }
        [HttpGet("gettoken")]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                var res = await _servise.LoginUser(login);
                if (res.IsSuccsess)
                    return Ok(res);
                return BadRequest(res);
            }
            return BadRequest(new UserManagerResponce() { IsSuccsess = false, Message = "model is not valid please check data" });
        }
        [HttpGet("getsimple")]
        public async Task<IActionResult> GetSimpleUser()
        {
            var res = await _rep.GetSimpleUser();
            if (res.IsSuccsess)
                return Ok(res);
            return BadRequest(res);
        }
        [Authorize(Roles = Role.Admin)]
        [HttpGet("getadmin")]
        public async Task<IActionResult> GetAdmin()
        {
            var res = await _rep.GetAllData();
            if (res.IsSuccsess)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpGet("createrole")]
        public async Task<IActionResult> CreateRole()
        {
            var role1 = new IdentityRole(Role.Admin);
            var role2 = new IdentityRole(Role.User);
            await _role.CreateAsync(role1);
            await _role.CreateAsync(role2);
            return Ok("All Role create succsessfully");
        }
    }
}
