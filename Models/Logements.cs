using System;

namespace HopflyApi.Models
{
    public class Logement
    {
        public int id { get; set; }
        public string location { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
    }
}