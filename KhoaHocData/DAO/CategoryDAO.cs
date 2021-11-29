using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class CategoryDAO
    {
        QL_KHOAHOCEntities db;
        public CategoryDAO()
        {
            db = new QL_KHOAHOCEntities();
        }
        public IEnumerable<DanhMucKhoaHoc> LayHetDanhMucKhoaHoc()
        {
            return db.DanhMucKhoaHocs;
        }
        public DanhMucKhoaHoc LayDanhMucKhoaHocTheoMa(int pMaDanhMuc)
        {
            return db.DanhMucKhoaHocs.SingleOrDefault(x => x.MaDanhMuc == pMaDanhMuc);
        }
        public IEnumerable<DanhMucKhoaHoc> LayDanhMucKhoaHoc()
        {
            return db.DanhMucKhoaHocs.Where(x=>true).OrderByDescending(x => x.MaDanhMuc);
        }
        public IEnumerable<LoaiKhoaHoc> LayLoaiKhoaHocTheoMaDanhMuc(int pMaDanhMuc)
        {
            return db.LoaiKhoaHocs.Where(x => x.MaDanhMuc == pMaDanhMuc);
        }
        public AllEnum.KetQuaTraVeDanhMuc TaoDanhMuc(string pTenDanhMuc)
        {
            if (db.DanhMucKhoaHocs.Any(x => x.TenDanhMuc.Trim().ToLower() == pTenDanhMuc.Trim().ToLower()))
                return AllEnum.KetQuaTraVeDanhMuc.DanhMucDaTonTai;
            db.DanhMucKhoaHocs.Add(new DanhMucKhoaHoc()
            {
                TenDanhMuc = pTenDanhMuc
            });
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVeDanhMuc.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVeDanhMuc.ThatBai;
            }
        }
        public AllEnum.KetQuaTraVeDanhMuc ThemTheLoai(int pMaDanhMuc, string pTenTheLoai)
        {
            if (!db.DanhMucKhoaHocs.Any(x => x.MaDanhMuc == pMaDanhMuc))
                return AllEnum.KetQuaTraVeDanhMuc.DanhMucKhongTonTai;
            if (!db.LoaiKhoaHocs.Any(x => x.TenLoai == pTenTheLoai.Trim().ToLower()))
                return AllEnum.KetQuaTraVeDanhMuc.TheLoaiDaTonTai;
            db.LoaiKhoaHocs.Add(new LoaiKhoaHoc()
            {
                MaDanhMuc = pMaDanhMuc,
                TenLoai = pTenTheLoai
            });
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVeDanhMuc.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVeDanhMuc.ThatBai;
            }
        }
    }
}
