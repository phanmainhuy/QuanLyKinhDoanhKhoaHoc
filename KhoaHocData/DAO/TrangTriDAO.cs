using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class TrangTriDAO
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        public TrangTri LayTrangTri(int pMaTrangTri)
        {
            return db.TrangTris.FirstOrDefault(x => x.MaTrangTri == pMaTrangTri);
        }
        public IEnumerable<TrangTri> LayTrangTriTheoLoai(int pMaLoaiTrangTri)
        {
            return db.TrangTris.Where(x => x.MaLoaiTrangTri == pMaLoaiTrangTri).ToList();
        }
        public KetQuaTraVe ThemTrangTri(int pMaLoaiTrangTri, string pValue)
        {
            if (!db.LoaiTrangTris.Any(x => x.MaLoaiTrangTri == pMaLoaiTrangTri))
                return KetQuaTraVe.ChaKhongTonTai;
            if (db.TrangTris.Any(x => x.MaLoaiTrangTri == pMaLoaiTrangTri && x.GiaTri == pValue))
                return KetQuaTraVe.DaTonTai;
            db.TrangTris.Add(new TrangTri()
            {
                MaLoaiTrangTri = pMaLoaiTrangTri,
                GiaTri = pValue
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
        public KetQuaTraVe ThayDoiGiaTriTrangTri(int pMaTrangTri, string pValue)
        {
            var tt = db.TrangTris.FirstOrDefault(x => x.MaTrangTri == pMaTrangTri);
            if (tt == null)
                return KetQuaTraVe.KhongTonTai;
            if (tt.GiaTri == pValue)
                return KetQuaTraVe.ThanhCong;
            tt.GiaTri = pValue;
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
        public KetQuaTraVe XoaTrangTri(int pMaTrangTri)
        {
            var tt= db.TrangTris.FirstOrDefault(x => x.MaTrangTri == pMaTrangTri);
            if (tt == null)
                return KetQuaTraVe.KhongTonTai;

            db.TrangTris.Remove(tt);
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
