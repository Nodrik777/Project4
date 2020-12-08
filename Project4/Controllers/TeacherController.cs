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
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime TeacherHireDate, decimal Salary)
        {
            //Identify that thismethod is running
            //Identify the inputs provided from the form

            Debug.WriteLine("I have access to create a method!");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(TeacherHireDate);
            Debug.WriteLine(Salary);

            //Server side validation for preventing form to submit with empty fields.
            if (TeacherFname == "" || EmployeeNumber == "") return RedirectToAction("Add");

            Teacher NewTeacher = new Teacher();
            NewTeacher.Teacherfname = TeacherFname;
            NewTeacher.Teacherlname = TeacherLname;
            NewTeacher.Employeenumber = EmployeeNumber;
            NewTeacher.TeacherHireDate = TeacherHireDate;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);
            return RedirectToAction("List");
        }
    }
}