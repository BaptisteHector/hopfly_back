using System;

namespace HopflyApi.Models
{
    public class Trip
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime begin_date { get; set; }
        public DateTime end_date { get; set; }
        public string user_id { get; set; }
        public string description { get; set; }
        public string activities_id {get; set;}
        public string location {get; set;}
        public string ticket_id {get; set;}
        public string logement_id {get; set;} 
        public int plan_id {get; set;} 
    }
}