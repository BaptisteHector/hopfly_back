using System;

namespace HopflyApi.Models
{
    public class Plan
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public double mark {get; set; }
        public int amount_mark {get; set; }
        public string activities {get; set; }
    }
}