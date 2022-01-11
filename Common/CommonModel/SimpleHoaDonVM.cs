﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.CommonModel
{
    public class SimpleHoaDonVM
    {
        public string STT { get; set; }
        public int MaHoaDon { get; set; }
        public int MaND { get; set; }
        public string TenND { get; set; }
        public decimal TongThanhToan { get; set; }
        public DateTime NgayTaoHoaDon { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string TrangThai { get; set; }
        public int MaGioHang { get; set; }
        public int? MaKhoaHoc { get; set; }
    }
}