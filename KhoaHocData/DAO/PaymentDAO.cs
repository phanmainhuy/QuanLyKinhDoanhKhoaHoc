using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;

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
        public AllEnum.KetQuaTraVe XacNhanThanhToanHoaDon(int pMaHD)
        {
            var hd = db.HoaDons.SingleOrDefault(x => x.MaHD == pMaHD);
            var dtt = db.DonThuTiens.SingleOrDefault(x => x.MaHD == pMaHD);

            if (hd == null)
                return AllEnum.KetQuaTraVe.ChaKhongTonTai;
            if (dtt == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            dtt.TrangThai = AllEnum.TrangThaiDonThuTien.DaThanhToan.ToString();
            hd.TrangThai = "0";
            hd.ThanhToan = true;
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }

        public IEnumerable<HoaDon> LayToanBoHoaDonPaging(int page, int pageSize, out int totalPage)
        {
            int skipSize = page * pageSize;
            totalPage = db.HoaDons.Count();
            return db.HoaDons.OrderBy(x => x.MaHD).Skip(skipSize).Take(pageSize);
        }
        public IEnumerable<HoaDon> LayToanBoHoaDonChoDuyetPaging(int page, int pageSize, out int totalPage)
        {
            int skipSize = page * pageSize;
            totalPage = db.HoaDons.Count();
            var lstDonThuTien = db.DonThuTiens.ToList();
            List<HoaDon> lstHoaDon = new List<HoaDon>();
            foreach (var item in db.HoaDons.Where(x => !x.ThanhToan.Value).ToList())
            {
                if (lstDonThuTien.Any(x => x.MaHD == item.MaHD)) ;
                lstHoaDon.Add(item);
            }
            return lstHoaDon;
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
        public int AddHoaDon(int MaND, int MaKM, string TrangThai, string HinhThucThanhToan, int MaGioHang, bool? isBlock = false)
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
                TrangThai = "1",
                ThanhToan = false,
                GiamGia = KhuyenMai != null ? KhuyenMai.GiaTri.Value : 0
            };
            db.HoaDons.Add(hd);
            var giohangItems = db.CT_GioHang.Where(x => x.MaGioHang == MaGioHang).ToList();
            if (GioHang.TrangThai == AllEnum.TrangThaiGioHang.DaTaoHoaDon.ToString())
                return -3;
            GioHang.TrangThai = AllEnum.TrangThaiGioHang.DaTaoHoaDon.ToString();
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
            var DonThuTien = db.DonThuTiens.SingleOrDefault(x => x.MaHD == MaHD);
            if (HoaDon == null)
                return false;
            if (HoaDon.ThanhToan.Value)
                return false;
            if (DonThuTien != null)
                return true;

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
            if (db.KhoaHocCuaToi(MaND).Any(x => x.MaKhoaHoc == MaKH))
                return -3;
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
                TrangThai = "0",
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
        public AllEnum.KetQuaTraVe ThanhToanHoaDon(int pMaHD)
        {
            var hd = db.HoaDons.SingleOrDefault(x => x.MaHD == pMaHD);
            if (hd == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            hd.ThanhToan = true;
            hd.TrangThai = "1";
            var td = db.TichDiems.SingleOrDefault(x => x.MaND == hd.MaND);
            if(td == null)
            {
                TichDiem tdMoi = new TichDiem();
                tdMoi.MaND = hd.MaND.Value;
                tdMoi.SoDiem += (int)(hd.TongTien / 100);
                db.TichDiems.Add(tdMoi);
            }
            else
            {
                td.SoDiem += (int)(hd.TongTien / 100);
            }
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }

        }
        public AllEnum.KetQuaTraVe TaoDonThuTien(int MaKH, int MaHD, string DiaChiThu, string SDTThu, string DonViThuHo, double SoTienThu, double PhiThuHo, DateTime? NgayDuKienThu, string GhiChu)
        {
            if (db.DonThuTiens.Any(x => x.MaHD == MaHD))
                return AllEnum.KetQuaTraVe.DaTonTai;
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
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            return AllEnum.KetQuaTraVe.ThatBai;
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
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public IEnumerable<HoaDon> LayToanBoHoaDonPaging(bool isThanhToan, int page, int pageSize, out int total)
        {
            int skipSize = (page - 1) * pageSize;

            var item = db.HoaDons.Where(x => x.ThanhToan == isThanhToan).OrderByDescending(x => x.MaHD);
            total = item.Count();
            return item.Skip(skipSize).Take(pageSize).ToList();
        }
        public IEnumerable<HoaDon> LayToanBoHoaDonDieuKienPaging(bool isThanhToan, int? MaKH, DateTime? NgayBatDau, DateTime? NgayKetThuc, int page, int pageSize, out int total)
        {
            int skipSize = (page - 1) * pageSize;
            var item = db.HoaDons.Where(x => x.ThanhToan == isThanhToan).OrderByDescending(x => x.MaHD).ToList();
            if (MaKH != null)
                item = item.Where(x => x.MaND == MaKH.Value).ToList();
            if(NgayBatDau != null && NgayKetThuc != null)
                item = item.Where(x => 
                DateTime.Compare(x.NgayLap.Value, NgayBatDau.Value) >= 0 &&
                DateTime.Compare(x.NgayLap.Value, NgayKetThuc.Value) <= 0
                ).ToList();
            total = item.Count();
            return item.Skip(skipSize).Take(pageSize).ToList();
        }
        public AllEnum.KetQuaTraVe GuiMailSauKhiThanhToan(string reciepiantMailAddress)
        {
            string MyMail = ConfigurationManager.AppSettings["mymail"];
            string MyMailPassword = ConfigurationManager.AppSettings["mymailpassword"];
            string Subject = "Đơn hàng thanh toán thành công",
            Body = "",
            FromMail = "",
            HostMail = "vu.vantuy.wt@gmail.com";

            try
            {

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(reciepiantMailAddress);
                    mail.Subject = Subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient client = new SmtpClient(HostMail))
                    {
                        client.Credentials = new System.Net.NetworkCredential(MyMail, MyMailPassword);
                        client.EnableSsl = true;
                        client.Send(mail);
                        return AllEnum.KetQuaTraVe.ThanhCong;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
    }
}