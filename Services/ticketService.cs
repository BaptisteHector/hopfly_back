using System;
using System.Collections.Generic;
using System.Linq;
using HopflyApi.Models;
using HopflyApi.Helpers;



namespace HopflyApi.Services
{
    public interface ITicketService
    {
        Ticket create(Ticket ticket);

        void update(Ticket ticket);

        void delete(int id);

        Activity[] getTicketActivities(int[] idActivities);
    }

    public class TicketService : ITicketService
    {
        private HopflyContext _context;

        public TicketService(HopflyContext context)
        {
            _context = context;
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _context.Tickets;
        }

        public Ticket GetById(int id)
        {
            return _context.Tickets.Find(id);
        }

        public Ticket create(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return ticket;
        }

        public Activity[] getTicketActivities(int[] idActivities) {
            var activity =  _context.Activities.Where(r => idActivities.Contains(r.id));
            return activity.ToArray();
        }

        public void update(Ticket ticketParam)
        {
            var ticket = _context.Tickets.Find(ticketParam.id);

            if (ticket == null)
                throw new AppException("Ticket not found");

            // update user properties



            _context.Tickets.Update(ticket);
            _context.SaveChanges();
        }

        public void delete(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                _context.SaveChanges();
            }
        }
    }
}