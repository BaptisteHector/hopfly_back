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
    public class logementController : ControllerBase
    {
        private readonly HopflyContext _context;
        private ILogementService _logementService;
        private IUserService _userService;


        public logementController(ILogementService logementService, IUserService userService, HopflyContext context)
        {
            _userService = userService;
            _logementService = logementService;
            _context = context;
        }

        public int generateID()
        {
            int LogementId = _context.Logements.OrderByDescending(u => u.id).Select(q => q.id).FirstOrDefault();
            if (LogementId != 0)
                return LogementId + 1;
            return 1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logement>>> GetLogements()
        {
            return await _context.Logements.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Logement>> GetLogement(int id)
        {
            var Logement = await _context.Logements.FindAsync(id);

            if (Logement == null)
            {
                return NotFound();
            }

            return Logement;
        }

        [HttpPost]
        public IActionResult createLogement([FromBody]Logement Logement)
        {
            Logement.id = generateID();
            try 
            {
                // save 
                _logementService.create(Logement);
                return Ok(new { message = "success"});
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, Logement item)
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
        public Activity[] GetLogementActivities(string id)
        {
            var Logements = _logementService.getLogementActivities(id.Remove(id.Length - 1).Split(',').Select(int.Parse).ToArray());
            return Logements;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(long id)
        {
            var Logement = await _context.Logements.FindAsync(id);

            if (Logement == null)
            {
                return NotFound();
            }

            _context.Logements.Remove(Logement);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}