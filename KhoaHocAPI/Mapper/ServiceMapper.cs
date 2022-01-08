using KhoaHocAPI.Models.Other;
using KhoaHocAPI.Models.Service;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class ServiceMapper
    {
        public static async Task<CustomerServiceVM>  MapCustomerService(ChamSocKhachHang cskh)
        {
            GetDAO gd = new GetDAO();
            var nd = (await gd.GetTenNguoiDung(cskh.MaND.Value));
            return new CustomerServiceVM()
            {
                MaCSKH = cskh.MaCSKH,
                MaNV = cskh.MaND,
                CachXuLy = cskh.CachXuLy == null ? "" : cskh.CachXuLy,
                MaLoaiVanDe = cskh.MaLoaiVanDe,
                NoiDung = cskh.NoiDung == null ? "" : cskh.NoiDung,
                SDT = cskh.SDTKH,
                TenKhachHang = cskh.TenKH,
                TenLoaiVanDe = cskh.LoaiVanDe.TenLoaiVanDe,
                TenNhanVien = nd,
                NgayTuVan = cskh.NgayLap == null ? DateTime.MinValue : cskh.NgayLap.Value
            };
        }
        public static async Task<IEnumerable<CustomerServiceVM>> MapListCustomerService(IEnumerable<ChamSocKhachHang> lstModel)
        {
            List<CustomerServiceVM> lstReturn = new List<CustomerServiceVM>();
            foreach (var item in lstModel)
            {
                lstReturn.Add( await MapCustomerService(item));
            }
            return lstReturn;
        }
        public static LoaiVanDeVM MapProblem(LoaiVanDe lvd)
        {
            return new LoaiVanDeVM()
            {
                MaLoaiVanDe = lvd.MaLoaiVanDe,
                TenLoaiVanDe= lvd.TenLoaiVanDe == null? "": lvd.TenLoaiVanDe
            };
        }
        public static IEnumerable<LoaiVanDeVM> MapListProblem(IEnumerable<LoaiVanDe> lstModel)
        {
            List<LoaiVanDeVM> lstReturn = new List<LoaiVanDeVM>();
            foreach (var item in lstModel)
            {
                lstReturn.Add(MapProblem(item));
            }
            return lstReturn;
        }
            
    }
}