using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Http5112_Assignment3.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Web.Http.Cors;


namespace Http5112_Assignment3.Controllers
{
    public class TeacherDataController : ApiController
    {

        private SchoolDbContext Teacher = new SchoolDbContext();

        //The database context class which allows us to access our MySQL Database.
        //AccessDatabase switched to a static method, one that can be called without an object.


        //This Controller Will access the teachers table of our blog database. Non-Deterministic.
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <returns>
        /// A list of teacher Objects with fields mapped to the database column values (first name, last name, bio).
        /// </returns>
        /// <example>GET api/teacherData/Listteachers -> {teacher Object, teacher Object, teacher Object...}</example>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]


        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //create an instance of a connection
            MySqlConnection Conn = Teacher.AccessDatabase();


            //open the connection between the web server and database
            Conn.Open();


            //establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "SELECT * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or concat(teacherfname, ' ', teacherlname) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();
            //get result set of query into a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();


            List<Teacher> Teachers = new List<Teacher>();

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = Convert.ToString(ResultSet["employeenumber"]);


                Teacher newTeacher = new Teacher();
                newTeacher.TeacherId = TeacherId;
                newTeacher.TeacherFname = TeacherFname;
                newTeacher.TeacherLname = TeacherLname;
                newTeacher.EmployeeNumber = EmployeeNumber;
               
               




                //add the teacher name to the list
                Teachers.Add(newTeacher);
            }
            //close the connection between the sql database and web server
            Conn.Close();

            //return the final list of teacher names
            return Teachers;




        }
            [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]

        public Teacher FindTeacher(int id)
            {
                Teacher newTeacher = new Teacher();
            //create an instance of a connection
            MySqlConnection Conn = Teacher.AccessDatabase();


            //open the connection between the web server and database
            Conn.Open();


            //establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "SELECT * FROM teachers where teacherid = "+id;

           
            //get result set of query into a variable 
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = (string)ResultSet["teacherfname"];
                string TeacherLname = (string)ResultSet["teacherlname"];
                string EmployeeNumber = ResultSet["employeenumber"] == DBNull.Value ? null : (string)ResultSet["employeenumber"];


                newTeacher.TeacherId = TeacherId;
                newTeacher.TeacherFname = TeacherFname;
                newTeacher.TeacherLname = TeacherLname;
                newTeacher.EmployeeNumber = EmployeeNumber;


            }
    


            return newTeacher;
            }
        /// <summary>
       
        /// </summary>
        /// <param name="id"></param>
        /// <example>POST : /api/TeacherData/DeleteTeacher/3</example>
        
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]

        public void DeleteTeacher(int id)
        {
            //create an instance of a connection
            MySqlConnection Conn = Teacher.AccessDatabase();


            //open the connection between the web server and database
            Conn.Open();


            //establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
        /// <summary>
        /// Adds an Teacher to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the author's table. </param>
        /// <example>
        /// POST api/TeacherData/AddTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        /// }
        /// </example>

        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]

        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Teacher.AccessDatabase();

            Debug.WriteLine(NewTeacher.TeacherFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber) values (@TeacherFname,@TeacherLname,@EmployeeNumber)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();



        }

        /// <summary>
        /// Updates a Teacher on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the Teacher's table.</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmployeeNumber":"t1244",
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Teacher.AccessDatabase();

            Debug.WriteLine(TeacherInfo.TeacherFname);
            //Debug.WriteLine(TeacherInfo.TeacherFname);

            //Open the connection between the web server and database
            Conn.Open();
            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            //SQL QUERY
            cmd.CommandText = "update teachers set teacherfname=@teacherFname, teacherlname=@teacherLname, employeeNumber = @employeeNumber where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }


    }
}
