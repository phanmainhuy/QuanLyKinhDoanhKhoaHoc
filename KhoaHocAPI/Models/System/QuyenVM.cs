using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class QuyenVM
    {
        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public string MetaLink { get; set; }
        public string Icon { get; set; }
        public bool HienThi { get; set; }
    }
}