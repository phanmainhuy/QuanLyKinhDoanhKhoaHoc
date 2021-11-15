using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class MenuTypeMapper
    {
        public static MenuTypeVM MapMenuType(LOAIQUYEN loaiQuyens)
        {
            return new MenuTypeVM()
            {
                MenuID = loaiQuyens.MaLoaiQuyen,
                MenuName = loaiQuyens.TenLoaiQuyen,
                RoleList = Mapper.RoleMapper.MapListRole(loaiQuyens.Quyens).ToList()
            };
        }
        public static IEnumerable<MenuTypeVM> MapListMenuType(IEnumerable<LOAIQUYEN> ListLoaiQuyen)
        {
            List<MenuTypeVM> lstReturn = new List<MenuTypeVM>();
            foreach(var item in ListLoaiQuyen.ToList())
            {
                lstReturn.Add(MapMenuType(item));
            }
            return lstReturn;
        }
    }
}