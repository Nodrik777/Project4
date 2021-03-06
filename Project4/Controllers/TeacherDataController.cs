﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using Project4.Models;
using System.Diagnostics;
using System.Web.Http.Cors;


namespace Project4.Controllers
{
    public class TeacherDataController : ApiController
    {

        //The database context class which allow us to access the MySQL database.
        private TeacherDbContext assignment = new TeacherDbContext();

        //The controller will access the teachers table of our school database
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example> Get api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of all information about teachers
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]

        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = assignment.AccessDatabase();

            //OPen the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL query
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) " +
                "or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) " +
                "like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            // Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher Info
            List<Teacher> TeacherDetail = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                decimal salary = (decimal)ResultSet["salary"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.Teacherid = TeacherId;
                NewTeacher.Teacherfname = TeacherFname;
                NewTeacher.Teacherlname = TeacherLname;
                NewTeacher.Employeenumber = EmployeeNumber;
                NewTeacher.Salary = salary;

                //Add teacher details to the list
                TeacherDetail.Add(NewTeacher);
            }


            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teacher's Information
            return TeacherDetail;
        }

        /// <summary>
        /// Find Teacher from the MySQL Database through an id. Non-Detrerministic.
        /// </summary>
        /// <param name="id">The teacher Id</param>
        /// <returns>Teacher Object containing information about teacher with a matching id.Empty Teacher Object if Id doesn't match in the system</returns>
        ///<example>api/TeacherData/FindTeacher/6 -> return Teacher Object</example>
        ///<example>api/TeacherData/FindTeacher/9 -> return Teacher Object</example>
        [HttpGet]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = assignment.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers  where teachers.teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int Teacherid = (int)ResultSet["teacherid"];
                string Teacherfname = ResultSet["teacherfname"].ToString();
                string Teacherlname = ResultSet["teacherlname"].ToString();
                string Employeenumber = ResultSet["employeenumber"].ToString();
                DateTime Hiredate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                NewTeacher.Teacherid = Teacherid;
                NewTeacher.Teacherfname = Teacherfname;
                NewTeacher.Teacherlname = Teacherlname;
                NewTeacher.Employeenumber = Employeenumber;
                NewTeacher.Hiredate = Hiredate;
                NewTeacher.Salary = Salary;

            }
            return NewTeacher;
        }

        /// <summary>
        /// Deletes Teacher from the connected MySQL database if Teacher Id exists. Does not maintain integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the teacher</param>
        /// <example>Post: api/TeacherData/DeleteTeacher/3</example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]

        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = assignment.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid= @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();
        }



        /// <summary>
        /// Adds new teacher in the connected database  
        /// </summary>
        /// <param name="NewTeacher">An Object with fields that maps to the column of the teacher's table</param>
        ///<example>
        ///POST: api/TeacherData/AddTeacher
        ///FROM DATA /POST DATA/ REQUEST BODY
        ///{
        ///"TeacherFname":"Chris",
        ///"TeacherLname":"Wong",
        ///"EmployeeNumber":"T789",
        ///"Hiredate":"2020-12-01",
        ///"Salary":"55.5"
        ///}
        ///</example>


        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = assignment.AccessDatabase();

            Debug.WriteLine(NewTeacher.Teacherfname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Insert into teachers(teacherfname, teacherlname, employeenumber, salary)" +
                " values(@Teacherfname, @Teacherlname, @Employeenumber, CURRENT_DATE(), @Salary)";
            cmd.Parameters.AddWithValue("@Teacherfname", NewTeacher.Teacherfname);
            cmd.Parameters.AddWithValue("@Teacherlname", NewTeacher.Teacherlname);
            cmd.Parameters.AddWithValue("@Employeenumber", NewTeacher.Employeenumber);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();
        }

        /// <summary>
        /// Updates an Teacher on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the teacher's table.</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/ 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Tom",
        ///	"TeacherLname":"Kruz",
        ///	"Salary":"99.99"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]

        public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = assignment.AccessDatabase();

            //Debug.WriteLine(TeacherInfo.Teacherfname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update teachers set teacherfname=@Teacherfname, teacherlname=@Teacherlname, salary=@Salary, employeenumber=@Employeenumber, where teacherid=@Teacherid";
            cmd.Parameters.AddWithValue("@Teacherid", id);
            cmd.Parameters.AddWithValue("@Teacherfname", TeacherInfo.Teacherfname);
            cmd.Parameters.AddWithValue("@Teacherlname", TeacherInfo.Teacherlname);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@Employeenumber", TeacherInfo.Employeenumber);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }
    }
}
