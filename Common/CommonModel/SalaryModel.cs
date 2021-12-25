using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonModel
{
    public class SalaryModel
    {
        public int MaLuong { get; set; }
        public int MaND { get; set; }
        public string TenND { get; set; }
        public string TenChucVu { get; set; }
        public DateTime NgayPhatLuong { get; set; }
        public decimal TongTien { get; set; }
        public decimal TienPhat { get; set; }
        public decimal SoTien { get; set; }
        public string GhiChu { get; set; }
    }
}
