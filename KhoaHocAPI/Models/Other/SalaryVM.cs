using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class SalaryVM
    {
        public int MaLuong { get; set; }
        public int MaND { get; set; }
        public decimal TongLuong { get; set; }
        public List<SalaryHistoryItemVM> DanhSachLichSu { get; set; }
    }
}