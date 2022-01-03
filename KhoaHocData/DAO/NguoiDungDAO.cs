using Common;
using KhoaHocData.EF;
using KhoaHocData.OnlineParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class NguoiDungDAO
    {
        QL_KHOAHOCEntities db;
        public NguoiDungDAO()
        {
            db = new QL_KHOAHOCEntities();
        }
        public AllEnum.RegisterResult DangKy(NguoiDung nd)
        {
            var item = db.NguoiDungs.SingleOrDefault(x => x.TenDN == nd.TenDN);
            if (item != null)
                return RegisterResult.DaTonTai;
            else
            {
                try
                {
                    db.NguoiDungs.Add(nd);
                    db.SaveChanges();

                    return RegisterResult.ThanhCong;

                }
                catch
                {
                    return RegisterResult.ThatBai;
                }
            }
        }
        public IEnumerable<NguoiDung> LayHetNguoiDung()
        {
            return db.NguoiDungs.Where(x=>!x.DaXoa.Value || x.DaXoa == null).ToList();
        }
        public NguoiDung LayNguoiDungTheoId(int pMaNguoiDung)
        {
            return db.NguoiDungs.Find(pMaNguoiDung);
        }
        public IEnumerable<NguoiDung> LayHetHeThong()
        {
            return db.NguoiDungs.Where(x => x.MaNhomNguoiDung != (int)MaNhomNguoiDung.Student && (!x.DaXoa.Value || x.DaXoa == null)).OrderBy(x => x.MaNhomNguoiDung);
        }
        public IEnumerable<NguoiDung> LayNguoiDungPaging(int pPage, int pPageSize, out int pTotalPage)
        {
            int SkipCount = pPage * pPageSize;
            pTotalPage = db.NguoiDungs.Count();
            return db.NguoiDungs.Where(x=>!x.DaXoa.Value || x.DaXoa == null).Skip(SkipCount - 1).ToList();
        }
        public IEnumerable<NguoiDung> LayDanhSachHocVien()
        {
            return db.NguoiDungs.Where(x => x.MaNhomNguoiDung == (int)MaNhomNguoiDung.Student);
        }
        public IEnumerable<NguoiDung> LayDanhSachHocVienPaging(int pPage, int pPageSize, out int pTotalPage, string searchString)
        {
            int SkipCount = pPageSize * (pPage - 1);
            List<NguoiDung> lstNguoiDung = new List<NguoiDung>();
            if (!string.IsNullOrEmpty(searchString) )
                lstNguoiDung = db.NguoiDungs.Where(x => x.MaNhomNguoiDung == (int)MaNhomNguoiDung.Student && x.HoTen.Contains(searchString)).ToList();
            else
                lstNguoiDung = db.NguoiDungs.Where(x => x.MaNhomNguoiDung == (int)MaNhomNguoiDung.Student).ToList();
            pTotalPage = lstNguoiDung.Count();
            return lstNguoiDung.Skip(SkipCount).Take(pPageSize);
        }
        public IEnumerable<NguoiDung> LayDanhSachTheoMaNhom(int pMaNhomNguoiDung)
        {
            return db.NguoiDungs.Where(x => x.MaNhomNguoiDung == pMaNhomNguoiDung && (!x.DaXoa.Value || x.DaXoa == null));
        }
        
        public bool DaMuaKhoaHoc(int pMaND, int pMaKhoaHoc)
        {
            var nd = db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND);
            if (nd == null)
                return false;
            if (nd.MaNhomNguoiDung != (int)AllEnum.MaNhomNguoiDung.Student)
                return false;
            List<HoaDon> lstHoaDon = db.HoaDons.Where(x => x.MaND == pMaND && x.ThanhToan.Value).ToList();
            if (lstHoaDon == null || lstHoaDon.Count() == 0)
                return false;
            foreach(var item in lstHoaDon)
            {
                if(item.CT_HoaDon != null && item.CT_HoaDon.Count() > 0)
                {
                    foreach (var item2 in item.CT_HoaDon.ToList())
                    {
                        if (item2.MaKhoaHoc == pMaKhoaHoc)
                            return true;
                    }
                }
            }
            return false;
        }
        public KetQuaTraVe DoiMatKhau(string pUserName, string pOldPassword, string pNewPassword)
        {
            pOldPassword = Utils.Encrypt(pOldPassword, pUserName);
            var nd = db.NguoiDungs.SingleOrDefault(x => x.TenDN == pUserName);
            if (nd == null)
                return KetQuaTraVe.KhongTonTai;
            if (nd.MatKhau != pOldPassword)
                return KetQuaTraVe.KhongChinhXac;
            pNewPassword = Utils.Encrypt(pNewPassword, pUserName);
            nd.MatKhau = pNewPassword;
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe DoiMatKhau2(string pUserName, string pNewPass, string Code)
        {
            var qmk = db.QuenMatKhaus.OrderByDescending(x=>x.STT).FirstOrDefault(x=>x.TenDN == pUserName);
            db.QuenMatKhaus.Select(x => x.TenDN);
            if (qmk == null)
                return KetQuaTraVe.KhongTonTai;
            if (qmk.ThoiGian < DateTime.Today)
                return KetQuaTraVe.KhongHopLe;
            if (qmk.Code == Code)
            {
                var nd = db.NguoiDungs.FirstOrDefault(x => x.TenDN == pUserName);
                pNewPass = Utils.Encrypt(pNewPass, pUserName);
                nd.MatKhau = pNewPass;
                try
                {
                    db.SaveChanges();
                    return KetQuaTraVe.ThanhCong;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return KetQuaTraVe.ThatBai;
                }
            }
            else
                return KetQuaTraVe.KhongChinhXac;
        }
        //public bool CoQuyenChinhSua(int pMaND, int pMaQuyen)
        public KetQuaTraVe ThayDoiThongTinNguoiDung(int pUserID, string pUserName, int pMaNhomNguoiDung, string pName, string pCMND, string HinhAnh,
            string Number, string Email, DateTime DoB, string pAddress, decimal Luong, string GioiTinh)
        {
            var nd = db.NguoiDungs.Where(x => x.MaND == pUserID).SingleOrDefault();
            var luong = db.Luongs.FirstOrDefault(x => x.MaND == pUserID);
            if(string.IsNullOrEmpty(nd.Email))
                if (db.NguoiDungs.Any(x => x.MaND != pUserID && x.Email == Email))
                    return KetQuaTraVe.DuLieuDaTonTai1;
            if(string.IsNullOrEmpty(nd.SDT))
                if (db.NguoiDungs.Any(x => x.MaND != pUserID && x.SDT == Number))
                    return KetQuaTraVe.DuLieuDaTonTai2;
            if(db.NguoiDungs.Any(x => x.MaND != pUserID && x.CMND.Trim() == pCMND.Trim()))
                return KetQuaTraVe.DuLieuDaTonTai3;
            if (db.NguoiDungs.Any(x => x.MaND != pUserID && x.TenDN.Trim().ToLower() == pUserName.Trim().ToLower()))
                return KetQuaTraVe.DaTonTai;
            if (luong == null)
            {
                db.Luongs.Add(new Luong()
                {
                    MaND = pUserID,
                    Luong1 = Luong
                });
            }
            else
            {
                if(Luong > 0)
                    luong.Luong1 = Luong;
            }

            if (nd == null)
                return KetQuaTraVe.KhongTonTai;
            if (nd.MaNhomNguoiDung != pMaNhomNguoiDung)
                nd.MaNhomNguoiDung = pMaNhomNguoiDung;
            if (nd.TenDN != pUserName)
                nd.TenDN = pUserName;
            if (nd.HoTen != pName)
                nd.HoTen = pName;
            if (nd.CMND != pCMND)
                nd.CMND = pCMND;
            if (nd.SDT != Number)
                nd.SDT = Number;
            if (nd.HinhAnh != HinhAnh)
                nd.HinhAnh = HinhAnh;
            if (nd.Email != Email)
                nd.Email = Email;
            if (nd.NgaySinh != DoB)
                nd.NgaySinh = DoB;
            if (nd.Diachi != pAddress)
                nd.Diachi = pAddress;
            if (GioiTinh != (-1).ToString())
                if (nd.GioiTinh != GioiTinh)
                    nd.GioiTinh = GioiTinh;
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe ThayDoiTrangThaiNguoiDung(List<int> pUserID, bool pTrangThai)
        {
            var lstND = db.NguoiDungs.ToList();
            foreach (var item in lstND)
            {
                if (pUserID.Contains(item.MaND))
                {
                    item.TrangThai = pTrangThai;
                }
            }
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
                throw;
            }
        }
        public KetQuaTraVe ThayDoiAnhDaiDienNguoiDung(int pMaND, string pHinhAnh)
        {
            var nd = db.NguoiDungs.FirstOrDefault(x => x.MaND == pMaND);
            if (db == null)
                return KetQuaTraVe.KhongTonTai;
            if (nd.HinhAnh == pHinhAnh)
                return KetQuaTraVe.ThanhCong;
            if (pHinhAnh == "" && nd.MaNhomNguoiDung != (int)MaNhomNguoiDung.Admin)
                pHinhAnh = "userdefault.jpg";
            nd.HinhAnh = pHinhAnh;
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe XoaNguoiDung(int pMaND)
        {
            var nd = db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND);
            if (nd == null)
                return KetQuaTraVe.ThanhCong;
            if (nd.DaXoa != null)
                if(nd.DaXoa.Value)
                    return KetQuaTraVe.ThanhCong;
            nd.DaXoa = true;
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe XoaNhieuNguoiDung(IEnumerable<int> lstMaND)
        {
            var lstND = db.NguoiDungs.ToList();
            List<NguoiDung> lstNguoiDung = new List<NguoiDung>();
            int i = 0;
            foreach(var item in lstND)
            {
                if (item.MaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Admin ||
                    item.MaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Student)
                    continue;
                if (lstMaND.Contains(item.MaND))
                {
                    item.DaXoa = true;
                    i++;
                }
                if (i == lstMaND.Count())
                    break;
            }
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
        public async Task<KetQuaTraVe> QuenMatKhau(string userName, string Email)
        {
            var nd = db.NguoiDungs.FirstOrDefault(x => x.TenDN.Trim().ToLower() == userName.Trim().ToLower());
            if (nd == null)
                return KetQuaTraVe.KhongTonTai;
            if (nd.Email != Email)
                return KetQuaTraVe.KhongChinhXac;
            string random = "";
            int length = 7;
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                random += r.Next(9);
            }
            QuenMatKhau qmk = new QuenMatKhau();
            qmk.Code = random;
            qmk.TenDN = nd.TenDN;
            qmk.ThoiGian = DateTime.Now.AddMinutes(15);
            db.QuenMatKhaus.Add(qmk);
            MailServices mail = new MailServices();
            string TieuDe = "Xác nhận quên mật khẩu";
            string NoiDung = "Mã siêu cấp của bạn là " + random;
            try
            {
                db.SaveChanges();
                await mail.GuiMailString(Email,TieuDe, NoiDung);
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
    }
}
