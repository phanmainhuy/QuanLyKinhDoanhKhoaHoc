using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class HoaDonDuyetVM
    {
        public int MaHoaDon { get; set; }
        public int MaND { get; set; }
        public decimal TongThanhToan { get; set; }
        public DateTime NgayTaoHoaDon { get; set; }
        public string SDTDat { get; set; }
        public string DiaChiThuTien { get; set; }
        public bool TrangThai { get; set; }
        public List<PaymentItemVM> DanhSachKhoaHoc { get; set; }
    }
}