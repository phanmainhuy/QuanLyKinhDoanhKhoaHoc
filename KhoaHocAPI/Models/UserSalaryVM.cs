using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class UserSalaryVM
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public bool Status { get; set; }
        public string HinhAnh { get; set; }
        public Decimal Salary { get; set; }
        public Decimal SalaryFine{ get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}