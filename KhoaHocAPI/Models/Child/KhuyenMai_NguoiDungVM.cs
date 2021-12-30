using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class KhuyenMai_NguoiDungVM
    {
        public int MaKM { get; set; }
        public int MaHV { get; set; }
        public string TenKM { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaTri { get; set; }
        public string MaApDung { get; set; }
        public DateTime HanSuDung { get; set; }
    }
}