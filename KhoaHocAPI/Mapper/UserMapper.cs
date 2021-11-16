using KhoaHocAPI.Models;
using KhoaHocData.DAO;
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
                Gender = "Nam",
                GroupID = nd.MaNhomNguoiDung.Value,
                GroupName = new UserGroupDAO().LayNhomTheoMa(nd.MaNhomNguoiDung.Value).TenNhomNguoiDung,
                Number = nd.SDT,
                Status = nd.TrangThai.Value
            };
            return user;
        }
        public static IEnumerable<UserViewModel> MapListUser(IEnumerable<NguoiDung> nguoidungs)
        {
            List<UserViewModel> lstReturn = new List<UserViewModel>();
            foreach(var item in nguoidungs)
            {
                lstReturn.Add(MapUser(item));
            }
            return lstReturn;
        }
        public static NguoiDung MapUserReverse(UserViewModel model)
        {
            return new NguoiDung()
            {
                MaND =  model.UserId,
                HoTen = model.UserName,
                Email = model.Email,
                Diachi = model.Address,
                NgaySinh = model.DoB,
                MaNhomNguoiDung = model.GroupID,
                SDT = model.Number
            };
        }
        public static IEnumerable<NguoiDung> MapListUserReverse(IEnumerable<UserViewModel> nguoidungs)
        {
            List<NguoiDung> lstReturn = new List<NguoiDung>();
            foreach (var item in nguoidungs)
            {
                lstReturn.Add(MapUserReverse(item));
            }
            return lstReturn;
        }
    }
}