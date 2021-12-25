using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class DonThuHoVM
    {
        public int MaKH { get; set; }
        public int MaHD { get; set; }
        public int? MaNV { get; set; }
        public string DiaChiThu { get; set; }
        public string SDTThu { get; set; }
        public string DonViThuHo { get; set; }
        public double SoTienThu { get; set; }
        public double PhiThuHo { get; set; }
        public DateTime? NgayDuKienThu { get; set; }
        public string GhiChu { get; set; }
        public string MaApDung { get; set; }
        public string Email { get; set; }
    }
}