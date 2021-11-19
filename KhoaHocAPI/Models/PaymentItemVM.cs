using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class PaymentItemVM
    {
        public int PayMentID { get; set; }
        public int CourseID { get; set; }
        public int ItemID { get; set; }
        public string CourseName { get; set; }
        public decimal LastPrice { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string ImageName { get; set; }
    }
}