using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class PermissionGroupVM
    {
        public int MaNhomNguoiDung { get; set; }
        public string TenNhomNguoiDung { get; set; }
        public List<QuyenVM> DanhSachQuyen { get; set; }
    }
}