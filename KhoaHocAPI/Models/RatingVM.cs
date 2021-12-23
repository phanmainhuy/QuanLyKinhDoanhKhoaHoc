using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class RatingVM
    {
        public int MaDanhGia { get; set; }
        public int MaND { get; set; }
        public int MaKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }
        public int Diem { get; set; }
        public decimal TongDiem { get; set; }
        public string NoiDung { get; set; }
        public string TenND { get; set; }
        public string HinhAnh { get; set; }
        public DateTime? NgayDanhGia { get; set; }
    }
}