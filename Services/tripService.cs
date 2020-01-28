using System;
using System.Collections.Generic;
using System.Linq;
using HopflyApi.Models;
using HopflyApi.Helpers;



namespace HopflyApi.Services
{
    public interface ITripService
    {
        Trip create(Trip trip);

        void update(Trip trip);

        void delete(int id);

        Activity[] getTripActivities(int[] idActivities);
        
        void updatePlanId(int idTrip, int idPlan);

    }

    public class TripService : ITripService
    {
        private HopflyContext _context;

        public TripService(HopflyContext context)
        {
            _context = context;
        }

        public IEnumerable<Trip> GetAll()
        {
            return _context.Trips;
        }

        public Trip GetById(int id)
        {
            return _context.Trips.Find(id);
        }

        public Trip create(Trip trip)
        {
            _context.Trips.Add(trip);
            _context.SaveChanges();

            return trip;
        }

        public void updatePlanId(int idTrip, int idPlan)
        {
            var trip = _context.Trips.Where(r => idTrip == r.id);

            if (trip == null)
                throw new AppException("Trip not found");
            
            foreach (var item in trip)
            {
                item.plan_id = idPlan;
                _context.Trips.Update(item);
            }
            _context.SaveChanges();
        }

        public Activity[] getTripActivities(int[] idActivities) {
            var activity =  _context.Activities.Where(r => idActivities.Contains(r.id));
            return activity.ToArray();
        }

        public void update(Trip tripParam)
        {
            var trip = _context.Trips.Find(tripParam.id);

            if (trip == null)
                throw new AppException("Trip not found");

            // update user properties
            trip.name = tripParam.name;
            trip.begin_date = tripParam.begin_date;
            trip.end_date = tripParam.end_date;
            trip.description = tripParam.description;
            trip.activities_id = tripParam.activities_id;
            trip.location = tripParam.location;


            _context.Trips.Update(trip);
            _context.SaveChanges();
        }

        public void delete(int id)
        {
            var trip = _context.Trips.Find(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                _context.SaveChanges();
            }
        }
    }
}