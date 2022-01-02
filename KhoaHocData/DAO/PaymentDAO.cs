using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

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

        public DonThuTien LayDonThuTienTheoMa(int pMaHoaDon)
        {
            return db.DonThuTiens.SingleOrDefault(x => x.MaHD == pMaHoaDon);
        }

        public async Task<AllEnum.KetQuaTraVe> XacNhanThanhToanHoaDon(int pMaHD)
        {
            var hd = db.HoaDons.SingleOrDefault(x => x.MaHD == pMaHD);
            var dtt = db.DonThuTiens.SingleOrDefault(x => x.MaHD == pMaHD);
            

            if (hd == null)
                return AllEnum.KetQuaTraVe.ChaKhongTonTai;
            if (dtt == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            dtt.TrangThai = AllEnum.TrangThaiDonThuTien.DaThanhToan.ToString();
            hd.TrangThai = "1";
            hd.ThanhToan = true;
            decimal TruTien = 0;
            
            if(hd.MaKM != null)
            {
                var km = db.KhuyenMais.FirstOrDefault(x => x.MaKM == hd.MaKM);
                TruTien = km.GiaTri == null? 0:km.GiaTri.Value;
            }
            var CTHD = hd.CT_HoaDon.ToList();
            var lstKhoaHoc = db.KhoaHocs.ToList();
            var nd = db.NguoiDungs.FirstOrDefault(x => x.MaND == hd.MaND);
            lstKhoaHoc = lstKhoaHoc.Where(x => CTHD.Any(y => y.MaKhoaHoc == x.MaKhoaHoc)).ToList();
            lstKhoaHoc.ForEach(x =>
            {
                if (x.SoLuongMua == null)
                    x.SoLuongMua = 1;
                else
                    x.SoLuongMua++;
            });
            try
            {
                db.SaveChanges();
                await GuiMailSauKhiThanhToan(dtt.Email, lstKhoaHoc, hd.MaHD, hd.TongTien.Value - TruTien <= 0? 1 : hd.TongTien.Value - TruTien);
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
                if (lstDonThuTien.Any(x => x.MaHD == item.MaHD))
                    lstHoaDon.Add(item);
            }
            return lstHoaDon;
        }
        
        public IEnumerable<HoaDon> LayToanBoHoaDonDaDuyetPaging(int page, int pageSize, out int totalPage)
        {
            int skipSize = page * pageSize;
            totalPage = db.HoaDons.Count();
            var lstDonThuTien = db.DonThuTiens.ToList();
            List<HoaDon> lstHoaDon = new List<HoaDon>();
            foreach (var item in db.HoaDons.Where(x => x.ThanhToan.Value).ToList())
            {
                if (lstDonThuTien.Any(x => x.MaHD == item.MaHD))
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
        public int AddHoaDon(int MaND, string MaKM, string TrangThai, string HinhThucThanhToan, int MaGioHang, bool? isBlock = false)
        {
            if (MaND == -1)
                return -2;
            var KhuyenMai = db.KhuyenMais.SingleOrDefault(x => x.MaApDung == MaKM);
            var hoadons = db.HoaDons.Where(x => x.MaND == MaND);
            var GioHang = db.GioHangs.SingleOrDefault(x => x.MaGioHang == MaGioHang);
            if (GioHang == null)
                return -4;
            if (GioHang.CT_GioHang.Count() == 0)
                return -4;
            var giohangItems = db.CT_GioHang.Where(x => x.MaGioHang == MaGioHang).ToList();
            
            HoaDon hd = new HoaDon()
            {
                MaKM = KhuyenMai?.MaKM,
                HinhThucThanhToan = HinhThucThanhToan,
                MaND = MaND,
                NgayLap = DateTime.Now.Date,
                TongTien = GioHang.TongTien.Value,
                TrangThai = "1",
                ThanhToan = false,
                GiamGia = KhuyenMai != null ? KhuyenMai.GiaTri.Value : 0
            };
            db.HoaDons.Add(hd);
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

        public int TaoHoaDon1KhoaHoc(int MaND, string MaKM, string TrangThai, string HinhThucThanhToan, int MaKH)
        {
            if (MaND == -1)
                return -2;
            if (db.KhoaHocCuaToi(MaND).Any(x => x.MaKhoaHoc == MaKH))
                return -3;
            var KhuyenMai = db.KhuyenMais.SingleOrDefault(x => x.MaApDung == MaKM);
            var KhoaHoc = db.KhoaHocs.SingleOrDefault(x => x.MaKhoaHoc == MaKH);
            if (KhoaHoc == null)
                return -1;
            HoaDon hd = new HoaDon()
            {
                MaKM = KhuyenMai?.MaKM,
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
            var khs = db.KhoaHocs.ToList();
            foreach(var item in hd.CT_HoaDon.ToList())
            {
                var ind = khs.SingleOrDefault(x => x.MaKhoaHoc == item.MaKhoaHoc);
                if (ind == null)
                    continue;
                if (ind.SoLuongMua == null)
                    ind.SoLuongMua = 1;
                else
                    ind.SoLuongMua++;
            }
            if (td == null)
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

        public AllEnum.KetQuaTraVe TaoDonThuTien(int MaKH, int MaHD, string DiaChiThu, string Email,
            string SDTThu, string DonViThuHo, double SoTienThu, double PhiThuHo,
            DateTime? NgayDuKienThu, string GhiChu, string MaApDung = null)
        {
            if (db.DonThuTiens.Any(x => x.MaHD == MaHD))
                return AllEnum.KetQuaTraVe.DaTonTai;
            var km = db.KhuyenMais.FirstOrDefault(x => x.MaApDung == MaApDung);
            if (km != null)
            {
                var km_kh = db.KhuyenMai_KhachHang.FirstOrDefault(x => x.MaND == MaKH && x.MaKM == km.MaKM);
                if (km_kh != null)
                {
                    km_kh.IsSuDung = true;
                }
            }
            
            db.DonThuTiens.Add(new DonThuTien()
            {
                MaHD = MaHD,
                DiaChiThu = DiaChiThu,
                DonViThuHo = DonViThuHo,
                GhiChu = GhiChu,
                MaKH = MaKH,
                NgayDuKienThu = DateTime.Now.AddDays(2),
                Email = Email,
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
            OtherFee = OtherFee > 100000 ? 10000 : OtherFee;
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
            if (NgayBatDau != null && NgayKetThuc != null)
                item = item.Where(x =>
                DateTime.Compare(x.NgayLap.Value, NgayBatDau.Value) >= 0 &&
                DateTime.Compare(x.NgayLap.Value, NgayKetThuc.Value) <= 0
                ).ToList();
            total = item.Count();
            return item.Skip(skipSize).Take(pageSize).ToList();
        }
        public IEnumerable<HoaDon> LayToanBoHoaDonDieuKien(DateTime? NgayBatDau, DateTime? NgayKetThuc)
        {
            var item = db.HoaDons.Where(x => x.ThanhToan == true).OrderByDescending(x => x.MaHD).ToList();
            
            if (NgayBatDau != null && NgayKetThuc != null)
                item = item.Where(x =>
                DateTime.Compare(x.NgayLap.Value, NgayBatDau.Value) >= 0 &&
                DateTime.Compare(x.NgayLap.Value, NgayKetThuc.Value) <= 0
                ).ToList();
            return item;
        }

        public async Task<AllEnum.KetQuaTraVe> GuiMailSauKhiThanhToan(string reciepiantMailAddress,
            IEnumerable<KhoaHoc> lstKhoaHoc,
            int pMaHoaDon,
            decimal TongTien
            
            )
        {
            string assemblyFile = HttpContext.Current.Server.MapPath("~/File");
            string MyMail = ConfigurationManager.AppSettings["mymail"];
            string MyMailPassword = ConfigurationManager.AppSettings["mymailpassword"];
            string TenCongTy = "Học online cùng ONLEARN";
            string DiaChiCongTy = "Xóm 3, thôn 2, xã IaKráj, huyện jayai, tỉnh Gia Lai";
            string SDTCongTy = "0976763378";
            string MaHoaDon = "HD"+pMaHoaDon.ToString();
            string file = File.ReadAllText(assemblyFile + "/product.txt");
            string mediaLink = "https://khoahocapi.conveyor.cloud/assets/images/courses/";
            string danhSachSanPham = "";
            foreach (var item in lstKhoaHoc.ToList())
            {
                var tempString = "";
                tempString = file.Replace("TENKHOAHOCCANTHAYTHE", item.TenKhoaHoc.ToString());
                tempString = tempString.Replace("DONGIACANTHAYTHE", string.Format("{0:0,0 vnđ}", item.DonGia.Value));
                tempString = tempString.Replace("HINHANHCANTHAYTHE", mediaLink + item.HinhAnh.ToString());
                danhSachSanPham += tempString;
            }
            string Subject = "Đơn hàng thanh toán thành công",
            Body = File.ReadAllText(assemblyFile + "/mailtemplate.txt"),
            FromMail = MyMail,
            HostMail = "smtp.gmail.com";
            Body = Body.Replace("DANHSACHKHOAHOCCANTHAYTHE", danhSachSanPham);
            Body = Body.Replace("TONGTHANHTIENCANTHAYTHE", string.Format("{0:0,0 vnđ}", TongTien));
            Body = Body.Replace("TENCONGTYCANTHAYTHE", TenCongTy);
            Body = Body.Replace("DIACHICONGTYCANTHAYTHE", DiaChiCongTy);
            Body = Body.Replace("SDTCONGTYCANTHAYTHE", SDTCongTy);
            Body = Body.Replace("MAHOADONCANTHAYTHE", MaHoaDon);
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(reciepiantMailAddress);
                    mail.Subject = Subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient client = new SmtpClient(HostMail, 587))
                    {
                        client.Credentials = new System.Net.NetworkCredential(MyMail, MyMailPassword);
                        client.EnableSsl = true;
                        await client.SendMailAsync(mail);
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