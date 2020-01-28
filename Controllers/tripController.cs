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
    public class tripController : ControllerBase
    {
        private readonly HopflyContext _context;
        private ITripService _tripService;
        private IUserService _userService;


        public tripController(ITripService tripService, IUserService userService, HopflyContext context)
        {
            _userService = userService;
            _tripService = tripService;
            _context = context;
        }

        public int generateID()
        {
            int TripId = _context.Trips.OrderByDescending(u => u.id).Select(q => q.id).FirstOrDefault();
            if (TripId != 0)
                return TripId + 1;
            return 1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            return await _context.Trips.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
            var Trip = await _context.Trips.FindAsync(id);

            if (Trip == null)
            {
                return NotFound();
            }

            return Trip;
        }

        [HttpPost]
        public IActionResult createTrip([FromBody]Trip Trip)
        {
            Trip.id = generateID();
            System.Diagnostics.Debug.WriteLine(Trip);

            try 
            {
                // save 
                _tripService.create(Trip);
                _userService.UpdateTripList(Trip.user_id.Remove(Trip.user_id.Length - 1).Split(',').Select(int.Parse).ToArray(), Trip.id);
                return Ok(new { message = "success"});
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, Trip item)
        {
            if (id != item.id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        public Activity[] GetTripActivities(string id)
        {
            var Trips = _tripService.getTripActivities(id.Remove(id.Length - 1).Split(',').Select(int.Parse).ToArray());
            return Trips;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(long id)
        {
            var Trip = await _context.Trips.FindAsync(id);

            if (Trip == null)
            {
                return NotFound();
            }

            _context.Trips.Remove(Trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}