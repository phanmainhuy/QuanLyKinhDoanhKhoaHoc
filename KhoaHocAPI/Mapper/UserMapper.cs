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
                UserName = nd.TenDN
            };
            return user;
        }
        public static UserViewModel MapUser(NguoiDung nd)
        {
            int diem = 0;
            var Diem = new GetDAO().LayDiemTheoMaNguoiDung(nd.MaND);
            if (Diem != null)
                diem = Diem.Value;
            var luong = (decimal)0;
            var Luong = new GetDAO().LayLuongTheoMaNguoiDung(nd.MaND);
            if (Luong != null)
                luong = Luong.Value;

            UserViewModel user = new UserViewModel()
            {
                UserId = nd.MaND,
                UserName = nd.TenDN == null? "": nd.TenDN,
                Name = nd.HoTen == null ? "" : nd.HoTen,
                Email = nd.Email == null ? "" : nd.Email,
                Address = nd.Diachi == null? "": nd.Diachi,
                DoB = nd.NgaySinh == null? DateTime.MinValue:  nd.NgaySinh.Value,
                Gender = nd.GioiTinh == null?"":nd.GioiTinh,
                GroupID = nd.MaNhomNguoiDung.Value,
                GroupName = new UserGroupDAO().LayNhomTheoMa(nd.MaNhomNguoiDung.Value).TenNhomNguoiDung,
                Number = nd.SDT == null? "" : nd.SDT,
                Status = nd.TrangThai != null ? nd.TrangThai.Value : false,
                HinhAnh = nd.HinhAnh == null? "userdefault.png": nd.HinhAnh,
                DiemTichLuy = diem,
                CMND = nd.CMND == null? "": nd.CMND,
                Salary = luong
            };
            return user;
        }
        public static UserGroupVM MapUserGroup(NhomNguoiDung nhomnguoidung)
        {
            return new UserGroupVM()
            {
                MaNhomNguoiDung = nhomnguoidung.MaNhomNguoiDung,
                TenNhomNguoiDung = nhomnguoidung.TenNhomNguoiDung
            };
        }
        public static IEnumerable<UserGroupVM> MapListUserGroup(IEnumerable<NhomNguoiDung> lstNhomNguoiDung)
        {
            List<UserGroupVM> lstReturn = new List<UserGroupVM>();
            foreach (var item in lstNhomNguoiDung.ToList())
            {
                lstReturn.Add(MapUserGroup(item));
            }
            return lstReturn;
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
                SDT = model.Number,
                HinhAnh = model.HinhAnh,
                GioiTinh = model.Gender
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
        public static UserSalaryVM MapUserSalary(NguoiDung nguoidung)
        {
            GetDAO db = new GetDAO();
            var nhomNguoiDung = db.GetNhomNguoiDung(nguoidung.MaNhomNguoiDung.Value);
            var luong = db.GetLuong(nguoidung.MaND);
            return new UserSalaryVM()
            {
                UserId = nguoidung.MaND,
                GroupID = nhomNguoiDung.MaNhomNguoiDung,
                GroupName = nhomNguoiDung.TenNhomNguoiDung,
                HinhAnh = nguoidung.HinhAnh,
                Name = nguoidung.HoTen,
                Status = nguoidung.TrangThai.Value,
                Salary = luong.Luong1.Value
            };
        }
        public static IEnumerable<UserSalaryVM> MapListUserSalary(IEnumerable<NguoiDung> lstNguoiDung)
        {
            List<UserSalaryVM> lstReturn = new List<UserSalaryVM>();
            foreach (var item in lstNguoiDung.ToList())
            {
                lstReturn.Add(MapUserSalary(item));
            }
            return lstReturn;
        }
    }
}