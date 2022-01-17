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
using static Common.AllEnum;

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
            var km = db.KhuyenMais.FirstOrDefault(x => x.MaKM == hd.MaKM);
            KhuyenMai_KhachHang kmkh = null;
            if(km != null)
                kmkh = db.KhuyenMai_KhachHang.FirstOrDefault(x => x.MaND == hd.MaND && x.MaKM == km.MaKM);
            dtt.TrangThai = AllEnum.TrangThaiDonThuTien.DaThanhToan.ToString();
            hd.TrangThai = "1";
            hd.ThanhToan = true;
            await TichDiemNguoiDung(hd.MaND.Value, hd.TongTien.Value);
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
                decimal GiaTri = 0;
                if (km != null)
                    GiaTri = km.GiaTri == null? 0: km.GiaTri.Value;
                await GuiMailSauKhiThanhToan(dtt.Email, lstKhoaHoc, hd.MaHD, hd.TongTien.Value, GiaTri);
                if(kmkh != null)
                    kmkh.IsSuDung = true;
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                kmkh.IsSuDung = false;
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
        public int AddHoaDon(int MaND, string MaKM, string TrangThai, string HinhThucThanhToan, int MaGioHang)
        {
            if (MaND == -1)
                return -2;
            var nd = db.NguoiDungs.FirstOrDefault(x => x.MaND == MaND);
            if (nd != null)
            {
                if (nd.DaXoa == true)
                    return -2;
            }
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
            var nd = db.NguoiDungs.FirstOrDefault(x => x.MaND == MaND);
            if(nd != null)
            {
                if (nd.DaXoa == true)
                    return -2;
            }
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
        public async Task<bool> TichDiemNguoiDung(int pMaND, decimal TongThanhToan)
        {
            if (TongThanhToan <= 0)
                return false;
            int TichDiemCongThem = (int)Math.Floor(TongThanhToan / 100);

            var tichdiem = db.TichDiems.FirstOrDefault(x => x.MaND == pMaND);
            if (tichdiem == null)
            {
                TichDiem td = new TichDiem();
                td.MaND = pMaND;
                td.SoDiem = TichDiemCongThem;
                db.TichDiems.Add(td);
            }
            else
            {
                if(tichdiem.SoDiem == null)
                    tichdiem.SoDiem = TichDiemCongThem;
                else 
                    tichdiem.SoDiem += TichDiemCongThem;
            }
            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public AllEnum.KetQuaTraVe TaoDonThuTien(int MaKH, int MaHD, string DiaChiThu, string Email,
            string SDTThu, string DonViThuHo, double SoTienThu, double PhiThuHo,
            DateTime? NgayDuKienThu, string GhiChu, string MaApDung = null)
        {

            if (db.DonThuTiens.Any(x => x.MaHD == MaHD))
                return AllEnum.KetQuaTraVe.DaTonTai;
            var hd = db.HoaDons.FirstOrDefault(x => x.MaHD == MaHD);
            var km = db.KhuyenMais.FirstOrDefault(x => x.MaApDung == MaApDung);
            if (km != null)
            {
                var km_kh = db.KhuyenMai_KhachHang.FirstOrDefault(x => x.MaND == MaKH && x.MaKM == km.MaKM);
                if (km_kh != null)
                {
                    hd.MaKM = km_kh.MaKM;
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
            hd.HinhThucThanhToan = "Offline";
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
        public async Task<KetQuaTraVe> ThanhToanNgay(int pMaHoaDon, string pMaApDung)
        {
            var hd = db.HoaDons.FirstOrDefault(x => x.MaHD == pMaHoaDon);
            var km = db.KhuyenMais.FirstOrDefault(x => x.MaApDung == pMaApDung);
            if (hd == null || km == null)
                return KetQuaTraVe.KhongTonTai;
            var cthd = hd.CT_HoaDon;
            var kmkh = db.KhuyenMai_KhachHang.FirstOrDefault(x => x.MaKM == km.MaKM && x.MaND == hd.MaND);
            var lstKhoaHoc = db.KhoaHocs.ToList();
            var nd = db.NguoiDungs.FirstOrDefault(x => x.MaND == hd.MaND);
            lstKhoaHoc = lstKhoaHoc.Where(x => cthd.Any(y => y.MaKhoaHoc == x.MaKhoaHoc)).ToList();
            lstKhoaHoc.ForEach(x =>
            {
                if (x.SoLuongMua == null)
                    x.SoLuongMua = 1;
                else
                    x.SoLuongMua++;
            });
            if (cthd.Count() == 0)
                return KetQuaTraVe.KhongHopLe;
            if (cthd.Sum(x => x.DonGia.Value) - km.GiaTri > 0)
                return KetQuaTraVe.KhongDuocPhep;
            if (kmkh.IsSuDung == true || kmkh.NgayKetThuc < DateTime.Today)
                return KetQuaTraVe.KhongChinhXac;
            hd.ThanhToan = true;
            kmkh.IsSuDung = true;
            await GuiMailSauKhiThanhToan(nd.Email, lstKhoaHoc, hd.MaHD, hd.TongTien.Value, km.GiaTri.Value);
            try
            {
                await db.SaveChangesAsync();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
            

        }
        public async Task<KetQuaTraVe> ThanhToanNgay2(int pMaHoaDon, string pMaApDung = "")
        {
            var hd = db.HoaDons.FirstOrDefault(x => x.MaHD == pMaHoaDon);
            var km = db.KhuyenMais.FirstOrDefault(x => x.MaApDung == pMaApDung);
            if (hd == null)
                return KetQuaTraVe.KhongTonTai;
            var cthd = hd.CT_HoaDon;
            var kmkh = new KhuyenMai_KhachHang();
            decimal GiaTri = 0;
            if(km != null)
            {
                kmkh = db.KhuyenMai_KhachHang.FirstOrDefault(x => x.MaKM == km.MaKM && x.MaND == hd.MaND);
                if (kmkh == null)
                    return KetQuaTraVe.KhongDuocPhep;
                if (kmkh.IsSuDung == true || kmkh.NgayKetThuc < DateTime.Today)
                    return KetQuaTraVe.KhongChinhXac;
                GiaTri = km.GiaTri == null ? 0 : km.GiaTri.Value;
                if (kmkh != null)
                {
                    kmkh.IsSuDung = true;
                    hd.MaKM = kmkh.MaKM;
                }
            }
            var lstKhoaHoc = db.KhoaHocs.ToList();
            var nd = db.NguoiDungs.FirstOrDefault(x => x.MaND == hd.MaND);
            lstKhoaHoc = lstKhoaHoc.Where(x => cthd.Any(y => y.MaKhoaHoc == x.MaKhoaHoc)).ToList();
            lstKhoaHoc.ForEach(x =>
            {
                if (x.SoLuongMua == null)
                    x.SoLuongMua = 1;
                else
                    x.SoLuongMua++;
            });
            if (cthd.Count() == 0)
                return KetQuaTraVe.KhongHopLe;
            
            hd.ThanhToan = true;
            hd.HinhThucThanhToan = PaymentType.ViDienTu.ToString();
            await TichDiemNguoiDung(hd.MaND.Value, hd.TongTien.Value);
            await GuiMailSauKhiThanhToan(nd.Email, lstKhoaHoc, hd.MaHD, hd.TongTien.Value, GiaTri);
            try
            {
                await db.SaveChangesAsync();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }


        public async Task<AllEnum.KetQuaTraVe> GuiMailSauKhiThanhToan(string reciepiantMailAddress,
            IEnumerable<KhoaHoc> lstKhoaHoc,
            int pMaHoaDon,
            decimal TongTien,
            decimal GiamGia = 0
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
            Body = Body.Replace("TONGTHANHTIENCANTHAYTHE",string.Format("{0:0,0 vnđ}", TongTien));
            Body = Body.Replace("TONGGIAMGIACANTHAYTHE", "- " + string.Format("{0:0,0 vnđ}", GiamGia));
            if(GiamGia >= TongTien)
                Body = Body.Replace("TONGTIENDATHANHTOAN", "- " + string.Format("{0:0,0 vnđ}", 0));
            else
                Body = Body.Replace("TONGTIENDATHANHTOAN", string.Format("{0:0,0 vnđ}", TongTien - GiamGia));
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