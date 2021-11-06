using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class CourseCartVM
    {
        public int CourseCartID { get; set; }
        public int UserID { get; set; }
        public decimal TongTien { get; set; }
        public List<CartItemVM> CartItems{ get; set; }
    }
}