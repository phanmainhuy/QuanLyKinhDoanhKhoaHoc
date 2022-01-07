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
        public KetQuaTraVe ThemChamSocKhachHang(int pMaLoaiVanDe, int pMaNhanVien, string SDT, string TenKH, string TinhTrang, string NoiDung)
        {
            ChamSocKhachHang cskh = new ChamSocKhachHang();
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
                throw;
            }
        }
        //public KetQuaTraVe ThayDoiThongTinChamSocKhachHang(int pMaCSKH, string SDT, string TenKH, string TinhTrang, string NoiDung)
        //{
        //    var cskh = db.ChamSocKhachHangs.FirstOrDefault(x => x.MaCSKH == pMaCSKH);
        //    if (cskh == null)
        //        return KetQuaTraVe.KhongTonTai;

        //    if(string.IsNullOrEmpty(SDT))
        //        cskh.SDTKH = SDT;
        //    if (string.IsNullOrEmpty(TenKH))
        //        cskh.TenKH = TenKH;
        //    if (string.IsNullOrEmpty(NoiDung))
        //        cskh.NoiDung = NoiDung;
        //}
    }
}
