using System;
using System.Collections.Generic;
using System.Linq;
using HopflyApi.Models;
using HopflyApi.Helpers;


namespace HopflyApi.Services
{
    public interface IActivityService
    {
        Activity create(Activity activity);

        void update(Activity activity);

        void delete(int id);
    }

    public class ActivityService : IActivityService
    {
        private HopflyContext _context;

        public ActivityService(HopflyContext context)
        {
            _context = context;
        }

        public IEnumerable<Activity> GetAll()
        {
            return _context.Activities;
        }

        public Activity GetById(int id)
        {
            return _context.Activities.Find(id);
        }

        public Activity create(Activity activity)
        {
            _context.Activities.Add(activity);
            _context.SaveChanges();

            return activity;
        }

        public void update(Activity activityParam)
        {
            var activity = _context.Activities.Find(activityParam.id);

            if (activity == null)
                throw new AppException("Activity not found");

            // update user properties
            activity.name = activityParam.name;
            activity.description = activityParam.description;
            activity.location = activityParam.location;


            _context.Activities.Update(activity);
            _context.SaveChanges();
        }

        public void delete(int id)
        {
            var activity = _context.Activities.Find(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                _context.SaveChanges();
            }
        }
    }
}