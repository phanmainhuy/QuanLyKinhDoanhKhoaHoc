using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class PaymentDAO
    {
        QL_KHOAHOCEntities db;

        public PaymentDAO()
        {
            db = new QL_KHOAHOCEntities();
        }
        public IEnumerable<HoaDon> LayToanBoHoaDon()
        {
            return db.HoaDons;
        }
        public HoaDon LayHoaDonTheoMa(int pMaHoaDon)
        {
            return db.HoaDons.SingleOrDefault(x => x.MaHD == pMaHoaDon);
        }
        public IEnumerable<HoaDon> LayToanBoHoaDonPaging(int page, int pageSize, out int totalPage)
        {
            int skipSize = page * pageSize;
            totalPage = db.HoaDons.Count();
            return db.HoaDons.OrderBy(x => x.MaHD).Skip(skipSize).Take(pageSize);
        }
        //public bool AddHoaDon(int MaND, int MaKM, string TrangThai, string HinhThucThanhToan, IEnumerable< CT_HoaDon> lstChiTietHoaDon)
        //{
        //    HoaDon hd = new HoaDon()
        //    {
        //        MaKM = MaKM,
        //        HinhThucThanhToan = HinhThucThanhToan,
        //        MaND = MaND,
        //        NgayLap = DateTime.Now.Date,
        //        TongTien = 0,
        //        TrangThai = "Vừa tạo",
        //        ThanhToan = false,
        //    };
        //    db.HoaDons.Add(hd);
        //    SaveAll();
        //    foreach(var item in lstChiTietHoaDon.ToList())
        //    {
        //        item.MaHD = hd.MaHD;
        //    }

        //    db.CT_HoaDon.AddRange(lstChiTietHoaDon);
        //    return SaveAll();
        //}
        public int AddHoaDon(int MaND, int MaKM, string TrangThai, string HinhThucThanhToan, int MaGioHang)
        {
            var KhuyenMai = db.KhuyenMais.SingleOrDefault(x => x.MaKM == MaKM);
            var GioHang = db.GioHangs.SingleOrDefault(x => x.MaGioHang== MaGioHang);
            HoaDon hd = new HoaDon()
            {
                MaKM = MaKM,
                HinhThucThanhToan = HinhThucThanhToan,
                MaND = MaND,
                NgayLap = DateTime.Now.Date,
                TongTien = GioHang.TongTien.Value,
                TrangThai = "Active",
                ThanhToan = false,
                GiamGia = KhuyenMai != null ? KhuyenMai.GiaTri.Value : 0
            };
            db.HoaDons.Add(hd);
            var giohangItems = db.CT_GioHang.Where(x => x.MaGioHang == MaGioHang).ToList();
            SaveAll();

            foreach(var item in giohangItems)
            {
                db.CT_HoaDon.Add(new CT_HoaDon()
                {
                    MaHD = hd.MaHD,
                    DonGia = item.DonGia,
                    MaKhoaHoc = item.MaKhoaHoc
                });
            }
            if (SaveAll())
                return hd.MaHD;
            else
                return -1;
        }
        public bool CancelOrder(int MaHD)
        {
            var HoaDon = db.HoaDons.SingleOrDefault(x => x.MaHD == MaHD);
            if (HoaDon == null)
                return false;
            if (HoaDon.ThanhToan.Value)
                return false;
            foreach(var item in db.CT_HoaDon.ToList())
            {
                if (item.MaHD == MaHD)
                    db.CT_HoaDon.Remove(item);
            }

            db.HoaDons.Remove(HoaDon);
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
