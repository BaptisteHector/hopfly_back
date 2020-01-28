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
    public class planController : ControllerBase
    {
        private readonly HopflyContext _context;
        private IPlanService _planService;
        private ITripService _tripService;


        public planController(IPlanService planService, ITripService tripService, HopflyContext context)
        {
            _tripService = tripService;
            _planService = planService;
            _context = context;
        }

        public int generateID()
        {
            int PlanId = _context.Plans.OrderByDescending(u => u.id).Select(q => q.id).FirstOrDefault();
            if (PlanId != 0)
                return PlanId + 1;
            return 1;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            return await _context.Plans.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(int id)
        {
            var Plan = await _context.Plans.FindAsync(id);

            if (Plan == null)
            {
                return NotFound();
            }

            return Plan;
        }

        [HttpPost("{id}")]
        public IActionResult createPlan([FromBody]Plan Plan, int id)
        {
            Plan.id = generateID();
            System.Diagnostics.Debug.WriteLine(Plan);
            try 
            {
                // save
                _planService.create(Plan);
                _tripService.updatePlanId(id, Plan.id);
                return Ok(new { message = "success" });
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(Plan item, int id)
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
        public Activity[] GetPlanActivities(string id)
        {
            var Plans = _planService.getPlanActivities(id.Remove(id.Length - 1).Split(',').Select(int.Parse).ToArray());
            return Plans;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(long id)
        {
            var Plan = await _context.Plans.FindAsync(id);

            if (Plan == null)
            {
                return NotFound();
            }

            _context.Plans.Remove(Plan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}