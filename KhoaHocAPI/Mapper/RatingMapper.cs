using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class RatingMapper
    {
       static GetDAO db = new GetDAO();
        public static RatingVM MapRating(DanhGiaKhoaHoc danhgia)
        {
            return new RatingVM()
            {
                MaKhoaHoc = danhgia.MaKhoaHoc,
                MaND = danhgia.MaND.Value,
                TenND = db.GetTenNguoiDung(danhgia.MaND.Value),
                TongDiem = db.GetDanhGiaKhoaHoc(danhgia.MaKhoaHoc),
                HinhAnh = db.GetHinhAnhNguoiDung(danhgia.MaND.Value),
                NoiDung = danhgia.NoiDung,
                TenKhoaHoc = db.GetTenKhoaHoc(danhgia.MaKhoaHoc)
            };
        }
        public static IEnumerable<RatingVM> MapListRating(IEnumerable<DanhGiaKhoaHoc> lstDanhGia)
        {
            List<RatingVM> lstReturn = new List<RatingVM>();
            foreach (var item in lstDanhGia.ToList())
            {
                lstReturn.Add(MapRating(item));
            }
            return lstReturn;
        }
    }
}