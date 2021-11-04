using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class CourseMapper
    {
        public static IEnumerable<CourseVM> MapListCourse(IEnumerable<KhoaHoc> lstKhoaHoc)
        {
            List<CourseVM> lstReturn = new List<CourseVM>();
            foreach(var item in lstKhoaHoc)
            {
                var course = MapCourse(item);
                lstReturn.Add(course);
            }
            return lstReturn;
        }
        public static CourseVM MapCourse(KhoaHoc item)
        {
            GetDAO getDAODB = new GetDAO();
            return new CourseVM()
            {
                DonGia = item.DonGia.Value,
                HinhAnh = item.HinhAnh,
                MaGV = item.MaGV.Value,
                MaKhoaHoc = item.MaKhoaHoc,
                MaLoai = item.MaLoai.Value,
                SoLuongMua = item.SoLuongMua.Value,
                TenKhoaHoc = item.TenKhoaHoc,
                TrangThai = item.TrangThai.Value,
                DanhGia = getDAODB.GetDanhGiaKhoaHoc(item.MaKhoaHoc),
                GioiThieu = item.MOTAKHOAHOC,
                TenGV = item.NguoiDung.HoTen
            };
        }
    }
}