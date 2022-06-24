using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class FoodController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page, string searching)
        {
            if (page == null)
                page = 1;

            if (searching == null)
            {
                var all_unit = db.SanPhams.ToList().OrderBy(m => m.MaSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.SanPhams.Where(x => x.Tensp.Contains(searching)).ToList().OrderBy(m => m.MaSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult Detail(int id)
        {
            var D_theloai = db.SanPhams.Where(m => m.MaSP == id).First();
            return View(D_theloai);
        }
        //-------------Create-------------------
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, SanPham tl)
        {
            var masp = collection["MaSP"];
            var tensp = collection["Tensp"];
            var dongia = collection["Dongia"];
            var size = collection["Size"];
            var hinhanh = collection["Hinhanh"];
            var soluongton = collection["Soluongton"];
            var Mansx = collection["MaNSX"];
            var Mancc = collection["MaNCC"];
            var MaLoaisp = collection["MaLoaiSP"];
            if (string.IsNullOrEmpty(tensp))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                tl.MaSP = int.Parse(masp);
                tl.Tensp = tensp;
                tl.Dongia = int.Parse(dongia);
                tl.Size = size;
                tl.Hinhanh = hinhanh;
                tl.Soluongton = int.Parse(soluongton);
                tl.MaNCC = Mancc;
                tl.MaLoaiSP = int.Parse(MaLoaisp);
                db.SanPhams.InsertOnSubmit(tl);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }
        //-------------Edit-------------------
        public ActionResult Edit(int id)
        {
            var tl = db.SanPhams.First(m => m.MaSP == id);
            return View(tl);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var tl = db.SanPhams.First(m => m.MaSP == id);
            var masp = collection["MaSP"];
            var tensp = collection["Tensp"];
            var dongia = collection["Dongia"];
            var size = collection["Size"];
            var hinhanh = collection["Hinhanh"];
            var soluongton = collection["Soluongton"];
            var Mancc = collection["MaNCC"];
            var MaLoaisp = collection["MaLoaiSP"];
            tl.MaSP = id;
            if (string.IsNullOrEmpty(masp) || string.IsNullOrEmpty(tensp) || string.IsNullOrEmpty(dongia) || string.IsNullOrEmpty(size) || string.IsNullOrEmpty(hinhanh) || string.IsNullOrEmpty(soluongton) || string.IsNullOrEmpty(Mancc) || string.IsNullOrEmpty(MaLoaisp))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                //tl.MaSP = int.Parse(masp);
                tl.Tensp = tensp;
                tl.Dongia = int.Parse(dongia);
                tl.Size = size;
                tl.Hinhanh = hinhanh;
                tl.Soluongton = int.Parse(soluongton);             
                tl.MaNCC = Mancc;
                tl.MaLoaiSP = int.Parse(MaLoaisp);
                UpdateModel(tl);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }
        //-------------Delete-------------------
        public ActionResult Delete(int id)
        {
            var D_theloai = db.SanPhams.First(m => m.MaSP == id);
            return View(D_theloai);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_theloai = db.SanPhams.Where(m => m.MaSP == id).First();
            db.SanPhams.DeleteOnSubmit(D_theloai);
            db.SubmitChanges();
            return RedirectToAction("ListSinhVien");
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        public ActionResult SanPham(int? page, string searching)
        {
            if (page == null)
                page = 1;
                
            if (searching == null)
            {
                var all_unit = db.SanPhams.ToList().OrderBy(m => m.MaSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.SanPhams.Where(x => x.Tensp.Contains(searching)).ToList().OrderBy(m => m.MaSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult SanPham2(int? page, string searching)
        {
            if (page == null)
                page = 1;

            if (searching == null)
            {
                var all_unit = db.SanPhams.ToList().OrderBy(m => m.MaSP).Where(P => P.MaLoaiSP ==2);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.SanPhams.Where(x => x.Tensp.Contains(searching)).ToList().OrderBy(m => m.MaSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        
    }
}