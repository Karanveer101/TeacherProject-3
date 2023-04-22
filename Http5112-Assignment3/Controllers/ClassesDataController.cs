using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Http5112_Assignment3.Models;
using MySql.Data.MySqlClient;

namespace Http5112_Assignment3.Controllers
{
    public class ClassesDataController : ApiController
    {
        private SchoolDbContext _context = new SchoolDbContext();

        [HttpGet]
        [Route("api/ClassesData/ListClasses")]
        public IEnumerable<Classes> ListClasses()
        {
            using (var conn = _context.AccessDatabase())
            {
                //open connection
                conn.Open();

                //sql query
                var cmd = new MySqlCommand("SELECT * from classes", conn);

                //implement read functionality
                using (var reader = cmd.ExecuteReader())
                {
                    var classes = new List<Classes>();

                    while (reader.Read())
                    {
                        var classs = new Classes
                        {
                            ClassId = Convert.ToInt32(reader["classid"]),
                            ClassCode = reader["classcode"].ToString(),
                            ClassName = reader["classname"].ToString(),
                        };

    classes.Add(classs);
                    }

return classes;
                }
            }
        }
    }
}