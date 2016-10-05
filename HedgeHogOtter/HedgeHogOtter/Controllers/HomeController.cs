using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HedgeHogOtter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            int a = GetNumberOfRecords();


            return View();

            
        }

        public int GetNumberOfRecords()
        {
            int count = -1;

            string cs = ConfigurationManager.ConnectionStrings["HHDB"].ConnectionString;

            SqlConnection conn = new SqlConnection(cs);
            try
            {
                // Open the connection
                conn.Open();

                // 1. Instantiate a new command
                SqlCommand cmd = new SqlCommand("select count(*) from dbo.Book", conn);

                // 2. Call ExecuteScalar to send command
                count = (int)cmd.ExecuteScalar();
            }
            finally
            {
                // Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return count;
        }
    }
}