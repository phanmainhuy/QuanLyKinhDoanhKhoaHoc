using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class OwnCourseVM
    {
        public int MaKhoaHoc { get; set; }
        public string TenKhoaHoc { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongMua { get; set; }
        public DateTime NgayTao { get; set; }
    }
}