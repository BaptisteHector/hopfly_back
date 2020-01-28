using System;

namespace HopflyApi.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string trip_id {get; set; }
        public string email {get; set; }
        public string contact_id {get; set; }
        public string friend_id {get; set; }
    }
}