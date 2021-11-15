using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class MenuTypeVM
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public List<QuyenVM> RoleList { get; set; }
    }
}