using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Models
{
    public class UserLogin
    {
        [Required]
        [MinLength(6, ErrorMessage ="Độ dài tài khoản phải lớn hơn 6 kí tự")]
        public string UserName { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Độ dài tài khoản phải lớn hơn 6 kí tự")]
        public string Password { get; set; }
    }
}