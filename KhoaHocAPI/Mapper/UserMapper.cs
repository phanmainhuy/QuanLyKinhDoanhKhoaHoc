using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class UserMapper
    {
        public static UserLogon MapUserLogon(NguoiDung nd)
        {
            UserLogon user = new UserLogon()
            {
                UserID = nd.MaND,
                Name = nd.HoTen,
            };
            return user;
        }
        public static UserViewModel MapUser(NguoiDung nd)
        {
            UserViewModel user = new UserViewModel()
            {
                UserId = nd.MaND,
                UserName = nd.TenDN,
                Name = nd.HoTen,
                Email = nd.Email,
                Address = nd.Diachi,
                DoB = nd.NgaySinh.Value,
                Gender = "Nam"
            };
            return user;
        }
    }
}