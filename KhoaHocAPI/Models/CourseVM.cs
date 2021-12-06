using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class CourseVM
    {
        public int MaKhoaHoc { get; set; }
        public int? MaLoai { get; set; }//
        public int? MaDM { get; set; }
        public string TenLoai { get; set; }//
        public string TenDanhMuc { get; set; }//
        public string TenKhoaHoc { get; set; }//
        public decimal? DonGia { get; set; }//
        public int SoLuongMua { get; set; }
        public bool TrangThai { get; set; }
        public string HinhAnh { get; set; }//
        public int? MaGV { get; set; }//
        public string TenGV { get; set; }
        public decimal DanhGia { get; set; }
        public string GioiThieu { get; set; }//
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayChapThuan { get; set; }
    }
}