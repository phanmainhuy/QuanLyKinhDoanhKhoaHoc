using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class DanhMucVM
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string  HinhAnh { get; set; }
        public int TongSoKhoaHoc { get; set; }
        public List<TheLoaiVM> DanhSachTheLoai { get; set; }
    }
}