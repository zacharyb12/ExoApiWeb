using Bll.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entity;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return Ok(_userService.GetUsers());
        }

        [HttpPost]
        public IActionResult Create(CreateUser user)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            _userService.AddUser(user);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(CreateUser user,Guid Id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.UpdateUser(user,Id);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(Guid Id) 
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.DeleteUser(Id);
            return Ok();
        }
    }
}
