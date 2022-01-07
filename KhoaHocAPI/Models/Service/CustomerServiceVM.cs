using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models.Service
{
    public class CustomerServiceVM
    {
        public int MaCSKH { get; set; }
        public int? MaLoaiVanDe { get; set; }
        public string TenLoaiVanDe { get; set; }
        public int? MaNV { get; set; }
        public string SDT { get; set; }
        public string TenKhachHang { get; set; }
        public string NoiDung { get; set; }
        public string CachXuLy { get; set; }
    }
}