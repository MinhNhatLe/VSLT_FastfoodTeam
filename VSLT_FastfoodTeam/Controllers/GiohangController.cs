using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VSLT_FastfoodTeam.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace VSLT_FastfoodTeam.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstgiohang = Session["Giohang"] as List<GioHang>;
            if (lstgiohang == null)
            {
                lstgiohang = new List<GioHang>();
                Session["Giohang"] = lstgiohang;
            }
            return lstgiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> lstGioHang = LayGioHang();
            var sach = data.SanPhams.FirstOrDefault(p => p.MaSP == id);
            GioHang sanpham = lstGioHang.Find(n => n.MaSP == id);
            if (sanpham == null)
            {
                if (sach.Soluongton == 0)
                {
                    MessageBox.Show("Hết hàng");
                    return Redirect(strURL);
                }
                else
                {
                    sanpham = new GioHang(id);
                    lstGioHang.Add(sanpham);
                    return Redirect(strURL);
                }
            }
            else
            {
                // Nếu số lượng nhỏ hơn số lượng tồn thì tăng số lượng lên 1
                if (sanpham.SoLuong < sach.Soluongton)
                {
                    sanpham.SoLuong++;
                    return Redirect(strURL);
                }
                else
                // nếu số lượng mua lớn hơn số lượng tồn thì in thông báo
                {
                    MessageBox.Show("Số lượng đã vượt quá số lượng tồn!!!");
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.SoLuong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private decimal TongTien()
        {
            decimal tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.thanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.MaSP == id);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.MaSP == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> lstGioHang = LayGioHang();
            var sach = data.SanPhams.FirstOrDefault(p => p.MaSP == id);
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.MaSP == id);
            if (sanpham != null)
            {
                sanpham.SoLuong = int.Parse(collection["txtsl"].ToString());
                if (sanpham.SoLuong > sach.Soluongton)
                {
                    MessageBox.Show("Số lượng đã vượt quá số lượng tồn!!!");
                    sanpham.SoLuong = 1;                 
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
           
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("SanPham", "Food");
            }
            var tempx = Session["TaiKhoan"] as TaiKhoan;
            var tempb = data.KhachHangs.Where(x => x.MaTK == tempx.MaTK).FirstOrDefault();
            Session["KhachHang"] = tempb;
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongluongsanpham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DonDatHang dh = new DonDatHang();
            var tempx = Session["TaiKhoan"] as TaiKhoan;
            var tempb = data.KhachHangs.Where(x => x.MaTK == tempx.MaTK).FirstOrDefault();
            Session["KhachHang"] = tempb;
            SanPham s = new SanPham();
            List<GioHang> gh = LayGioHang();
            var ngaygiao = String.Format("{0: dd/MM/yyyy}", collection["NgayGiao"]);

            dh.MaKH = tempb.MaKH;
            dh.Ngaydat = DateTime.Now;
            dh.Ngaygiao = DateTime.Parse(ngaygiao);        
            data.DonDatHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                CTDDH ctdh = new CTDDH();
                ctdh.MaDDH = dh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.soluong = item.SoLuong;
                ctdh.Dongia = (int)item.DonGia;
                s = data.SanPhams.Single(n => n.MaSP == item.MaSP);
                var thaydoislton = s.Soluongton - ctdh.soluong;
                s.Soluongton = thaydoislton;
                data.SubmitChanges();
                data.CTDDHs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        public JsonResult SendMailToUser()
        {
            bool result = false;           
            result = SendEmail("lehuuvang1@gmail.com", "Kính Chào Qúy Khách", "<p> Hi bạn!!!<br />Chúng tôi sẽ giao hàng nhanh nhất có thể vui lòng chuẩn bị tiền mặt!!!!<br />Cảm ơn bạn đã đặt hàng của chúng tôi!!! <br />Have a nice day</p>");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public bool SendEmail(string toEmail, string subject, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}