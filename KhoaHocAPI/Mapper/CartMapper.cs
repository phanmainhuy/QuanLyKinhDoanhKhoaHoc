using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class CartMapper
    {
        public static CourseCartVM MapCourseCart(GioHang gh)
        {
            GetDAO getDAODB = new GetDAO();
            if (gh != null)
            {
                CourseCartVM CartVM = new CourseCartVM();
                CartVM.CourseCartID = gh.MaGioHang;
                CartVM.UserID = gh.MaND.Value;
                CartVM.TongTien = gh.TongTien.Value;
                CartVM.CartItems = new List<CartItemVM>();
                foreach (var item in new CartDAO().LayDanhSachTrongGioHang(gh.MaGioHang))
                {
                    CartItemVM CItemVM = new CartItemVM()
                    {
                        CourseID = item.MaKhoaHoc,
                        CourseName = item.KhoaHoc.TenKhoaHoc,
                        TeacherId = getDAODB.GetGiaoVienTheoMa(item.KhoaHoc.MaGV.Value).MaND,
                        TeacherName = getDAODB.GetGiaoVienTheoMa(item.KhoaHoc.MaGV.Value).HoTen,
                        AfterPrice = item.KhoaHoc.DonGia.Value,
                        OriginPrice = item.KhoaHoc.DonGia.Value,
                        ImageName = item.KhoaHoc.HinhAnh,
                    };
                    CartVM.CartItems.Add(CItemVM);
                }
                return CartVM;
            }
            else
                return null;
        }
    }
}