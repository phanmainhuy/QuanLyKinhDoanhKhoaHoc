using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class RatingMapper
    {
       static GetDAO db = new GetDAO();
        public static async Task<RatingVM> MapRating(DanhGiaKhoaHoc danhgia)
        {
            return new RatingVM()
            {
                MaKhoaHoc = danhgia.MaKhoaHoc,
                MaND = danhgia.MaND,
                TenND = await db.GetTenNguoiDung(danhgia.MaND),
                TongDiem = db.GetDanhGiaKhoaHoc(danhgia.MaKhoaHoc),
                Diem = danhgia.Diem == null?0:danhgia.Diem.Value,
                HinhAnh = db.GetHinhAnhNguoiDung(danhgia.MaND),
                NoiDung = danhgia.NoiDung,
                TenKhoaHoc = await  db.GetTenKhoaHoc(danhgia.MaKhoaHoc),
                NgayDanhGia = danhgia.NgayDanhGia,
                MaDanhGia = danhgia.MaDanhGia
            };
        }
        public static async Task<IEnumerable<RatingVM>> MapListRating(IEnumerable<DanhGiaKhoaHoc> lstDanhGia)
        {
            List<RatingVM> lstReturn = new List<RatingVM>();
            foreach (var item in lstDanhGia.ToList())
            {
                lstReturn.Add(await MapRating(item));
            }
            return lstReturn;
        }
    }
}