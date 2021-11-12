using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class OtherMapper
    {
        public static UserGroupVM MapUserGroup(NhomNguoiDung nhomnguoidung)
        {
            return new UserGroupVM()
            {
                MaNhomNguoiDung = nhomnguoidung.MaNhomNguoiDung,
                TenNhomNguoiDung= nhomnguoidung.TenNhomNguoiDung
            };
        }
        public static IEnumerable<UserGroupVM> MapListUserGroup(IEnumerable<NhomNguoiDung> lstNhomNguoiDung)
        {
            List<UserGroupVM> lstReturn = new List<UserGroupVM>();
            foreach(var item in lstNhomNguoiDung.ToList())
            {
                lstReturn.Add(MapUserGroup(item));
            }
            return lstReturn;
        }
        public static NhomNguoiDung MapUserGroupReverse(UserGroupVM model)
        {
            return new NhomNguoiDung()
            {
                MaNhomNguoiDung = model.MaNhomNguoiDung,
                TenNhomNguoiDung = model.TenNhomNguoiDung
            };
        }
        public static IEnumerable<NhomNguoiDung> MapListUserGroupReverse(IEnumerable<UserGroupVM> lstNhomNguoiDung)
        {
            List<NhomNguoiDung> lstReturn = new List<NhomNguoiDung>();
            foreach (var item in lstNhomNguoiDung.ToList())
            {
                lstReturn.Add(MapUserGroupReverse(item));
            }
            return lstReturn;
        }
    }
}