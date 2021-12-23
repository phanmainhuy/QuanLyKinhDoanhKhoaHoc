using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class DanhGiaDAO
    {
        private QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        public IEnumerable<DanhGiaKhoaHoc> LayDanhGiaKhoaHocTheoMaKhoaHoc(int pMaKhoaHoc)
        {
            var item = db.DanhGiaKhoaHocs.ToList();
            return db.DanhGiaKhoaHocs.Where(x => x.MaKhoaHoc == pMaKhoaHoc).ToList();
        }
        public KetQuaTraVe ThemMoiDanhGia(int pMaND, int pMaKhoaHoc, string pNoiDung, int pDiem)
        {
            if (db.KhoaHocCuaToi(pMaND).ToList().Where(x => x.MaKhoaHoc == pMaKhoaHoc).Count() == 0)
            {
                return KetQuaTraVe.KhongDuocPhep;
            }
            if (db.DanhGiaKhoaHocs.Any(x => x.MaND == pMaND && x.MaKhoaHoc == pMaKhoaHoc))
                return KetQuaTraVe.DaTonTai;
            if (pDiem > 5 || pDiem <= 0)
            {
                return KetQuaTraVe.KhongHopLe;
            }
            DanhGiaKhoaHoc dg = new DanhGiaKhoaHoc();
            dg.MaND = pMaND;
            dg.MaKhoaHoc = pMaKhoaHoc;
            dg.NoiDung = pNoiDung;
            dg.Diem = pDiem;
            db.DanhGiaKhoaHocs.Add(dg);
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
        public KetQuaTraVe ThayDoiDanhGia(int pMaND, int pMaKhoaHoc, string pNoiDung, int pDiem)
        {
            if (db.KhoaHocCuaToi(pMaND).ToList().Where(x => x.MaKhoaHoc == pMaKhoaHoc).Count() == 0)
            {
                return KetQuaTraVe.KhongDuocPhep;
            }
            if (pDiem > 5 || pDiem <= 0)
            {
                return KetQuaTraVe.KhongHopLe;
            }

            DanhGiaKhoaHoc dg = db.DanhGiaKhoaHocs.Where(x => x.MaND == pMaND && x.MaKhoaHoc == pMaKhoaHoc).FirstOrDefault();
            if (dg == null)
                return KetQuaTraVe.KhongTonTai;
            dg.NoiDung = pNoiDung;
            dg.Diem = pDiem;
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
        public KetQuaTraVe XoaDanhGia(int pMaND, int pMaKhoaHoc)
        {
            DanhGiaKhoaHoc dg = db.DanhGiaKhoaHocs.Where(x => x.MaND == pMaND && x.MaKhoaHoc == pMaKhoaHoc).FirstOrDefault();
            if (dg == null)
                return KetQuaTraVe.KhongTonTai;
            db.DanhGiaKhoaHocs.Remove(dg);
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
        public DanhGiaKhoaHoc LayDanhGiaDaDanhGia(int pMaND, int pMaKhoaHoc)
        {
            return db.DanhGiaKhoaHocs.FirstOrDefault(x => x.MaND == pMaND && x.MaKhoaHoc == pMaKhoaHoc);
        }
    }
}
