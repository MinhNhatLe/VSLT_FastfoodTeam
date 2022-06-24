using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class FoodCategoryController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        public ActionResult Index(int? page, string searching)
        {
            if (page == null)
                page = 1;


            if (searching == null)
            {
                var all_unit = db.LoaiSPs.ToList().OrderBy(m => m.MaLoaiSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var all_unit = db.LoaiSPs.Where(x => x.TenLoai.Contains(searching)).ToList().OrderBy(m => m.MaLoaiSP);
                int pageSize = 10;
                int pageNum = page ?? 1;
                return View(all_unit.ToPagedList(pageNum, pageSize));
            }
        }
        //-------------Create-------------------
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, LoaiSP tl)
        {
            var tenloai = collection["TenLoai"];
            var icon = collection["Icon"];
            if (string.IsNullOrEmpty(tenloai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                tl.TenLoai = tenloai;
                tl.Icon = icon;
                db.LoaiSPs.InsertOnSubmit(tl);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }
        //-------------Edit-------------------
        public ActionResult Edit(int id)
        {
            var E_category = db.LoaiSPs.First(m => m.MaLoaiSP == id);
            return View(E_category);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var tl = db.LoaiSPs.First(m => m.MaLoaiSP == id);
            var tenloai = collection["TenLoai"];
            var icon = collection["Icon"];
            tl.TenLoai = tenloai;
            tl.Icon = icon;
            UpdateModel(tl);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        //-------------Delete-------------------
        public ActionResult Delete(int id)
        {
            var D_theloai = db.LoaiSPs.First(m => m.MaLoaiSP == id);
            return View(D_theloai);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_theloai = db.LoaiSPs.First(m => m.MaLoaiSP == id);
            db.LoaiSPs.DeleteOnSubmit(D_theloai);
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
    }
}