using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class KhuyenMaiVM
    {
        public int MaKM { get; set; }
        public string TenKM { get; set; }
        public int MaNguoiTao { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaTri { get; set; }
        public int DiemCanMua { get; set; }
        public int ThoiGianKeoDai { get; set; }
    }
}