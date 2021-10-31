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
        public LoginResult DangKy(NguoiDung nd)
        {
            var item = db.NguoiDungs.SingleOrDefault(x => x.TenDN == nd.TenDN);
            if (item != null)
                return LoginResult.DaTonTai;
            else
            {
                try
                {
                db.NguoiDungs.Add(nd);
                db.SaveChanges();
                return LoginResult.ThanhCong;

                }
                catch
                {
                    return LoginResult.ThatBai;
                }
            }
        }
        public IEnumerable<NguoiDung> LayHetNguoiDung()
        {
            return db.NguoiDungs.ToList();
        }
        public IEnumerable<NguoiDung> LayNguoiDungPaging(int pPage, int pPageSize, out int pTotalPage)
        {
            int SkipCount = pPage * pPageSize;
            pTotalPage = db.NguoiDungs.Count();
            return db.NguoiDungs.Skip(SkipCount - 1).ToList();
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
            return db.NguoiDungs.Where(x => x.MaNhomNguoiDung == pMaNhomNguoiDung);
        }
        public NguoiDung LayNguoiDungTheoId(int pMaNguoiDung)
        {
            return db.NguoiDungs.Find(pMaNguoiDung);
        }
    }
    public enum LoginResult
    {
        DaTonTai,
        ThanhCong,
        ThatBai
    }
}
