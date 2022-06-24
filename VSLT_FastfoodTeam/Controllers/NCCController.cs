using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class NCCController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page, string searching)
        {
            if (page == null)
                page = 1;


            if (searching == null)
            {
                var all_unit = db.NCCs.ToList().OrderBy(m => m.MaNCC);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.NCCs.Where(x => x.TenNCC.Contains(searching)).ToList().OrderBy(m => m.MaNCC);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, NCC tl)
        {
            var mancc = collection["MaNCC"];
            var tenncc = collection["TenNCC"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var sdt = collection["SDT"];
            if (string.IsNullOrEmpty(tenncc))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                tl.MaNCC = mancc;
                tl.TenNCC = tenncc;
                tl.Diachi = diachi;
                tl.Email = email;
                tl.SDT = sdt;
                db.NCCs.InsertOnSubmit(tl);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
             
            return this.Create();
        }
    }
}