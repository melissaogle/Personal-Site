using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketTracker.data;

//in order to create a user object (from identity) when we create and employee
//object, we need to add references for Identity, Owin, Models 
//Identity.EntityFramework
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentitySample.Models;




namespace TicketTracker.web.Controllers
{
    public class TSTEmployeesController : Controller
    {
        private TicketTrackerEntities db = new TicketTrackerEntities();
        //private object selectedRoles;

        //Big gain o conntecting these two tables together
        //(Identity>AspNETUsers and TSTEmployees) is taht we can get
        //access to OUR TSTEmoloyee object based off of who is logged in. 

        //Create a custom method to retrieve the current logged in ASPNetUser
        //and return the associated TSTEmoloyee object

        public TSTEmployee GetCurrentEmployee()
        {
            ////Do Stuff
            ////get the current loggied in user
            //var currentUserLoggedIn = User.Identity.Name;//email
            ////create the employee object that has the same email as above
            //TSTEmployee e = db.TSTEmployees.FirstOrDefault(emp => emp.Email ==
            //currentUserLoggedIn);
            //return e;

            //more succinct code
            //this code replaces everything above
            return db.TSTEmployees
                .FirstOrDefault(x => x.Email == User.Identity.Name);
            
        }

        // GET: TSTEmployees
        public ActionResult Index()
        {
            var tSTEmployees = db.TSTEmployees.Include(t => t.TSTDepartment);
            return View(tSTEmployees.ToList());
        }

        public ActionResult InactiveEmployees()
        {
            var tSTEmployees = db.TSTEmployees.Where(e=>e.IsActive==false).Include(t => t.TSTDepartment);
            return View(tSTEmployees.ToList());
        }
        [Authorize(Roles = "Admin, HRAdmin")]

        // GET: TSTEmployees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            if (tSTEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tSTEmployee);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // GET: TSTEmployees/Create
        public ActionResult Create()
        {
            //Part of creating the user object when creating an employee is selecting
            //the appropriate role(s).  retrieve the list of roles for this 
            //Application (Identity.Owin) Open Web Interface for .NET

            var RoleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();

            //create a viewbag object ot pass to the view to be consumed and populate our checkboxlist (we 
            //will need this in the [Http] post as well.)
            ViewBag.RoleId =
                new SelectList(RoleManager.Roles.ToList().OrderBy(r => r.Name),"Name", "Name");






            ViewBag.DeptID = new SelectList(db.TSTDepartments, "DeptID", "DeptName");
            return View();
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // POST: TSTEmployees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpID,fname,lname,address1,address2,City,State,zip,phone1,phone2,Email,DeptID,Image,DOB,HireDate,SeparationDate,IsActive,JobTitle,Notes,UserID")]
        TSTEmployee tSTEmployee, string[] selectedRoles,
            HttpPostedFileBase empPhoto)

        {
            if (ModelState.IsValid)
            {
                #region Create Identity User when creating an Employee
                //This same type of code can be found in the UsersAdmin Create()
                //HttpPost action for teh controller

                //Create a new UserManager object
                var userManager = System.Web.HttpContext.Current.
                    GetOwinContext().GetUserManager<ApplicationUserManager>();


                //create the new application user and assign value to the Username
                //and email properties
                var newUser = new ApplicationUser()
                {
                    UserName = tSTEmployee.Email,
                    Email = tSTEmployee.Email
                };

                //use the usermanager to create the userName and password combo
                userManager.Create(newUser, "P@ssw0rd");
                //other options
                //EL3ph@nt, Y3aR!ght, FirstName+x12345$
                //This actually sets the password for the user
                //you could go to the Identity.Config and configure
                //to email the user their password.  If you do that, do not forget
                //to setup email in the web.config or in the Identity.config
                //There is NO need to do both

                //add the user to teh selected role(s)
                if (selectedRoles != null)
                {
                    userManager.AddToRoles(newUser.Id, selectedRoles);
                }
                else
                {
                    //no selection to the cbl
                    userManager.AddToRole(newUser.Id, "User");
                }

                //now that we ahve an Identity User, assign the valie to 
                //the Employee.UserID property
                tSTEmployee.UserID = newUser.Id;

                #endregion

                //prework, find a no photo avalable image
                //we will use this image as a default instead of Leaving it null

                //default the value of the variable 
                //imageName to our no photo name value
                string imageName = "noPhoto.jpg";

                //see if the user uploaded an image
                if (empPhoto != null)
                {
                    //get the file name
                    imageName = empPhoto.FileName;

                    //use the file name to get the extension
                    string ext = imageName.Substring(imageName.LastIndexOf('.'));

                    //check the extension to ensure it is appropriate
                    //create an array of VALID extensions and compare
                    //the current value of ext
                    string[] goodExts = new string[]
                        {".png", ".jpg", ".jpeg", ".gif"};

                    //if it its valid
                    if (goodExts.Contains(ext))
                    {
                        //rename the file - Guid (Global Unique Identifier
                        //and concatonate teh extenstion
                        imageName = Guid.NewGuid() + ext;

                        //save the FILE to the webserver
                        empPhoto.SaveAs(Server.MapPath(
                            "~/Content/images/Employee/" + imageName));
                    }
                    else
                    {
                        //BAD EXTENSION
                        //assign the imageName to [ourDefaultName]
                        imageName = "NoPhoto.jpg";
                    }
                }
                //no matter what
                //send the text value of the imagename to the db
                tSTEmployee.Image = imageName;






                db.TSTEmployees.Add(tSTEmployee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var RoleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();

            //create a viewbag object ot pass to the view to be consumed and populate our checkboxlist (we 
            //will need this in the [Http] post as well.)
            ViewBag.RoleId =
                new SelectList(RoleManager.Roles.ToList().OrderBy(r => r.Name), "Name", "Name");

            ViewBag.DeptID = new SelectList(db.TSTDepartments, "DeptID", "DeptName", tSTEmployee.DeptID);
            return View(tSTEmployee);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // GET: TSTEmployees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            if (tSTEmployee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeptID = new SelectList(db.TSTDepartments, "DeptID", "DeptName", tSTEmployee.DeptID);
            return View(tSTEmployee);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // POST: TSTEmployees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpID,fname,lname,address1,address2,City,State,zip,phone1,phone2,Email,DeptID,Image,DOB,HireDate,SeparationDate,IsActive,JobTitle,Notes,UserID")]
        TSTEmployee tSTEmployee, HttpPostedFileBase empPhoto)
        {
            if (ModelState.IsValid)
            {

                #region FileUPload
                //prework, find a no photo avalable image
                //we will use this image as a default instead of Leaving it null

               //the image already exists with the record

                //see if the user uploaded an image
                if (empPhoto != null)
                {
                    //get the file name
                   string imageName = empPhoto.FileName;

                    //use the file name to get the extension
                    string ext = imageName.Substring(imageName.LastIndexOf('.'));

                    //check the extension to ensure it is appropriate
                    //create an array of VALID extensions and compare
                    //the current value of ext
                    string[] goodExts = new string[]
                        {".png", ".jpg", ".jpeg", ".gif"};

                    //if it its valid
                    if (goodExts.Contains(ext))
                    {
                        //rename the file - Guid (Global Unique Identifier
                        //and concatonate teh extenstion
                        imageName = Guid.NewGuid() + ext;

                        //save the FILE to the webserver
                        empPhoto.SaveAs(Server.MapPath(
                            "~/Content/images/Employee/" + imageName));

                        //send the text value of the imagename to the db
                        tSTEmployee.Image = imageName;

                    }
                   
                }
                //the hiddenfield will continue to hold the original image and 
                //care for anything that shuld bvwe in the else               


                #endregion



                db.Entry(tSTEmployee).State = EntityState.Modified;
                db.SaveChanges();

                if(tSTEmployee.IsActive)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("InactiveEmployees");
                }
                    
               
                
            }

            ViewBag.DeptID = new SelectList(db.TSTDepartments, "DeptID", "DeptName", tSTEmployee.DeptID);
            return View(tSTEmployee);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // GET: TSTEmployees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            if (tSTEmployee == null)
            {
                return HttpNotFound();
            }
            return View(tSTEmployee);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // POST: TSTEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTEmployee tSTEmployee = db.TSTEmployees.Find(id);
            db.TSTEmployees.Remove(tSTEmployee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
