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
                DiemCanMua = khuyenMai.Diem.Value
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
    }
}