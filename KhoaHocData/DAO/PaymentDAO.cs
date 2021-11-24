using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KhoaHocData.DAO
{
    public class PaymentDAO
    {
        private QL_KHOAHOCEntities db;

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
            if (MaND == -1)
                return -2;
            var KhuyenMai = db.KhuyenMais.SingleOrDefault(x => x.MaKM == MaKM);
            var GioHang = db.GioHangs.SingleOrDefault(x => x.MaGioHang == MaGioHang);
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

            foreach (var item in giohangItems)
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
            foreach (var item in db.CT_HoaDon.ToList())
            {
                if (item.MaHD == MaHD)
                    db.CT_HoaDon.Remove(item);
            }

            db.HoaDons.Remove(HoaDon);
            return SaveAll();
        }

        public int TaoHoaDon1KhoaHoc(int MaND, int MaKM, string TrangThai, string HinhThucThanhToan, int MaKH)
        {
            if (MaND == -1)
                return -2;
            var KhuyenMai = db.KhuyenMais.SingleOrDefault(x => x.MaKM == MaKM);
            var KhoaHoc = db.KhoaHocs.SingleOrDefault(x => x.MaKhoaHoc == MaKH);
            if (KhoaHoc == null)
                return -1;
            HoaDon hd = new HoaDon()
            {
                MaKM = MaKM,
                HinhThucThanhToan = HinhThucThanhToan,
                MaND = MaND,
                NgayLap = DateTime.Now.Date,
                TongTien = KhoaHoc != null ? KhoaHoc.DonGia.Value : 0,
                TrangThai = "Active",
                ThanhToan = false,
                GiamGia = KhuyenMai != null ? KhuyenMai.GiaTri.Value : 0
            };
            db.HoaDons.Add(hd);
            SaveAll();

            db.CT_HoaDon.Add(new CT_HoaDon()
            {
                MaHD = hd.MaHD,
                DonGia = KhoaHoc.DonGia.Value,
                MaKhoaHoc = KhoaHoc.MaKhoaHoc
            });
            if (SaveAll())
                return hd.MaHD;
            else
                return -1;
        }

        public int TaoDonThuTien(int MaKH, int MaHD, string DiaChiThu, string SDTThu, string DonViThuHo, double SoTienThu, double PhiThuHo, DateTime? NgayDuKienThu, string GhiChu)
        {
            
            db.DonThuTiens.Add(new DonThuTien()
            {
                MaHD = MaHD,
                DiaChiThu = DiaChiThu,
                DonViThuHo = DonViThuHo,
                GhiChu = GhiChu,
                MaKH = MaKH,
                NgayDuKienThu = DateTime.Now.AddDays(2),
                NgayTao = DateTime.Now.Date,
                PhiThuHo = (decimal)PhiThuHo,
                SDTThu = SDTThu,
                SoTienThu = (decimal)SoTienThu,
                TrangThai = "Process"
            });
            if (SaveAll())
            {
                return 1;
            }
            return 0;
        }

        public double TinhCuocThuHo(double GiaMatHang)
        {
            var CodRate1 = 0.01;
            var CodRate2 = 0.012;
            var OtherRate = 0.001;
            var COD = GiaMatHang > 1000000 
                ? ((GiaMatHang * CodRate1) > 15000 
                    ? (GiaMatHang * CodRate1) 
                    : 15000) 
                : ((GiaMatHang * CodRate2) > 18000 
                    ? (GiaMatHang * CodRate2) 
                    : 18000);
            var ServiceFee = 10000;
            var OtherFee = (GiaMatHang * OtherRate) > 10000 
                ? (GiaMatHang * OtherRate) 
                : 10000;
            OtherFee =OtherFee > 100000 ? 10000 : OtherFee;
            return COD + ServiceFee + OtherFee;
        }

        private bool SaveAll()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}