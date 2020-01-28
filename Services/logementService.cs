using System;
using System.Collections.Generic;
using System.Linq;
using HopflyApi.Models;
using HopflyApi.Helpers;



namespace HopflyApi.Services
{
    public interface ILogementService
    {
        Logement create(Logement logement);

        void update(Logement logement);

        void delete(int id);

        Activity[] getLogementActivities(int[] idActivities);
    }

    public class LogementService : ILogementService
    {
        private HopflyContext _context;

        public LogementService(HopflyContext context)
        {
            _context = context;
        }

        public IEnumerable<Logement> GetAll()
        {
            return _context.Logements;
        }

        public Logement GetById(int id)
        {
            return _context.Logements.Find(id);
        }

        public Logement create(Logement logement)
        {
            _context.Logements.Add(logement);
            _context.SaveChanges();

            return logement;
        }

        public Activity[] getLogementActivities(int[] idActivities) {
            var activity =  _context.Activities.Where(r => idActivities.Contains(r.id));
            return activity.ToArray();
        }

        public void update(Logement logementParam)
        {
            var logement = _context.Logements.Find(logementParam.id);

            if (logement == null)
                throw new AppException("Logement not found");

            // update user properties



            _context.Logements.Update(logement);
            _context.SaveChanges();
        }

        public void delete(int id)
        {
            var logement = _context.Logements.Find(id);
            if (logement != null)
            {
                _context.Logements.Remove(logement);
                _context.SaveChanges();
            }
        }
    }
}