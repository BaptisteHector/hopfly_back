using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HopflyApi.Models;
using HopflyApi.Services;
using System;
using Microsoft.AspNetCore.Cors;
using HopflyApi.Helpers;
using Microsoft.Data.SqlClient;

namespace HopflyApi.Controllers
{
    [Route("api/Hopfly/[controller]/[action]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly HopflyContext _context;
        private IUserService _userService;

        public userController(IUserService userService, HopflyContext context)
        {
            _userService = userService;
            _context = context;
        }

        public int generateID()
        {
             var User = _context.Users.OrderByDescending(u => u.id).FirstOrDefault();
            return User.id + 1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> getusers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return User;
        }

        [HttpGet("{id}")]
        public Trip[] GetUserTrips(string id)
        {
            var ids = id.Remove(id.Length - 1).Split(',').Select<string, int>(int.Parse).ToArray();
            var Trips = _userService.getUserTrips(ids);
            return Trips;
        }

        [HttpPost]
        public IActionResult login([FromBody]User item)
        {
            var user = _userService.login(item.username, item.password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(new {
                id = user.id,
                username = user.username,
                password = user.password,
                trip_id = user.trip_id,
            });
        }

        [HttpPost]
        public IActionResult register([FromBody]User user)
        {
            user.id = generateID();
            try 
            {
                // save 
                _userService.register(user);
                return Ok(new { message = "success"});
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, User item)
        {
            if (id != item.id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(long id)
        {
            var User = await _context.Users.FindAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            _context.Users.Remove(User);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}