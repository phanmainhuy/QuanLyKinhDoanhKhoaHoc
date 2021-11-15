using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class RoleMapper
    {
        public static QuyenVM MapRole(Quyen quyen)
        {
            return new QuyenVM()
            {
                MaQuyen = quyen.MaQuyen,
                TenQuyen = quyen.TenQuyen,
                MetaLink = quyen.MetaLink,
                Icon = quyen.icon
            };
        }
        public static IEnumerable<QuyenVM> MapListRole(IEnumerable<Quyen> quyens)
        {
            List<QuyenVM> lstReturn = new List<QuyenVM>();
            foreach(var item in quyens)
            {
                lstReturn.Add(MapRole(item));
            }
            return lstReturn;
        }
        public static Quyen MapQuyen(QuyenVM model, ICollection<NhomNguoiDung> pNhomNguoiDung = null)
        {
            return new Quyen()
            { 
                MaQuyen = model.MaQuyen,
                TenQuyen = model.TenQuyen
            };
        }
        public static IEnumerable<Quyen> MapListQuyen(IEnumerable<QuyenVM> quyenVMs)
        {
            List<Quyen> lstReturn = new List<Quyen>();
            foreach(var item in quyenVMs.ToList())
            {
                lstReturn.Add(MapQuyen(item));
            }
            return lstReturn;
        }

    }
}