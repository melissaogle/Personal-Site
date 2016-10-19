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
    public class TSTDepartmentsController : Controller
    {
        private TicketTrackerEntities db = new TicketTrackerEntities();

        // GET: TSTDepartments
        public ActionResult Index()
        {
            return View(db.TSTDepartments.ToList());
        }
        //Commented out because all the 
        // GET: TSTDepartments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);
        //    if (tSTDepartment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tSTDepartment);
        //}

        // GET: TSTDepartments/Create
        public ActionResult InactiveDepartments()
        {
            return View(db.TSTDepartments.ToList());
        }
        [Authorize(Roles = "Admin, HRAdmin")]

        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin, HRAdmin")]

        // POST: TSTDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeptID,DeptName,DeptDesc,IsActive")] TSTDepartment tSTDepartment)
        {
            if (ModelState.IsValid)
            {
                db.TSTDepartments.Add(tSTDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tSTDepartment);
        }
        [Authorize(Roles = "Admin, HRAdmin")]

        // GET: TSTDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);
            if (tSTDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tSTDepartment);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // POST: TSTDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeptID,DeptName,DeptDesc,IsActive")] TSTDepartment tSTDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tSTDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tSTDepartment);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // GET: TSTDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);
            if (tSTDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tSTDepartment);
        }
        [Authorize(Roles = "Admin, HRAdmin")]
        // POST: TSTDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TSTDepartment tSTDepartment = db.TSTDepartments.Find(id);
            db.TSTDepartments.Remove(tSTDepartment);
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
