using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Controllers
{
    public class UserController : Controller
    {
        MyDataDataContext mydata = new MyDataDataContext();
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, TaiKhoan tk, KhachHang kh)
        {
            var tentk = collection["TenTK"];
            var matkhau = collection["matkhau"];
            var MatkhauXacNhan = collection["MatkhauXacNhan"];
            var email = collection["email"];
            var tenkh = collection["TenKH"];
            var sdt = collection["SDT"];
            var diachi = collection["DiaChi"];

            var checktk = mydata.TaiKhoans.Where(n => n.TenTK == tentk).FirstOrDefault();
            var checkmail = mydata.TaiKhoans.Where(n => n.Email == email).FirstOrDefault();

            if (String.IsNullOrEmpty(MatkhauXacNhan))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if (matkhau != MatkhauXacNhan)
                {
                    ViewData["MatkhauGiongNhau"] = "Mật khẩu và mật tkẩu xác nhận phải giống nhau";
                }
                else
                {
                    if (checktk != null) ViewData["taikhoantontai"] = "Tên tài khoản đã tồn tại! vui lòng chọn tên tài khoản khác";
                    else
                    {
                        if (checkmail != null) ViewData["mailtontai"] = "Email đã được sử dụng! vui lòng chọn Email khác";
                        else
                        {
                            tk = new TaiKhoan();
                            tk.TenTK = tentk;
                            tk.MatKhau = MatkhauXacNhan;
                            tk.Email = email;
                            tk.LoaiTK = false;
                            mydata.TaiKhoans.InsertOnSubmit(tk);

                            mydata.SubmitChanges();
                            kh = new KhachHang();
                            kh.TenKH = tenkh;
                            kh.SDT = sdt;
                            kh.DiaChi = diachi;
                            kh.MaTK = tk.MaTK;
                            mydata.KhachHangs.InsertOnSubmit(kh);
                            mydata.SubmitChanges();

                            return RedirectToAction("DangNhap", "User");
                        }
                    }
                }
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tentk = collection["TenTK"];
            var mattkhau = collection["MatKhau"];

            TaiKhoan tk = mydata.TaiKhoans.Where(x => x.TenTK == tentk && x.MatKhau == mattkhau).FirstOrDefault();
            if (tk != null)
            {
                if(tk.LoaiTK == true)
                {
                    ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công!";
                    Session["TaiKhoan"] = tk;
                    return RedirectToAction("/admin/Index", "admin");
                }                
                else
                {
                    ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công!";
                    Session["TaiKhoan"] = tk;
                    return RedirectToAction("Index", "LayoutMain");
                }
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                return this.DangNhap();
            }

        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "LayoutMain");
          

        }
    }
}