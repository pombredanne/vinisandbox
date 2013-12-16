using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViniSandbox.Models;
using WebViniSandbox.Security;
using MvcContrib.UI.Grid;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using System.IO;
using WebViniSandbox.Util;

namespace WebViniSandbox.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private vinisandboxContext db = new vinisandboxContext();

        //
        // GET: /File/

        public ActionResult Index(bool? group, bool? owner, int? id_file_detail, int? page, int? size, GridSortOptions sort, string searcher, int? id_user)
        {            
            IEnumerable<file> files = db.files.ToList();
            if (owner.HasValue && owner.Value)
            {
                files = files.Where(p => p.user.email == User.Identity.Name);
                ViewBag.owner = true;
            }
            if (id_file_detail.HasValue)
            {
                files = files.Where(a => a.id_file_detail == id_file_detail.Value);
                ViewBag.id_file_detail = id_file_detail.Value;
            }
            if (searcher != null)
            {             
                files = files.Where(a => a.name.ToLower().Contains(searcher.ToLower()) || searcher.Equals(a.file_detail.md5) 
                    || searcher.Equals(a.file_detail.sha1) || searcher.Equals(a.file_detail.sha256) || searcher.Equals(a.file_detail.sha512) 
                    || searcher.Equals(a.file_detail.ssdeep) || searcher.Equals(a.file_detail.crc32));               
            }
            if (id_user.HasValue)
            {                
                files = files.Where(p => p.user.id == id_user);
                ViewBag.id_user = id_user;
            }

            ViewData["sort"] = sort;
            ViewData["owner"] = owner;
            
            if (group.HasValue && group.Value)
            {
                sort.Column = String.IsNullOrEmpty(sort.Column) ? "md5" : sort.Column;
                ViewData["sort"] = sort;
                IEnumerable<file_detail> fd = files.Select(p => p.file_detail);
                fd = fd.Distinct().OrderBy(sort.Column, sort.Direction).AsPagination(page ?? 1, size ?? 10);
                ViewBag.data = fd;
                if (Request.IsAjaxRequest())
                    return PartialView("_GridFileDet");
                return View();
            }
            else
            {
                sort.Column = String.IsNullOrEmpty(sort.Column) ? "name" : sort.Column;
                ViewData["sort"] = sort;
                files = files.OrderBy(sort.Column, sort.Direction).AsPagination(page ?? 1, size ?? 10);
                ViewBag.data = files;
                if (Request.IsAjaxRequest())
                    return PartialView("_GridFile");
                return View();
            }                                    
       } 

        //
        // GET: /File/Details/5

        public ActionResult Details(bool? isFile, int id = 0)
        {
            ViewBag.isFile = isFile.HasValue && isFile.Value;
            file file = db.files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return PartialView(file);
        }

        //
        // GET: /File/Create

        public ActionResult Create()
        {
            return PartialView();
        }

        //
        // POST: /File/Create

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file)
        {
            try
            {
                user user = db.users.FirstOrDefault(p => p.email == User.Identity.Name);
                addFile(file, user, "manual");
                return Content("ok");   
            }
            catch (Exception)
            {                
            }           
            return Content("");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ProgReceive(HttpPostedFileBase file, string email, string password)
        {
            string hashedpass = MD5Helper.StringToMD5(password);
            var user = db.users.FirstOrDefault(p => p.email == email && p.password == hashedpass);
            if (user != null)
            {
                addFile(file, user, "server_honeypot");
                return Content("Received");
            }    
            return Content("Invalid Credential");
        }

        [NonAction]
        public void addFile(HttpPostedFileBase file, user user, string source)
        {
            var fileName = Path.GetFileName(file.FileName);
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();

            ViniSandbox.Models.file f = new ViniSandbox.Models.file();
            //f.analyzed = true;
            f.date = DateTime.Now;
            f.name = fileName;
            f.source = source;

            string md5 = MD5Helper.ByteToMD5(data);

            file_detail fd = db.file_detail.FirstOrDefault(p => p.md5 == md5);

            if (fd == null)
            {
                fd = new file_detail();
                fd.md5 = md5;
                fd.data = data;
                fd.analyzed = false;
            }

            user.files.Add(f);
            f.user = user;
            f.file_detail = fd;
            fd.files.Add(f);

            db.SaveChanges();
        }

        public ActionResult DownloadFileDet(int? id)
        {
            if (id.HasValue)
            {
                var aux = db.file_detail.FirstOrDefault(p => p.id == id.Value);
                if (aux != null)
                {
                    var file = aux.files.FirstOrDefault();
                    if (file != null)
                        return File(aux.data, System.Net.Mime.MediaTypeNames.Application.Octet, file.name);
                }
            }
            return HttpNotFound();
        }

        public ActionResult DownloadFile(int? id)
        {
            if (id.HasValue)
            {
                file file = db.files.FirstOrDefault(p => p.id == id.Value);
                if (file != null)
                {
                    return File(file.file_detail.data, System.Net.Mime.MediaTypeNames.Application.Octet, file.name);
                }                
            }
            return HttpNotFound();
        }

        public ActionResult Malicious(int id, bool? isFile, bool isChecked)
        {
            ViewBag.isFile = isFile.HasValue && isFile.Value;
            file file = null;
            if (isFile.HasValue && isFile.Value)
                file = db.files.Find(id);
            else
                file = db.file_detail.Find(id).files.FirstOrDefault();
            ViewBag.isChecked = isChecked;
            return PartialView(file);
        }

        [HttpPost, ActionName("Malicious")]
        public ActionResult MaliciousConfirmed(int id, bool isChecked)
        {
            file_detail fd = db.files.Find(id).file_detail;
            if (fd != null)
            {
                fd.malicious = !isChecked;
                db.Entry(fd).State = EntityState.Modified;
                db.SaveChanges();
                return Content("ok");
            }
            return Content("");
        }

        public ActionResult SendAntivirus(int id, bool isFile)
        {
            file file = db.files.Find(id);
            return PartialView(file);
        }

        [HttpPost, ActionName("SendAntivirus")]
        public ActionResult SendAntivirusConfirmed(int id)
        {
            file f = db.files.Find(id);
            try
            {
                if (f != null)
                {
                    string[] emails = db.antivirus.Where(p => !String.IsNullOrEmpty(p.email)).Select(p => p.email).ToArray();
                    EmailUtil.EnviaMensagemComAnexos(f, emails);
                    f.file_detail.antivirus_sended = true;
                    db.SaveChanges();
                    return Content("ok");
                }
            }
            catch (Exception)
            {
            }
            return Content("");
        }


        public ActionResult Reanalize(int id, bool isFile)
        {
            file file = db.files.Find(id);
            return PartialView(file);
        }

        [HttpPost, ActionName("Reanalize")]
        public ActionResult ReanalizeConfirmed(int id)
        {
            file f = db.files.Find(id);
            try
            {
                if (f != null)
                {
                    f.file_detail.analyzed = false;
                    db.SaveChanges();
                    return Content("ok");
                }
            }
            catch (Exception)
            {
            }
            return Content("");
        }
        ////
        //// GET: /File/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    file file = db.files.Find(id);
        //    if (file == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.id_file_detail = new SelectList(db.file_detail, "id", "type", file.id_file_detail);
        //    ViewBag.id_user = new SelectList(db.users, "id", "email", file.id_user);
        //    return View(file);
        //}

        ////
        //// POST: /File/Edit/5

        //[HttpPost]
        //public ActionResult Edit(file file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(file).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.id_file_detail = new SelectList(db.file_detail, "id", "type", file.id_file_detail);
        //    ViewBag.id_user = new SelectList(db.users, "id", "email", file.id_user);
        //    return View(file);
        //}

        ////
        //// GET: /File/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    file file = db.files.Find(id);
        //    if (file == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(file);
        //}

        ////
        //// POST: /File/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    file file = db.files.Find(id);
        //    db.files.Remove(file);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}