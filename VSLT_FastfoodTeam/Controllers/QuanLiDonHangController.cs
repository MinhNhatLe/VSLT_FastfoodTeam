using VSLT_FastfoodTeam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using PagedList;

namespace FastFood.Controllers
{
    public class QuanLiDonHangController : Controller
    {
        // GET: QuanLiDonHang
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult DaGiaoHang(int? page, string searching)
        {
            if (page == null)
                page = 1;

            if (searching == null)
            {
                var lstDSDHCG = db.DonDatHangs.Where(x => x.Tinhtrangdonhang == null).ToList().OrderBy(m => m.MaDDH);
                int pageSize = 8;
                int pageNum = page ?? 1;
                return View(lstDSDHCG.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var lstDSDHCG = db.DonDatHangs.Where(x => x.KhachHang.TenKH.Contains(searching) && x.Tinhtrangdonhang == null).ToList().OrderBy(m => m.MaDDH);
                int pageSize = 8;
                int pageNum = page ?? 1;
                return View(lstDSDHCG.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult ChuaGiaoHang(int? page, string searching)
        {
            if (page == null)
                page = 1;

            if (searching == null)
            {
                var lstDSDHCG = db.DonDatHangs.Where(x => x.Tinhtrangdonhang != null).ToList().OrderBy(m => m.MaDDH);
                int pageSize = 8;
                int pageNum = page ?? 1;
                return View(lstDSDHCG.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var lstDSDHCG = db.DonDatHangs.Where(x => x.KhachHang.TenKH.Contains(searching) && x.Tinhtrangdonhang != null).ToList().OrderBy(m => m.MaDDH);
                int pageSize = 8;
                int pageNum = page ?? 1;
                return View(lstDSDHCG.ToPagedList(pageNum, pageSize));
            }
        }
        [HttpGet]
        public ActionResult TimKiem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TimKiem(FormCollection collection)
        {
            var lst = new List<DonDatHang>();
            try
            {
                var option = collection["SearchString"];
                lst = db.DonDatHangs.Where(p => (p.MaDDH.ToString().ToLower().Contains(option.ToString().ToLower()))).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            return View(lst);
        }
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ViewBag.getid = ddhUpdate.MaDDH;
            ddhUpdate.Tinhtrangdonhang = ddh.Tinhtrangdonhang;
            db.SubmitChanges();
            var lstChiTietDH = db.CTDDHs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;


            return View(ddhUpdate);
        }
        [HttpGet]
        public ActionResult DDuyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.CTDDHs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            ViewBag.getid = id;
            return View(model);
        }

        public ActionResult Xem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.CTDDHs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            ViewBag.getid = id;
            return View(model);
        }



            public ActionResult InHoaDon(int madon)
        {
            var hd = from c in db.CTDDHs where (c.MaDDH == madon) select c;
            return View(hd);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Duyet(int id)
        {
            var find_x = db.DonDatHangs.Where(x => x.MaDDH == id).FirstOrDefault();
            find_x.Tinhtrangdonhang = false;
            UpdateModel(find_x);
            db.SubmitChanges();
            return RedirectToAction("DaGiaoHang");
        }
    }
}