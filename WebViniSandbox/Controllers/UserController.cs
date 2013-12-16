using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViniSandbox.Models;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using MvcContrib.UI.Grid;
using WebViniSandbox.Security;

namespace WebViniSandbox.Controllers
{
    [Authorize(Roles="admin")]
    public class UserController : Controller
    {
        private vinisandboxContext db = new vinisandboxContext();

        //
        // GET: /User/

        public ActionResult Index(int? page, int? size, GridSortOptions sort, string searcher)
        {
            sort.Column = String.IsNullOrEmpty(sort.Column) ? "name" : sort.Column;
            IEnumerable<user> users = db.users.ToList();
            if (searcher != null)
            {             
                users = users.Where(a => a.name.ToLower().Contains(searcher.ToLower()) || a.email.ToLower().Contains(searcher.ToLower()));
            }
            users = users.OrderBy(sort.Column, sort.Direction).AsPagination(page ?? 1, size ?? 10);
            ViewData["sort"] = sort;
            if (Request.IsAjaxRequest())
                return PartialView("_Grid", users);
            return View(users);
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return PartialView("Edit");
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(user user)
        {
            ModelState.Remove("id");
            if(string.IsNullOrEmpty(user.name))
                ModelState.AddModelError("name", "Nome é obrigatorio.");
            if (string.IsNullOrEmpty(user.nickname))
                ModelState.AddModelError("nickname", "Apelido é obrigatorio.");

            if (db.users.Count(p => p.email == user.email) > 0)
                ModelState.AddModelError("email", "Email já cadastrado.");
            if (db.users.Count(p => p.nickname == user.nickname) > 0)
                ModelState.AddModelError("nickname", "Apelido já cadastrado.");

            user.creation_date = DateTime.Now;

            if (ModelState.IsValid)
            {
                user.password = MD5Helper.StringToMD5(user.password);
                db.users.Add(user);
                db.SaveChanges();
                return Content("ok");
            }

            return PartialView("Edit", user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(user user)
        {
            var DbAux = new vinisandboxContext();
            if (DbAux.users.Count(p => p.email == user.email) > 0)
                ModelState.AddModelError("email", "Email já cadastrado.");
            ModelState.Remove("password");
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(user.password))
                    user.password = MD5Helper.StringToMD5(user.password);
                else                
                    user.password = DbAux.users.ToList().Find(p => p.id == user.id).password;
                                    
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("ok");
                }
                catch (Exception ex)
                {
                }                
            }
            return PartialView(user);
        }


        public ActionResult Delete(int id)
        {
            user user = db.users.Find(id);
            return PartialView("Details", user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return Content("ok");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}