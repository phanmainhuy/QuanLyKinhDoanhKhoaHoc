using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class SalaryHistoryItemVM
    {
        public int MaLuong { get; set; }
        public DateTime NgayPhatLuong { get; set; }
        public decimal SoTien { get; set; }
        public decimal TienPhat { get; set; }
        public string GhiChu { get; set; }
    }
}