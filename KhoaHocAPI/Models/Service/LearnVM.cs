using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class LearnVM
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public List<ChuongVM> DanhSachChuong { get; set; }
    }
}