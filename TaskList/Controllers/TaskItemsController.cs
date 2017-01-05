using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskList.Models;
using Microsoft.AspNet.Identity;

namespace TaskList.Controllers
{
	[Authorize]
	public class TaskItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TaskItems
        public async Task<ActionResult> Index()
        {
            var taskItems = db.TaskItems.Include(t => t.User);
            return View(await taskItems.ToListAsync());
        }

        // GET: TaskItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskItem taskItem = await db.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return HttpNotFound();
            }
            return View(taskItem);
        }

        // GET: TaskItems/Create
        public ActionResult Create()
        {
			var task = new TaskItem();
            return View(task);
        }

        // POST: TaskItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,IsCompleted,DateCreated,DateCompleted,UserId")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
				taskItem.UserId = HttpContext.User.Identity.GetUserId();
				taskItem = SetCompleteDate(taskItem);
                db.TaskItems.Add(taskItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(taskItem);
        }

        // GET: TaskItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskItem taskItem = await db.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return HttpNotFound();
            }
            return View(taskItem);
        }

        // POST: TaskItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,IsCompleted,DateCreated,DateCompleted,UserId")] TaskItem taskItem)
        {
			taskItem = SetCompleteDate(taskItem);
            if (ModelState.IsValid)
            {
				taskItem.UserId = HttpContext.User.Identity.GetUserId();
				db.Entry(taskItem).State = EntityState.Modified;
				await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(taskItem);
        }

        // GET: TaskItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskItem taskItem = await db.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return HttpNotFound();
            }
            return View(taskItem);
        }

        // POST: TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TaskItem taskItem = await db.TaskItems.FindAsync(id);
            db.TaskItems.Remove(taskItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

		public TaskItem SetCompleteDate(TaskItem taskItem)
		{
			if (taskItem.IsCompleted && !taskItem.DateCompleted.HasValue)
			{
				taskItem.DateCompleted = DateTime.Now;
			}
			else
				if (!taskItem.IsCompleted && taskItem.DateCompleted.HasValue)
			{
				taskItem.DateCompleted = null;
			}
			return taskItem;
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
