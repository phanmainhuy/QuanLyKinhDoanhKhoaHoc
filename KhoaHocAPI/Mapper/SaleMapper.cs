using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class SaleMapper
    {
        public static KhuyenMaiVM MapSale(KhuyenMai khuyenMai)
        {
            return new KhuyenMaiVM()
            {
                MaKM = khuyenMai.MaKM,
                TenKM = khuyenMai.TenKM,
                GiaTri = khuyenMai.GiaTri.Value,
                HinhAnh = khuyenMai.HinhAnh,
                MaNguoiTao = khuyenMai.MaND.Value,
                DiemCanMua = khuyenMai.Diem.Value,
                ThoiGianKeoDai = khuyenMai.ThoiGianKeoDai.Value
            };
        }
        public static IEnumerable<KhuyenMaiVM> MapListSale(IEnumerable<KhuyenMai> lstKhuyenMai)
        {
            List<KhuyenMaiVM> lstReturn = new List<KhuyenMaiVM>();
            foreach (var item in lstKhuyenMai.ToList())
            {
                lstReturn.Add(MapSale(item));
            }
            return lstReturn;   
        }
        public static KhuyenMai_NguoiDungVM MapSaleBought(KhuyenMai khuyenMai)
        {
            var kmkh = khuyenMai.KhuyenMai_KhachHang.
                SingleOrDefault(x => x.MaND == khuyenMai.MaND && (x.IsSuDung == false || x.IsSuDung == null));
            var hsd = DateTime.Today.AddDays(-1);
            if(kmkh != null)
                hsd = kmkh.NgayKetThuc.Value;
            return new KhuyenMai_NguoiDungVM()
            {
                MaKM = khuyenMai.MaKM,
                TenKM = khuyenMai.TenKM,
                GiaTri = khuyenMai.GiaTri.Value,
                HinhAnh = khuyenMai.HinhAnh,
                MaApDung = khuyenMai.MaApDung,
                MaHV = khuyenMai.MaND.Value,
                HanSuDung = hsd
            };
        }
        public static IEnumerable<KhuyenMai_NguoiDungVM> MapListSaleBought(IEnumerable<KhuyenMai> lstKhuyenMai)
        {
            List<KhuyenMai_NguoiDungVM> lstReturn = new List<KhuyenMai_NguoiDungVM>();
            foreach (var item in lstKhuyenMai.ToList())
            {
                lstReturn.Add(MapSaleBought(item));
            }
            return lstReturn;
        }
    }
}