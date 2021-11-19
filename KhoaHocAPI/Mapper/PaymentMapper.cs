using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System.Collections.Generic;
using System.Linq;

namespace KhoaHocAPI.Mapper
{
    public class PaymentMapper
    {
        private GetDAO getDAODB = new GetDAO();

        public PaymentItemVM MapPaymentItem(CT_HoaDon cthd)
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
        public IEnumerable<PaymentItemVM> MapListPaymentItem(IEnumerable<CT_HoaDon> lstCT)
        {
            List<PaymentItemVM> lstReturn = new List<PaymentItemVM>();
            foreach (var item in lstCT.ToList())
            {
                lstReturn.Add(MapPaymentItem(item));
            }
            return lstReturn;
        }
        public CT_HoaDon MapPaymentItemReverse(PaymentItemVM model)
        {
            return new CT_HoaDon()
            {
                MaKhoaHoc = model.CourseID,
                DonGia = model.LastPrice
            };
        }
        public IEnumerable<CT_HoaDon> MapListPaymentItemReverse(IEnumerable<PaymentItemVM> lstModel)
        {
            List<CT_HoaDon> lstReturn = new List<CT_HoaDon>();
            foreach(var item in lstModel.ToList())
            {
                lstReturn.Add(MapPaymentItemReverse(item));
            }
            return lstReturn;
        }
    }
}