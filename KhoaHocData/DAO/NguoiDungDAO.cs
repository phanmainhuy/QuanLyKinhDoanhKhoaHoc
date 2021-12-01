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
            return db.NguoiDungs.ToList();
        }
        public IEnumerable<NguoiDung> LayHetHeThong()
        {
            return db.NguoiDungs.Where(x => x.MaNhomNguoiDung != (int)MaNhomNguoiDung.Student).OrderBy(x => x.MaNhomNguoiDung);
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
        //public bool CoQuyenChinhSua(int pMaND, int pMaQuyen)
    }
    
}
