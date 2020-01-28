using System;

namespace HopflyApi.Models
{
    public class Ticket
    {
        public int id { get; set; }
        public string arrival { get; set; }
        public DateTime dep_date { get; set; }
        public DateTime arr_date { get; set; }
        public string number { get; set; }
        public string departure { get; set; }
    }
}