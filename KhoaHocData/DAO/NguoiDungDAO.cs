using Common;
using KhoaHocData.EF;
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
        public IEnumerable<NguoiDung> LayDanhSachHocVienPaging(int pPage, int pPageSize, out int pTotalPage)
        {
            int SkipCount = pPage * pPageSize;
            List<NguoiDung> lstNguoiDung = db.NguoiDungs.Where(x => x.MaNhomNguoiDung == (int)MaNhomNguoiDung.Student).ToList();
            pTotalPage = lstNguoiDung.Count();
            return lstNguoiDung.Skip(pTotalPage);
        }
        public IEnumerable<NguoiDung> LayDanhSachTheoMaNhom(int pMaNhomNguoiDung)
        {
            return db.NguoiDungs.Where(x => x.MaNhomNguoiDung == pMaNhomNguoiDung && !x.DaXoa.Value || x.DaXoa == null);
        }
        public NguoiDung LayNguoiDungTheoId(int pMaNguoiDung)
        {
            return db.NguoiDungs.Find(pMaNguoiDung);
        }
        public bool DaMuaKhoaHoc(int pMaND, int pMaKhoaHoc)
        {
            var nd = db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND);
            if (nd.MaNhomNguoiDung != (int)AllEnum.MaNhomNguoiDung.Student)
                return false;
            if (nd == null)
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
            var nd = db.NguoiDungs.SingleOrDefault(x => x.TenDN == pUserName && x.MatKhau == pOldPassword);
            if (nd == null)
                return KetQuaTraVe.KhongTonTai;
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
        //public bool CoQuyenChinhSua(int pMaND, int pMaQuyen)
        public KetQuaTraVe ThayDoiThongTinNguoiDung(int pUserID, string pUserName, int pMaNhomNguoiDung, string pName, string pCMND, string HinhAnh,
            string Number, string Email, DateTime DoB, string pAddress)
        {
            var nd = db.NguoiDungs.Where(x => x.MaND == pUserID).SingleOrDefault();
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
            if (nd.NgaySinh.Value != DoB)
                nd.NgaySinh = DoB;
            if (nd.Diachi != pAddress)
                nd.Diachi = pAddress;
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
        public KetQuaTraVe ThayDoiTrangThaiNguoiDung(int pUserID, bool pTrangThai)
        {
            var nd = db.NguoiDungs.FirstOrDefault(x => x.MaND == pUserID);
            if (nd == null)
                return KetQuaTraVe.KhongTonTai;
            if (nd.TrangThai.Value == pTrangThai)
                return KetQuaTraVe.ThanhCong;
            nd.TrangThai = pTrangThai;
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
    }
}
