using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HedgeHogOtter.Models
{
    public class Book
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Subject { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        [Required]
        public string BookCondition { get; set; }
        public string PublisherPlace { get; set; }
        public int PublishYear { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int Id { get; set; }
        public int FeatureFlag { get; set; }
    }
}