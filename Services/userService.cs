using System;
using System.Collections.Generic;
using System.Linq;
using HopflyApi.Models;
using HopflyApi.Helpers;



namespace HopflyApi.Services
{
    public interface IUserService
    {
        User login(string username, string password);

        User register(User user);
        void UpdateTripList(int[] idUser, int idTrip);
        Trip[] getUserTrips(int[] idTrips);

    }

    public class UserService : IUserService
    {
        private HopflyContext _context;

        public UserService(HopflyContext context)
        {
            _context = context;
        }

        public User login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (password != user.password)
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User register(User user)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.username == user.username))
                throw new AppException("Username \"" + user.username + "\" is already taken");

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam)
        {
            var user = _context.Users.Find(userParam.id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.username != user.username)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.username == userParam.username))
                    throw new AppException("Username " + userParam.username + " is already taken");
            }

            // update user properties
            user.username = userParam.username;


            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public Trip[] getUserTrips(int[] idTrips) {
            var trips =  _context.Trips.OrderByDescending(u => u.begin_date).Where(r => idTrips.Contains(r.id));
            System.Diagnostics.Debug.WriteLine("TRIPS == " + trips.Count());
            return trips.ToArray();
        }

        public void UpdateTripList(int[] idUser, int idTrip)
        {
            var user = _context.Users.Where(r => idUser.Contains(r.id));

            if (user == null)
                throw new AppException("User not found");
            
            foreach (var item in user) {
                item.trip_id += idTrip.ToString() + ",";
                _context.Users.Update(item);
            }
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}