using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class CSKHDAO
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        public IEnumerable<ChamSocKhachHang> GetAllChamSocKhachHang(string sdt, string TenKhachHang, int page, int pageSize, out int total)
        {
            var chamsockhachhang = db.ChamSocKhachHangs.ToList();
            var returnResult = chamsockhachhang;
            var skipSize = (page - 1) * pageSize;

            if (!string.IsNullOrEmpty(sdt))
            {
                returnResult = returnResult.Where(x => x.SDTKH == sdt).ToList();
            }
            if (!string.IsNullOrEmpty(TenKhachHang))
            {
                returnResult = returnResult.Where(x => x.TenKH == TenKhachHang).ToList();
            }

            total = returnResult.Count();
            return returnResult.Skip(skipSize).Take(pageSize - 1);
        }
        public KetQuaTraVe ThemChamSocKhachHang(int pMaLoaiVanDe, int pMaNhanVien, string SDT, string TenKH, string NoiDung)
        {
            ChamSocKhachHang cskh = new ChamSocKhachHang();
            if (!db.LoaiVanDes.Any(x => x.MaLoaiVanDe == pMaLoaiVanDe))
                return KetQuaTraVe.ChaKhongTonTai;
            cskh.MaLoaiVanDe = pMaLoaiVanDe;
            cskh.MaND = pMaNhanVien;
            cskh.SDTKH = SDT;
            cskh.TenKH = TenKH;
            cskh.NoiDung = NoiDung;
            cskh.NgayLap = DateTime.Today;
            db.ChamSocKhachHangs.Add(cskh);
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
        public KetQuaTraVe ThayDoiThongTinChamSocKhachHang(int pMaCSKH, int? pMaLoaiVanDe, string SDT, string TenKH, string NoiDung, string CachXuLy)
        {
            var cskh = db.ChamSocKhachHangs.FirstOrDefault(x => x.MaCSKH == pMaCSKH);
            if (cskh == null)
                return KetQuaTraVe.KhongTonTai;
            if(pMaLoaiVanDe == null)
                return KetQuaTraVe.ChaKhongTonTai;
            if (!db.LoaiVanDes.Any(x => x.MaLoaiVanDe == pMaLoaiVanDe))
                return KetQuaTraVe.ChaKhongTonTai;
            cskh.MaLoaiVanDe = pMaLoaiVanDe;
            if (!string.IsNullOrEmpty(SDT))
                cskh.SDTKH = SDT;
            if (!string.IsNullOrEmpty(TenKH))
                cskh.TenKH = TenKH;
            if (!string.IsNullOrEmpty(NoiDung))
                cskh.NoiDung = NoiDung;
            if (!string.IsNullOrEmpty(CachXuLy))
                cskh.CachXuLy = CachXuLy;
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
        public KetQuaTraVe XoaChamSocKhachHang(int pMaCSKH)
        {
            var cskh = db.ChamSocKhachHangs.FirstOrDefault(x => x.MaCSKH == pMaCSKH);
            if (cskh == null)
                return KetQuaTraVe.KhongTonTai;
            db.ChamSocKhachHangs.Remove(cskh);
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
        public IEnumerable<LoaiVanDe> LayToanBoLoaiVanDe()
        {
            return db.LoaiVanDes.ToList();
        }
        public KetQuaTraVe ThemLoaiVanDe(string TenLoaiVanDe)
        {
            if (db.LoaiVanDes.Any(x => x.TenLoaiVanDe.Trim().ToLower().Equals(TenLoaiVanDe.Trim().ToLower())))
            {
                return KetQuaTraVe.DaTonTai;
            }
            db.LoaiVanDes.Add(new LoaiVanDe()
            {
                TenLoaiVanDe = TenLoaiVanDe
            });
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
        public KetQuaTraVe ThayDoiTenLoaiVanDe(int pMaLoaiVanDe, string pTenLoaiVanDe)
        {
            var LoaiVanDe = db.LoaiVanDes.FirstOrDefault(x => x.MaLoaiVanDe == pMaLoaiVanDe);
            if (LoaiVanDe == null)
                return KetQuaTraVe.KhongTonTai;
            LoaiVanDe.TenLoaiVanDe = pTenLoaiVanDe;
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
        public KetQuaTraVe XoaLoaiVanDe(int pMaLoaiVanDe)
        {
            var LoaiVanDe = db.LoaiVanDes.FirstOrDefault(x => x.MaLoaiVanDe == pMaLoaiVanDe);
            if (LoaiVanDe == null)
                return KetQuaTraVe.KhongTonTai;
            if (db.ChamSocKhachHangs.Any(x => x.MaLoaiVanDe == pMaLoaiVanDe))
                return KetQuaTraVe.KhongDuocPhep;
            db.LoaiVanDes.Remove(LoaiVanDe);
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
