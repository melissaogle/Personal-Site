using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketTracker.data;

namespace TicketTracker.web.Controllers
{
    public class TSTTicketsController : Controller
    {
        private TicketTrackerEntities db = new TicketTrackerEntities();

        #region Custom Methods

        //prework for Notes
        //Get Emokoyee(tech - For Notes, for Create - It is the submittedByID)
        //In order for this to work, we must have tied the identity 
        //AspNetUsers Table to the TSTEmployee table. (Means each TSTEmployee is 
        //represented in the AspNetUsers Tabel via the email address. )
        //This will not work if you are still using Admin@example.com UNLESS you created
        //an employee with taht email address. 

        public TSTEmployee GetCurrentEmoloyee()
        {
            var currentUser = User.Identity.Name;//AspNetUser UserName (email)
            TSTEmployee e = db.TSTEmployees
                .FirstOrDefault(x => x.Email == currentUser); //compare the TSTEmployee address
            //and only return the TSTEmoloyee that synchs with the AspNetUser Name
            return e;
        }


        //AddTechNote()
        /// <summary>
        /// Notation informations:
        /// This is the method that is going to be called by jquery/Ajax from the edie
        /// view to add the note on the fly and persist it to the (Tech)Notes Table
        /// it will post the NEW note to the view in the notes section BEFORE submitting the 
        /// form.
        /// </summary>

        public JsonResult AddTechNote(int ticketId, string note)
        {
            //get the ticket that was passed in to the method and retrieve the associated
            //record.
            TSTTicket ticket = db.TSTTickets.Single(x => x.TicketID == ticketId);

            //get the current logged on employee so taht we can fulfill the 
            //TechID field for the TSTTechNote
            TSTEmployee tech = GetCurrentEmoloyee();

            //Make the note
            //make sure the TSTEmployee is not null
            if (tech != null)
            {
                //Create the TSTNote object
                TSTTicketNote newNote = new TSTTicketNote()
                {
                    //object initialization syntax
                    //property = (is assiged the value) of
                    //hard coded / passed in data
                    TicketID = ticketId,//passed into the method
                    Notation = note,//passed into the method
                    EmpID = tech.EmpID,//derived
                    NotationDate = DateTime.Now

                };
                //Persist the note at the data structure
                //save changes
                db.TSTTicketNotes.Add(newNote);
                db.SaveChanges();
                //----the note is created - exists at the data structure
                //now we need to send it back to the view, so we can display it in the
                //edit view
                //This NEVER hits the wevserver, jQuery has not idea what the TSTNote
                //objec is.  We send over datat that can be parsed by jQuery. 

                var data = new
                {
                    //otf (on the fly variable) = newNote.Property
                    TechNotes = newNote.Notation,
                    Tech = newNote.TSTEmployee.fname,
                    Date = string.Format("{0:g}", newNote.NotationDate)
                    //because this doesn't hit the webserver, we format it here
                };
                //send notation infor to the browser for jQuery to parse
                return Json(data, JsonRequestBehavior.AllowGet);

            }//end the if

            return null;//no note if employee is null. 
        }//ends the AddNewNote()


        #endregion

        // GET: TSTTickets
        public ActionResult Index()
        {
            var tSTTickets = db.TSTTickets.Include(t => t.TSTEmployee).Include(t => t.TSTEmployee1).Include(t => t.TSTTicketStatus);
            //Show only open tickets to unauthenticated users
            //1) Know your statuses (what is open vs. what is closed)

            
                //only "open" tickets
                return View(tSTTickets.Where(s => s.StatusID != 3).ToList());
            
        }
        [Authorize]
        public ActionResult InactiveTickets()
        {
            var tSTTickets = db.TSTTickets.Include(t => t.TSTEmployee).Include(t => t.TSTEmployee1).Include(t => t.TSTTicketStatus);
            //Show only open tickets to unauthenticated users
            //1) Know your statuses (what is open vs. what is closed)


           
              
                return View(tSTTickets.Where(s => s.StatusID == 3).ToList());
            
        }
       


        
        // GET: TSTTickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            if (tSTTicket == null)
            {
                return HttpNotFound();
            }
            return View(tSTTicket);
        }
        [Authorize]
        // GET: TSTTickets/Create
        public ActionResult Create()
        {
            //ViewBag.SubmittedByID = new SelectList(db.TSTEmployees, "EmpID", "fname");
            //ViewBag.AssignedTechID = new SelectList(db.TSTEmployees, "EmpID", "fname");
            //ViewBag.StatusID = new SelectList(db.TSTTicketStatuses, "StatusID", "StatusName");
            return View();
        }
        [Authorize]
        // POST: TSTTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketID,StatusID,SubmissionDate,SubmittedByID,CompletedDate,TicketDesc,AssignedTechID")] TSTTicket tSTTicket)
        {
            if (ModelState.IsValid)
            {
                tSTTicket.SubmittedByID = GetCurrentEmoloyee().EmpID;
                tSTTicket.SubmissionDate = DateTime.Now;
                tSTTicket.StatusID = 1;

                db.TSTTickets.Add(tSTTicket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.SubmittedByID = new SelectList(db.TSTEmployees, "EmpID", "fname", tSTTicket.SubmittedByID);
            //ViewBag.AssignedTechID = new SelectList(db.TSTEmployees, "EmpID", "fname", tSTTicket.AssignedTechID);
            //ViewBag.StatusID = new SelectList(db.TSTTicketStatuses, "StatusID", "StatusName", tSTTicket.StatusID);
            return View(tSTTicket);
        }
        [Authorize(Roles = "Admin, Technician")]
        // GET: TSTTickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            if (tSTTicket == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubmittedByID = new SelectList(db.TSTEmployees, "EmpID", "fname", tSTTicket.SubmittedByID);
            ViewBag.AssignedTechID = new SelectList(db.TSTEmployees.Where(x=>x.DeptID==2), "EmpID", "FullName", tSTTicket.AssignedTechID);
            ViewBag.StatusID = new SelectList(db.TSTTicketStatuses, "StatusID", "StatusName", tSTTicket.StatusID);
            return View(tSTTicket);
        }
        [Authorize(Roles = "Admin, Technician")]
        // POST: TSTTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketID,StatusID,SubmissionDate,SubmittedByID,CompletedDate,TicketDesc,AssignedTechID")] TSTTicket tSTTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tSTTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.SubmittedByID = new SelectList(db.TSTEmployees, "EmpID", "fname", tSTTicket.SubmittedByID);
            ViewBag.AssignedTechID = new SelectList(db.TSTEmployees.Where(x => x.DeptID == 2), "EmpID", "FullName", tSTTicket.AssignedTechID);
            ViewBag.StatusID = new SelectList(db.TSTTicketStatuses, "StatusID", "StatusName", tSTTicket.StatusID);
            return View(tSTTicket);
        }
        [Authorize(Roles = "Admin, Technician")]
        // GET: TSTTickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            if (tSTTicket == null)
            {
                return HttpNotFound();
            }
            return View(tSTTicket);
        }
        [Authorize(Roles = "Admin, Technician")]
        // POST: TSTTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTTicket tSTTicket = db.TSTTickets.Find(id);
            db.TSTTickets.Remove(tSTTicket);
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
