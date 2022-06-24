using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VSLT_FastfoodTeam.Models;

namespace VSLT_FastfoodTeam.Models
{
    public class GioHang
    {
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal thanhtien
        {
            get
            {
                return SoLuong * DonGia;
            }
        }
        public string HinhAnh { get; set; }

        public GioHang(int imasp)
        {
            using (MyDataDataContext db = new MyDataDataContext())
            {
                this.MaSP = imasp;
                this.TenSP = db.SanPhams.Single(n => n.MaSP == imasp).Tensp;
                this.HinhAnh = db.SanPhams.Single(n => n.MaSP == imasp).Hinhanh;
                this.DonGia = db.SanPhams.Single(n => n.MaSP == imasp).Dongia.Value;
                this.SoLuong = 1;
            }
        }
    }
}