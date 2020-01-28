using System;

namespace HopflyApi.Models
{
    public class Activity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int price {get; set; }
        public double mark {get; set; }
        public int amount_mark {get; set; }
    }
}