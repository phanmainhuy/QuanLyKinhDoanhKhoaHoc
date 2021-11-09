using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class PaymentItem
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public decimal OriginPrice { get; set; }
        public decimal AfterPrice { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string ImageName { get; set; }
    }
}