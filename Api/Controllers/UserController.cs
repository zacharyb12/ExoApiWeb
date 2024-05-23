using Bll.CustomsExeptions;
using Bll.Services.UserServiceBll;
using Microsoft.AspNetCore.Mvc;
using Models.Dto.UserDto;
using Models.Entity.CreateUser;
using Models.Entity.User;
using Models.Entity.UserModels;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        public ActionResult<IEnumerable<User>> Get() 
        {
            return Ok(_userService.GetUsers());
        }


        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
        public IActionResult GetById(Guid id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> Login([FromBody]UserLogin user) 
        {
            string? token = _userService.Login(user);
            if (token is not null)
            {
                return Ok(token);
            }
            return BadRequest(StatusCodes.Status400BadRequest);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateUser))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Create(CreateUser user)
        {
            if (ModelState.IsValid) 
            {
                try
                {

                    User? userCreate = _userService.AddUser(user);

                    if (userCreate is not null)
                    {
                        return CreatedAtAction(nameof(GetById), new { id = userCreate.Id }, userCreate);
                    }
                }
                catch (AlreadyExistException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
                return BadRequest(ModelState);
        }


        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUser))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Update( UpdateUser updateUser, Guid id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    User? user = _userService.UpdateUser(updateUser,id);

                    if (user is not null)
                    {
                        return Ok(user);
                    }
                }
                catch (AlreadyExistException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
                return NotFound(id); 
            
        }

        [HttpDelete]
        [Route("{id:Guid}")] // Annotation alternative pour indiquer la route
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
        public IActionResult DeleteUser(Guid id)
        {
            bool delete = _userService.DeleteUser(id);

            return delete ? NoContent() : NotFound(id);
        }
    }
}
