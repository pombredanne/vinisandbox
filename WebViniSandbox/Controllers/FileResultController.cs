using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViniSandbox.Models;
using System.Text.RegularExpressions;

namespace WebViniSandbox.Controllers
{
    public class FileResultController : Controller
    {
        private vinisandboxContext db = new vinisandboxContext();

        //
        // GET: /FileResult/

        public ActionResult Index(int id)
        {
            var result_file = db.result_file.Find(id);
            if (result_file == null)
                return HttpNotFound();
            if (Regex.IsMatch(result_file.program_name, ".+.(?:jpg|jpeg|gif|png)$"))
            {
                return new FileContentResult(result_file.data, "image/jpg");
            }
            return Content("");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}