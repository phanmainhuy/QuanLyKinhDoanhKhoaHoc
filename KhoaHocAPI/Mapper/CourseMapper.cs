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
        public static IEnumerable<KhoaHoc> MapListCourseReverse(IEnumerable<CourseVM> lstKhoaHoc)
        {
            List<KhoaHoc> lstReturn = new List<KhoaHoc>();
            foreach (var item in lstKhoaHoc)
            {
                var course = MapCourseReverse(item);
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
                TenLoai = item.LoaiKhoaHoc.TenLoai,
                TenDanhMuc = item.LoaiKhoaHoc.DanhMucKhoaHoc.TenDanhMuc,
                TrangThai = item.TrangThai.Value,
                DanhGia = getDAODB.GetDanhGiaKhoaHoc(item.MaKhoaHoc),
                GioiThieu = item.MOTAKHOAHOC,
                TenGV = item.NguoiDung.HoTen,
                NgayTao = item.NgayTao.Value,
                NgayChapThuan = item.NgayChapThuan.Value,
                MaDM = item.LoaiKhoaHoc.MaDanhMuc,
                HienThi = item.HienThi
            };
        }
        public static KhoaHoc MapCourseReverse(CourseVM item)
        {
            GetDAO getDAODB = new GetDAO();
            return new KhoaHoc()
            {
                DonGia = item.DonGia,
                HinhAnh = item.HinhAnh,
                MaGV = item.MaGV,
                MaKhoaHoc = item.MaKhoaHoc,
                MaLoai = item.MaLoai,
                SoLuongMua = item.SoLuongMua,
                TenKhoaHoc = item.TenKhoaHoc,
                TrangThai = item.TrangThai,
                MOTAKHOAHOC = item.GioiThieu,
                NgayTao = item.NgayTao.Value,
                NgayChapThuan = item.NgayChapThuan.Value,
                HienThi = item.HienThi.Value
            };
        }
    }
}