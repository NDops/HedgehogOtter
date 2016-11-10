using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HedgeHogOtter.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public String Firstname { get; set; }
        public String Lastname { get; set; }
        public String Email { get; set; }
    }
}