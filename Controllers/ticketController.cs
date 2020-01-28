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
    public class ticketController : ControllerBase
    {
        private readonly HopflyContext _context;
        private ITicketService _ticketService;
        private IUserService _userService;


        public ticketController(ITicketService ticketService, IUserService userService, HopflyContext context)
        {
            _userService = userService;
            _ticketService = ticketService;
            _context = context;
        }

        public int generateID()
        {
            int TicketId = _context.Tickets.OrderByDescending(u => u.id).Select(q => q.id).FirstOrDefault();
            if (TicketId != 0)
                return TicketId + 1;
            return 1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var Ticket = await _context.Tickets.FindAsync(id);

            if (Ticket == null)
            {
                return NotFound();
            }

            return Ticket;
        }

        [HttpPost]
        public IActionResult createTicket([FromBody]Ticket Ticket)
        {
            Ticket.id = generateID();
            try 
            {
                // save 
                _ticketService.create(Ticket);
                return Ok(new { message = "success"});
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, Ticket item)
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
        public Activity[] GetTicketActivities(string id)
        {
            var Tickets = _ticketService.getTicketActivities(id.Remove(id.Length - 1).Split(',').Select(int.Parse).ToArray());
            return Tickets;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(long id)
        {
            var Ticket = await _context.Tickets.FindAsync(id);

            if (Ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(Ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}