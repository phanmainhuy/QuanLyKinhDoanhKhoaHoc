using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class ChuongVM
    {
        public int MaChuong { get; set; }
        public int MaKhoaHoc { get; set; }
        public string TenChuong { get; set; }
        public string TenKhoaHoc { get; set; }
        public List<BaiHocVM> DanhSachBaiHoc { get; set; }
    }
}