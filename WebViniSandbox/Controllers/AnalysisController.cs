using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;
using MvcContrib.Pagination;
using MvcContrib.Sorting;
using WebViniSandbox.Models.ViewModel;

namespace WebViniSandbox.Controllers
{
    [Authorize]
    public class AnalysisController : Controller
    {
        private vinisandboxContext db = new vinisandboxContext();

        //
        // GET: /Analysis/

        public ActionResult Index(int? page, int? size, GridSortOptions sort, int id_file = 0)
        {
            sort.Column = String.IsNullOrEmpty(sort.Column) ? "start_date" : sort.Column;            
            IEnumerable<analysis> analysis = new List<analysis>();
            var file = db.files.Find(id_file);
            if (file != null)
            {
                ViewBag.id_file = id_file;
                analysis = file.file_detail.analyses.ToList();
            }                        
            if (Request.IsAjaxRequest())
            {
                ViewData["sort"] = sort;
                analysis = analysis.OrderBy(sort.Column, sort.Direction).AsPagination(page ?? 1, size ?? 10);
                return PartialView("_Grid", analysis);
            }
            sort.Direction = SortDirection.Descending;
            ViewData["sort"] = sort;
            analysis = analysis.OrderBy(sort.Column, sort.Direction).AsPagination(page ?? 1, size ?? 10);
            return View(analysis);            
        }

        //
        // GET: /Analysis/Details/5

        public ActionResult Details(int id = 0)
        {
            analysis analysis = db.analyses.Find(id);
            if (analysis == null)
                return HttpNotFound();
            if (Request.IsAjaxRequest())
            {
                ViewBag.Scans = analysis.antivirus_scan.AsPagination(1, 1000000);
                ViewBag.Dns = analysis.dns.AsPagination(1, 1000000);
                ViewBag.Images = analysis.result_file;
                ViewBag.Events = analysis.computer_event.AsPagination(1, 1000000); ;
                return PartialView("_Dynamic", analysis);                
            }
            else
            {
                Mapper.Mapper mapper = new Mapper.Mapper();
                IPagination<Info> infos = mapper.HashMapper(analysis.file_detail).AsPagination(1, 100);
                ViewBag.Infos = infos;
                IPagination<import_function> imports = analysis.file_detail.pe_file.import_function.OrderBy(p => p.import_library.name).AsPagination(1, 1000000);
                ViewBag.Imports = imports;
                IPagination<export_function> exports = analysis.file_detail.pe_file.export_function.OrderBy(p => p.name).AsPagination(1, 1000000);
                ViewBag.Exports = exports;
                IPagination<resource> resources = analysis.file_detail.pe_file.resources.OrderBy(p => p.id).AsPagination(1, 1000000);
                ViewBag.Resources = resources;
                IPagination<section> sections = analysis.file_detail.pe_file.sections.OrderBy(p => p.id).AsPagination(1, 1000000);
                ViewBag.Section = sections;
                IPagination<miscellaneous> misc = analysis.miscellaneous.OrderBy(p => p.type).AsPagination(1, 1000000);
                ViewBag.Miscellaneous = misc;
                return View(analysis);
            }                                                        
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}