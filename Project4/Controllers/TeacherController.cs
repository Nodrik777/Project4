using System;
using System.Collections.Generic;
using Project4.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Project4.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher/Index
        public ActionResult Index()
        {
            return View();
        }


        //Get:/Teacher/List
        //Acquire information about the teachers and send it to the List.cshtml
        public ActionResult List(string SearchKey = null)
        {
            //We will get the information from ListTeachers method
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> TeacherDetail = controller.ListTeachers(SearchKey);
            return View(TeacherDetail);
        }

        //Get:/Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        //Get:/Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);
            return View(NewTeacher);
        }

        // GET: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //Get:/Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST:/Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, decimal Salary)
        {
            //Identify that thismethod is running
            //Identify the inputs provided from the form

            Debug.WriteLine("I have access to create a method!");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(Salary);

            //Server side validation for preventing form to submit with empty fields.
            if (TeacherFname == "" || EmployeeNumber == "") 
                return RedirectToAction("New");

            Teacher NewTeacher = new Teacher();
            NewTeacher.Teacherfname = TeacherFname;
            NewTeacher.Teacherlname = TeacherLname;
            NewTeacher.Employeenumber = EmployeeNumber;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

        //GET : /Teacher/Ajax_New
        public ActionResult Ajax_New()
        {
            return View();
        }

        /// <summary>
        /// Routes to a dynamically generated "Teacher Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Teacher</param>
        /// <returns>A dynamic "Update Teacher" webpage which provides the current information of the author and asks the user for new information as part of a form.</returns>
        /// <example>GET : /Teacher/Update/5</example>
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        public ActionResult Ajax_Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }


        /// <summary>
        /// Receives a POST request containing information about an existing author in the system, with new values. Conveys this information to the API, and redirects to the "Author Show" page of our updated author.
        /// </summary>
        /// <param name="id">Id of the Teacher to update</param>
        /// <param name="TeacherFname">The updated first name of the teacher</param>
        /// <param name="TeacherLname">The updated last name of the teacher</param>
        
        /// <param name="Salary">The updated email of the author.</param>
        /// <returns>A dynamic webpage which provides the current information of the teacher.</returns>
        /// <example>
        /// POST : /Teacher/Update/9
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"Salary":"99.99"
        /// }
        /// </example>
        [HttpPost]
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, decimal Salary)
        {
            Teacher TeacherInfo = new Teacher();
            TeacherInfo.Teacherfname = TeacherFname;
            TeacherInfo.Teacherlname = TeacherLname;
            TeacherInfo.Salary = Salary;
            

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);

            return RedirectToAction("Show/" + id);
        }

        

    }
}