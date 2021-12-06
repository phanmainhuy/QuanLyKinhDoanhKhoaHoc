using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DoB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public bool Status { get; set; }
        public string HinhAnh { get; set; }
        public int DiemTichLuy { get; set; }
        public string CMND { get; set; }
    }
}