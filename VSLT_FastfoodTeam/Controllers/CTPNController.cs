using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class CTPNController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page, string searching)
        {
            if (page == null)
                page = 1;


            if (searching == null)
            {
                var all_unit = db.CTPNs.ToList().OrderBy(m => m.Maphieu);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.CTPNs.Where(x => x.SanPham.Tensp.Contains(searching)).ToList().OrderBy(m => m.MaSP);
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
        public ActionResult Create(FormCollection collection, CTPN tl, PhieuNhap pn)
        {
            var masp = collection["MaSP"];
            var giamua = collection["Giamua"];
            var soluong = collection["Soluong"];

            var checkmasp = db.SanPhams.Where(m => m.MaSP == int.Parse(masp)).FirstOrDefault();
            if (checkmasp == null) { ViewData["maspkotontai"] = "Bạn đã nhập sai sản phẩm bạn"; }
            else
            {
                pn = new PhieuNhap();
                pn.Ngaynhap = DateTime.Today;
                db.PhieuNhaps.InsertOnSubmit(pn);
                db.SubmitChanges();
                tl.MaSP = int.Parse(masp);
                tl.Maphieu = pn.Maphieu;
                tl.Giamua = int.Parse(giamua);
                tl.Soluong = int.Parse(soluong);
                db.CTPNs.InsertOnSubmit(tl);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();


        }
    }
}