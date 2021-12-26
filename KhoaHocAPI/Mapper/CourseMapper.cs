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
        public static BoughtCourseVM MapBoughtCourse(KhoaHoc item, int MaND)
        {
            GetDAO getDAODB = new GetDAO();
            var date = getDAODB.GetNgayTaoHoaDon(MaND, item.MaKhoaHoc);

            return new BoughtCourseVM()
            {
                DonGia = item.DonGia.Value,
                HinhAnh = item.HinhAnh,
                MaGV = item.MaGV.Value,
                MaKhoaHoc = item.MaKhoaHoc,
                TenKhoaHoc = item.TenKhoaHoc,
                TrangThai = item.TrangThai.Value,
                DanhGia = getDAODB.GetDanhGiaKhoaHoc(item.MaKhoaHoc),
                GioiThieu = item.MOTAKHOAHOC,
                TenGV = item.NguoiDung.HoTen,
                HienThi = item.HienThi,
                NgayMua = date == null? DateTime.MinValue: date.Value
            };
        }
        public static IEnumerable<BoughtCourseVM> MapListBoughtCourse(IEnumerable<KhoaHoc> lstKhoaHoc, int MaND)
        {
            List<BoughtCourseVM> lstReturn = new List<BoughtCourseVM>();
            foreach (var item in lstKhoaHoc.ToList())
            {
                lstReturn.Add(MapBoughtCourse(item, MaND));
            }
            return lstReturn;
        }
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
                HienThi = item.HienThi == null?false: item.HienThi,
                SoLuongMua = item.SoLuongMua == null?0:item.SoLuongMua.Value
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
                TenKhoaHoc = item.TenKhoaHoc,
                TrangThai = item.TrangThai,
                MOTAKHOAHOC = item.GioiThieu,
                NgayTao = item.NgayTao.Value,
                NgayChapThuan = item.NgayChapThuan.Value,
                HienThi = item.HienThi == null ? false: item.HienThi.Value
            };
        }
    }
}