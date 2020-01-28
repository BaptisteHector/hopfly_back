using System;
using System.Collections.Generic;
using System.Linq;
using HopflyApi.Models;
using HopflyApi.Helpers;



namespace HopflyApi.Services
{
    public interface IPlanService
    {
        Plan create(Plan plan);

        void update(Plan plan);

        void delete(int id);

        Activity[] getPlanActivities(int[] idActivities);
    }

    public class PlanService : IPlanService
    {
        private HopflyContext _context;

        public PlanService(HopflyContext context)
        {
            _context = context;
        }

        public IEnumerable<Plan> GetAll()
        {
            return _context.Plans;
        }

        public Plan GetById(int id)
        {
            return _context.Plans.Find(id);
        }

        public Plan create(Plan plan)
        {
            _context.Plans.Add(plan);
            _context.SaveChanges();

            return plan;
        }

        public Activity[] getPlanActivities(int[] idActivities) {
            var activity =  _context.Activities.Where(r => idActivities.Contains(r.id));
            return activity.ToArray();
        }

        public void update(Plan planParam)
        {
            var plan = _context.Plans.Find(planParam.id);

            if (plan == null)
                throw new AppException("Plan not found");

            // update user properties



            _context.Plans.Update(plan);
            _context.SaveChanges();
        }

        public void delete(int id)
        {
            var plan = _context.Plans.Find(id);
            if (plan != null)
            {
                _context.Plans.Remove(plan);
                _context.SaveChanges();
            }
        }
    }
}