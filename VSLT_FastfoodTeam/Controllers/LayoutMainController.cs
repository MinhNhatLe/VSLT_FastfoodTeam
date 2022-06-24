using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class LayoutMainController : Controller
    {
        // GET: LayoutMain
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index()
        {
            var lstSP = from tt in db.SanPhams
                        where tt.MaLoaiSP == 2
                        select tt;
            ViewBag.listSP = lstSP;

            var lstNGK = from tt in db.SanPhams
                         where tt.MaLoaiSP == 1
                         select tt;
            ViewBag.listNGK = lstNGK;
            return View();
        }

        [ChildActionOnly]
        public ActionResult SanPhamPartial_content_1()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamPartial_content_2()
        {

            return PartialView();
        }

        public ActionResult MenuPartial()
        {
            var lstsp = db.SanPhams;
            return PartialView(lstsp);
        }

        public ActionResult Detail(int id)
        {
            var D_sach = db.SanPhams.Where(m => m.MaSP == id).First();
            return View(D_sach);
        }
    }
}