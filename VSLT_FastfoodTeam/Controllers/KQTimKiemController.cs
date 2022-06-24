using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class KQTimKiemController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: KQTimKiem
        public ActionResult KQTimKiem(string SearchString)
        {
            var item = db.SanPhams.Where(n => n.Tensp.Contains(SearchString));
            return View(item.OrderBy(n => n.Tensp));
        }
    }
}