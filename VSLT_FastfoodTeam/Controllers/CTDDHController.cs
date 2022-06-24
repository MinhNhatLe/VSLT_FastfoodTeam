using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class CTDDHController : Controller
    {
        // GET: CTDDH
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page, string searching)
        {
            if (page == null)
                page = 1;


            if (searching == null)
            {
                var all_unit = db.CTDDHs.ToList().OrderBy(m => m.MaDDH);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.CTDDHs.Where(x => x.SanPham.Tensp.Contains(searching)).ToList().OrderBy(m => m.MaSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }
    }
}