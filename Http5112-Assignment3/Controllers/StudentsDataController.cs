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
    public class StudentsDataController : ApiController
    {
        private SchoolDbContext _context = new SchoolDbContext();

        [HttpGet]
        [Route("api/StudentsData/ListStudents")]
        public IEnumerable<Students> ListStudents()
        {
            using (var conn = _context.AccessDatabase())
            {

                //open connection
                conn.Open();

                //sql query
                var cmd = new MySqlCommand("SELECT * from Students", conn);


                //implement read functionality
                using (var reader = cmd.ExecuteReader())
                {
                    var students = new List<Students>();

                    while (reader.Read())
                    {
                        var student = new Students
                        {
                            StudentId = Convert.ToInt32(reader["studentid"]),
                            StudentFname = reader["studentfname"].ToString(),
                            StudentLname = reader["studentlname"].ToString(),
                            StudentNumber = reader["studentnumber"].ToString()
                        };

                        students.Add(student);
                    }

                    return students;
                }
            }
        }
    }
}