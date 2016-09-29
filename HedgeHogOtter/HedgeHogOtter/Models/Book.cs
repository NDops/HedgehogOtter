using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HedgeHogOtter.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Subject { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string BookCondition { get; set; }
        public string PublishPlace { get; set; }
        public int PublishYear { get; set; }
        public int Quantity { get; set; }
    }
}