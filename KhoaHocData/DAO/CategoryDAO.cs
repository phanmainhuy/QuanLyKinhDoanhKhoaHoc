using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class CategoryDAO
    {
        private QL_KHOAHOCEntities db;

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
            return db.DanhMucKhoaHocs.Where(x => true).OrderByDescending(x => x.MaDanhMuc);
        }

        public IEnumerable<LoaiKhoaHoc> LayLoaiKhoaHocTheoMaDanhMuc(int pMaDanhMuc)
        {
            return db.LoaiKhoaHocs.Where(x => x.MaDanhMuc == pMaDanhMuc);
        }

        public AllEnum.KetQuaTraVe TaoDanhMuc(string pTenDanhMuc, string pHinhAnh)
        {
            if (db.DanhMucKhoaHocs.Any(x => x.TenDanhMuc.Trim().ToLower() == pTenDanhMuc.Trim().ToLower()))
                return KetQuaTraVe.DaTonTai;
            var dm = new DanhMucKhoaHoc();
            dm.TenDanhMuc = pTenDanhMuc;
            if (!string.IsNullOrEmpty(pHinhAnh))
                dm.HinhAnh = pHinhAnh;
            db.DanhMucKhoaHocs.Add(dm);
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }

        public AllEnum.KetQuaTraVe SuaThongTinDanhMuc(int pMaDanhMuc, string pTenDanhMuc, string pHinhAnh)
        {
            var dm = db.DanhMucKhoaHocs.SingleOrDefault(x => x.MaDanhMuc == pMaDanhMuc);
            if (dm == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            if (dm.TenDanhMuc == pTenDanhMuc && dm.HinhAnh == pHinhAnh)
                return AllEnum.KetQuaTraVe.ThanhCong;
            dm.TenDanhMuc = pTenDanhMuc;
            dm.HinhAnh = pHinhAnh;
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }

        public KetQuaTraVe XoaDanhMuc(int pMaDM)
        {
            var dm = db.DanhMucKhoaHocs.SingleOrDefault(x => x.MaDanhMuc == pMaDM);
            if (dm == null)
                return KetQuaTraVe.KhongTonTai;
            if (db.LoaiKhoaHocs.Any(x => x.MaDanhMuc == dm.MaDanhMuc))
                return KetQuaTraVe.KhongDuocPhep;
            db.DanhMucKhoaHocs.Remove(dm);
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe DoiTrangThaiDanhMuc(List<int> pMaDM, bool HienThi)
        {
            var dms = db.DanhMucKhoaHocs;
            foreach (var item in dms.ToList())
            {
                if(pMaDM.Contains(item.MaDanhMuc))
                    item.HienThi = HienThi;
            }
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe DoiTrangThaiTheLoai(List<int> pMaTL, bool HienThi)
        {
            var dms = db.LoaiKhoaHocs;
            foreach (var item in dms.ToList())
            {
                if (pMaTL.Contains(item.MaLoai))
                    item.HienThi = HienThi;
            }
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }

        public AllEnum.KetQuaTraVe ThemTheLoai(int pMaDanhMuc, string pTenTheLoai)
        {
            if (!db.DanhMucKhoaHocs.Any(x => x.MaDanhMuc == pMaDanhMuc))
                return KetQuaTraVe.ChaKhongTonTai;
            if (db.LoaiKhoaHocs.Any(x => x.TenLoai == pTenTheLoai.Trim().ToLower()))
                return KetQuaTraVe.DaTonTai;
            db.LoaiKhoaHocs.Add(new LoaiKhoaHoc()
            {
                MaDanhMuc = pMaDanhMuc,
                TenLoai = pTenTheLoai
            });
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }

        public KetQuaTraVe ThayDoiThongTinTheLoai(int pMaTheLoai, string pTenTheLoai)
        {
            var tl = db.LoaiKhoaHocs.SingleOrDefault(x => x.MaLoai == pMaTheLoai);
            if (tl == null)
                return KetQuaTraVe.KhongTonTai;
            if (tl.TenLoai == pTenTheLoai)
            {
                return KetQuaTraVe.ThanhCong;
            }
            tl.TenLoai = pTenTheLoai;
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe XoaTheLoai(int pMaTheLoai)
        {
            var tl = db.LoaiKhoaHocs.SingleOrDefault(x => x.MaLoai == pMaTheLoai);
            if (tl == null)
                return KetQuaTraVe.KhongTonTai;
            if (db.KhoaHocs.Any(x => x.MaLoai == pMaTheLoai))
                return KetQuaTraVe.KhongDuocPhep;
            db.LoaiKhoaHocs.Remove(tl);
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
    }
}