using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class TheLoaiVM
    {
        public int MaDM { get; set; }
        public int MaTheLoai { get; set; }
        public string  TenTheLoai { get; set; }
        public int SoLuongKhoaHoc { get; set; }
        public bool HienThi { get; set; }
    }
}