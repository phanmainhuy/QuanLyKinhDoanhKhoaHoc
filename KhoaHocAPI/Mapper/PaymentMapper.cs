using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System.Collections.Generic;
using System.Linq;

namespace KhoaHocAPI.Mapper
{
    public class PaymentMapper
    {
        private static GetDAO getDAODB = new GetDAO();

        public static PaymentItemVM MapPaymentItem(CT_HoaDon cthd)
        {
            return new PaymentItemVM()
            {
                CourseID = cthd.MaKhoaHoc.Value,
                ItemID = cthd.MaCTHD,
                CourseName = cthd.KhoaHoc.TenKhoaHoc,
                ImageName = cthd.KhoaHoc.HinhAnh,
                LastPrice = cthd.DonGia.Value,
                PayMentID = cthd.MaHD.Value,
                TeacherId = cthd.KhoaHoc.MaGV.Value,
                TeacherName = getDAODB.GetGiaoVienTheoMa(cthd.KhoaHoc.MaGV.Value).HoTen
            };
        }
        public static IEnumerable<PaymentItemVM> MapListPaymentItem(IEnumerable<CT_HoaDon> lstCT)
        {
            List<PaymentItemVM> lstReturn = new List<PaymentItemVM>();
            foreach (var item in lstCT.ToList())
            {
                lstReturn.Add(MapPaymentItem(item));
            }
            return lstReturn;
        }
        public static HoaDonVM MapOrder(HoaDon hoaDon)
        {
            return new HoaDonVM()
            {
                MaND = hoaDon.MaND.Value,
                DanhSachHangHoa = MapListPaymentItem(hoaDon.CT_HoaDon.ToList()).ToList(),
                HinhThucThanhToan = hoaDon.HinhThucThanhToan,
                TongThanhToan = hoaDon.TongTien.Value,
                MaHoaDon = hoaDon.MaHD
            };
        }
        public static List<HoaDonVM> MapListOrder(IEnumerable<HoaDon> lstHoaDon)
        {
            List<HoaDonVM> lstReturn = new List<HoaDonVM>();
            foreach (var item in lstHoaDon)
            {
                lstReturn.Add(new HoaDonVM()
                {
                    MaND = item.MaND.Value,
                    DanhSachHangHoa = MapListPaymentItem(item.CT_HoaDon.ToList()).ToList(),
                    HinhThucThanhToan = item.HinhThucThanhToan,
                    TongThanhToan = item.TongTien.Value,
                    MaHoaDon = item.MaHD
                });
            }
            return lstReturn;
        }
        public static SimpleHoaDonVM MapSimpleOrder(HoaDon hoaDon)
        {
            return new SimpleHoaDonVM()
            {
                MaND = hoaDon.MaND.Value,
                HinhThucThanhToan = hoaDon.HinhThucThanhToan,
                TongThanhToan = hoaDon.TongTien.Value,
                MaHoaDon = hoaDon.MaHD,
                TrangThai = hoaDon.TrangThai,
                NgayTaoHoaDon = hoaDon.NgayLap.Value,
                TenND = new GetDAO().GetTenNguoiDung(hoaDon.MaND.Value)
            };
        }
        public static List<SimpleHoaDonVM> MapListSimpleOrder(IEnumerable<HoaDon> lstHoaDon)
        {
            List<SimpleHoaDonVM> lstReturn = new List<SimpleHoaDonVM>();
            foreach (var item in lstHoaDon)
            {
                lstReturn.Add(MapSimpleOrder(item));
            }
            return lstReturn;

        }
        public static CT_HoaDon MapPaymentItemReverse(PaymentItemVM model)
        {
            return new CT_HoaDon()
            {
                MaKhoaHoc = model.CourseID,
                DonGia = model.LastPrice
            };
        }
        public static IEnumerable<CT_HoaDon> MapListPaymentItemReverse(IEnumerable<PaymentItemVM> lstModel)
        {
            List<CT_HoaDon> lstReturn = new List<CT_HoaDon>();
            foreach (var item in lstModel.ToList())
            {
                lstReturn.Add(MapPaymentItemReverse(item));
            }
            return lstReturn;
        }
    }
}