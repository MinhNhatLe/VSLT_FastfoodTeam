using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Areas.admin.Controllers
{
    public class CustomerController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();

        // GET: admin/Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Customer(int? page, string searching)
        {
            if (page == null)
                page = 1;
            if (searching == null)
            {
                var all_unit = db.KhachHangs.ToList().OrderBy(m => m.MaKH);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.KhachHangs.Where(x => x.TenKH.Contains(searching)).ToList().OrderBy(m => m.MaKH);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }

        public ActionResult CustomerOrder(int? page, string searching)
        {
            
            if (page == null)
                page = 1;
            if (searching == null)
            {
                var all_unit = db.cusorders.ToList();
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.cusorders.Where(x => x.TenKH.Contains(searching)).ToList();
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }
    }
}