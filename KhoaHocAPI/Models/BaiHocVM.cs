using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class BaiHocVM
    {
        public int MaBaiHoc { get; set; }
        public string TenBaiHoc { get; set; }
        public int MaChuong { get; set; }
        public string TenChuong { get; set; }
        public string VideoName { get; set; }
    }
}