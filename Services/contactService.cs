using System;
using System.Collections.Generic;
using System.Linq;
using HopflyApi.Models;
using HopflyApi.Helpers;



namespace HopflyApi.Services
{
    public interface IContactService
    {
        Contact create(Contact contact);

        void update(Contact contact);

        void delete(int id);

        Activity[] getContactActivities(int[] idActivities);
    }

    public class ContactService : IContactService
    {
        private HopflyContext _context;

        public ContactService(HopflyContext context)
        {
            _context = context;
        }

        public IEnumerable<Contact> GetAll()
        {
            return _context.Contacts;
        }

        public Contact GetById(int id)
        {
            return _context.Contacts.Find(id);
        }

        public Contact create(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact;
        }

        public Activity[] getContactActivities(int[] idActivities) {
            var activity =  _context.Activities.Where(r => idActivities.Contains(r.id));
            return activity.ToArray();
        }

        public void update(Contact contactParam)
        {
            var contact = _context.Contacts.Find(contactParam.id);

            if (contact == null)
                throw new AppException("Contact not found");

            // update user properties



            _context.Contacts.Update(contact);
            _context.SaveChanges();
        }

        public void delete(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
            }
        }
    }
}