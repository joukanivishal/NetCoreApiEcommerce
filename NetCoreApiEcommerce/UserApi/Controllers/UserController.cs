using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public static List<User> Users = new List<User>();
        public readonly JwtTokenHandler _jwtTokenHandler;

        public UserController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return Users.ToList();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return Users.FirstOrDefault(r => r.Id == id);
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            Users.Add(user);
            return Ok();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] User user)
        {
            if (!Users.Any(r => r.Id == user.Id))
            {
                return NoContent();
            }
            var obj = Users.FirstOrDefault(x => x.Id == user.Id);
            if (obj != null) obj.Name = user.Name;
            return Ok();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!Users.Any(r => r.Id == id))
            {
                return NoContent();
            }
            var obj = Users.FirstOrDefault(x => x.Id == id);
            Users.Remove(obj);
            return Ok();
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult AuthenticateUser([FromBody] AuthenticationRequest authenticationRequest)
        {
            var user = Users.FirstOrDefault(r => r.Username == authenticationRequest.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            if (user.Password.Equals(authenticationRequest.Password))
            {
                return Ok(_jwtTokenHandler.GenerateJwtToken(authenticationRequest));
            }
            return Ok();
        }

    }
}
