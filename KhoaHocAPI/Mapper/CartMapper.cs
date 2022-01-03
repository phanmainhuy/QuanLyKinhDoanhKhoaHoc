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
    public static class CartMapper
    {
        public static async Task<CourseCartVM> MapCourseCart(GioHang gh)
        {
            GetDAO getDAODB = new GetDAO();
            if (gh != null)
            {
                CourseCartVM CartVM = new CourseCartVM();
                CartVM.CourseCartID = gh.MaGioHang;
                CartVM.UserID = gh.MaND.Value;
                CartVM.TongTien = gh.TongTien == null? 0:gh.TongTien.Value;
                CartVM.CartItems = new List<CartItemVM>();
                foreach (var item in new CartDAO().LayDanhSachTrongGioHang(gh.MaGioHang))
                {
                    var tenKhoaHoc = await getDAODB.GetKhoaHoc(item.MaKhoaHoc);

                    CartItemVM CItemVM = new CartItemVM()
                    {
                        CourseID = item.MaKhoaHoc,
                        CourseName = tenKhoaHoc.TenKhoaHoc == null? "": tenKhoaHoc.TenKhoaHoc,
                        TeacherId = getDAODB.GetGiaoVienTheoMa(tenKhoaHoc.MaGV.Value).MaND,
                        TeacherName = getDAODB.GetGiaoVienTheoMa(tenKhoaHoc.MaGV.Value).HoTen,
                        AfterPrice = tenKhoaHoc.DonGia == null ? 0 : tenKhoaHoc.DonGia.Value,
                        OriginPrice = tenKhoaHoc.DonGia == null?0: tenKhoaHoc.DonGia.Value,
                        ImageName = tenKhoaHoc.HinhAnh,
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