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
    public class contactController : ControllerBase
    {
        private readonly HopflyContext _context;
        private IContactService _contactService;
        private IUserService _userService;


        public contactController(IContactService contactService, IUserService userService, HopflyContext context)
        {
            _userService = userService;
            _contactService = contactService;
            _context = context;
        }

        public int generateID()
        {
            int ContactId = _context.Contacts.OrderByDescending(u => u.id).Select(q => q.id).FirstOrDefault();
            if (ContactId != 0)
                return ContactId + 1;
            return 1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var Contact = await _context.Contacts.FindAsync(id);

            if (Contact == null)
            {
                return NotFound();
            }

            return Contact;
        }

        [HttpPost]
        public IActionResult createContact([FromBody]Contact Contact)
        {
            Contact.id = generateID();
            try 
            {
                // save 
                _contactService.create(Contact);
                return Ok(new { message = "success"});
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, Contact item)
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
        public Activity[] GetContactActivities(string id)
        {
            var Contacts = _contactService.getContactActivities(id.Remove(id.Length - 1).Split(',').Select(int.Parse).ToArray());
            return Contacts;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(long id)
        {
            var Contact = await _context.Contacts.FindAsync(id);

            if (Contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(Contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}