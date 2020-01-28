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
    public class activityController : ControllerBase
    {
        private readonly HopflyContext _context;
        private IActivityService _activityService;
        private IUserService _userService;


        public activityController(IActivityService activityService, IUserService userService, HopflyContext context)
        {
            _userService = userService;
            _activityService = activityService;
            _context = context;
        }

        public int generateID()
        {
            var Activity = _context.Activities.OrderByDescending(u => u.id).FirstOrDefault();
            if (Activity != null)
                return Activity.id + 1;
            return 1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            var Activity = await _context.Activities.FindAsync(id);

            if (Activity == null)
            {
                return NotFound();
            }

            return Activity;
        }

        [HttpPost]
        public IActionResult createActivity([FromBody]Activity Activity)
        {
            Activity.id = generateID();
            try 
            {
                // save 
                _activityService.create(Activity);
                return Ok(new { id = Activity.id,
                name = Activity.name,
                description = Activity.description,
                location = Activity.location,
                price = Activity.price,
                mark = Activity.mark,
                amount_mark = Activity.amount_mark});
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, Activity item)
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
            var Activity = await _context.Activities.FindAsync(id);

            if (Activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(Activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}