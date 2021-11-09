using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class Payment
    {
        QL_KHOAHOCEntities db;

        public Payment()
        {
            db = new QL_KHOAHOCEntities();
        }
        public IEnumerable<HoaDon> LayToanBoHoaDon()
        {
            return db.HoaDons;
        }
        public IEnumerable<HoaDon> LayToanBoHoaDonPaging(int page, int pageSize, out int totalPage)
        {
            int skipSize = page * pageSize;
            totalPage = db.HoaDons.Count();
            return db.HoaDons.OrderBy(x => x.MaHD).Skip(skipSize).Take(pageSize);
        }
        public bool AddHoaDon(int MaND, int MaKM, decimal GiamGia, string TrangThai, string HinhThucThanhToan)
        {
            HoaDon hd = new HoaDon()
            {
                MaKM = MaKM,
                GiamGia = GiamGia,
                HinhThucThanhToan = HinhThucThanhToan,
                MaND = MaND,
                NgayLap = DateTime.Now.Date,
                TongTien = 0,
                TrangThai = "Vừa tạo",
                ThanhToan = false,
            };
            db.HoaDons.Add(hd);
            return SaveAll();
        }
        public bool AddChiTietHoaDon(int MaND, int MaKM, decimal GiamGia, string TrangThai, string HinhThucThanhToan, int MaHD, int MaKH, decimal DonGia)
        {
            var hd = db.HoaDons.SingleOrDefault(x => x.MaHD == MaHD);
            if (hd == null)
            {
                AddHoaDon(MaND, MaKM, GiamGia, TrangThai, HinhThucThanhToan);
            }

            CT_HoaDon ct = new CT_HoaDon()
            {
                MaHD = MaHD,
                DonGia = DonGia,
                MaKhoaHoc = MaKH
            };
            db.CT_HoaDon.Add(ct);
            return SaveAll();

        }

        private bool SaveAll()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
