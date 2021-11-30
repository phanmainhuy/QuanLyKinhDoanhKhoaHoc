using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class KhoaHocDAO
    {
        private QL_KHOAHOCEntities db;

        public KhoaHocDAO()
        {
            db = new QL_KHOAHOCEntities();
        }

        public KhoaHoc LayKhoaHocTheoMa(int maKhoaHoc)
        {
            return db.KhoaHocs.Where(x => x.MaKhoaHoc == maKhoaHoc).SingleOrDefault();
        }

        public IEnumerable<KhoaHoc> LayRaToanBoKhoaHoc()
        {
            return db.KhoaHocs.OrderBy(x => x.MaKhoaHoc);
        }

        public IEnumerable<KhoaHoc> LayRaToanBoKhoaHocTheoTrang(int page, int pageSize, out int totalRow)
        {
            int skipCount = page * pageSize;
            totalRow = (int)Math.Ceiling((float)db.KhoaHocs.Count() / pageSize);
            return db.KhoaHocs.OrderByDescending(x => x.MaKhoaHoc).Skip(skipCount);
        }
        public IEnumerable<KhoaHoc> LayRaKhoaHocTheoMaLoaiKhoaHocPaging(int maLoai, int page, int pageSize, out int total)
        {
            int skipSize = pageSize * (page - 1);
            var result = db.KhoaHocs.Where(x => x.MaLoai == maLoai).ToList();
            total = result.Count();
            return result.Skip(skipSize).Take(pageSize).ToList();
        }

        public IEnumerable<KhoaHoc> LayRaKhoaHocTheoMaLoaiKhoaHoc(int maLoai, int limit)
        {
            return db.KhoaHocs.Where(x => x.MaLoai == maLoai).OrderByDescending(x => x.MaKhoaHoc).Take(limit);
        }
        public IEnumerable<KhoaHoc> LayKhoaHocTheoMaLoaiKhoaHocPaging(int maLoai, int page, int pageSize, out int total)
        {
            int skipSize = (page - 1) * pageSize;
            
            var items = db.KhoaHocs.Where(x => x.MaLoai == maLoai).OrderByDescending(x => x.MaKhoaHoc).ToList();
            total = items.Count();
            return items.Skip(skipSize).Take(pageSize).ToList();
        }

        public IEnumerable<KhoaHoc> LayKhoaHocMoiNhat(int limit)
        {
            return db.KhoaHocs.OrderByDescending(x => x.MaKhoaHoc).Take(limit);
        }

        public IEnumerable<KhoaHoc> LayKhoaHocMuaNhieu(int pLimit)
        {
            return db.KhoaHocs.OrderByDescending(x => x.SoLuongMua.Value).Take(pLimit);
        }

        public IEnumerable<KhoaHoc> TimKiemKhoaHoc(string pSearchString)
        {
            if (String.IsNullOrEmpty(pSearchString))
                return db.KhoaHocs;
            var item = db.SearchKhoaHoc(pSearchString);
            return (IEnumerable<KhoaHoc>)item;
        }

        public IEnumerable<KhoaHoc> TimKiemKhoaHocPaging(string pSearchString, out int total, int page, int pageSize)
        {
            int skipSize = (page-1) * pageSize;
            if (string.IsNullOrEmpty(pSearchString))
            {
                var result = db.KhoaHocs.OrderByDescending(x => x.MaKhoaHoc).ToList();
                total = result.Count();


                return result.Skip(skipSize).Take(pageSize).ToList();
            }
            var item = db.SearchKhoaHoc(pSearchString).OrderByDescending(x => x.MaKhoaHoc).ToList();
            total = item.Count();
            return item.Skip(skipSize).Take(pageSize).ToList();
        }
        public IEnumerable<KhoaHoc> TimKiemKhoaHocPagingSorting(string pSearchString, out int total, int page, int pageSize, int type)
        {
            int skipSize = (page - 1) * pageSize;
            if (string.IsNullOrEmpty(pSearchString))
            {
                var result = SortingBy(db.KhoaHocs.OrderByDescending(x => x.MaKhoaHoc).ToList(), (SortingType)type);
                total = result.Count();
                return result.Skip(skipSize).Take(pageSize).ToList();
            }
            var item = SortingBy(db.SearchKhoaHoc(pSearchString).OrderByDescending(x => x.MaKhoaHoc).ToList(), (SortingType)type);
            total = item.Count();
            return item.Skip(skipSize).Take(pageSize).ToList();
        }
        public IEnumerable<KhoaHoc> SortingBy(IEnumerable<KhoaHoc> input, SortingType type)
        {
            if (type == SortingType.Cheapest)
                return input.OrderBy(x => x.DonGia);
            if (type == SortingType.HighestRate)
                return input.OrderByDescending(x => x.DanhGiaKhoaHoc.Diem);
            if (type == SortingType.MostExpensive)
                return input.OrderByDescending(x => x.DonGia);
            if (type == SortingType.MostLearn)
                return input.OrderByDescending(x => x.SoLuongMua);
            return input.OrderByDescending(x => x.MaKhoaHoc);
        }
        public IEnumerable<KhoaHoc> TimKiemKhoaHocTheoTheLoai(int pMaTheLoai, string pSearchString)
        {
            if (string.IsNullOrEmpty(pSearchString))
                return db.KhoaHocs.Where(x => x.MaLoai == pMaTheLoai).OrderByDescending(x => x.MaKhoaHoc);
            var item = db.SearchKhoaHocTheoTheLoai(pMaTheLoai, pSearchString);
            return item;
        }

        public IEnumerable<KhoaHoc> TimKiemKhoaHocTheoTheLoaiPaging(
            int pMaTheLoai,
            string pSearchString,
            out int total,
            int page = 1,
            int pageSize = 12)
        {
            if (string.IsNullOrEmpty(pSearchString))
            {
                var result = db.KhoaHocs.Where(x => x.MaLoai == pMaTheLoai).OrderByDescending(x => x.MaKhoaHoc).ToList();
                total = result.Count();
                return result;
            }
            int SkipSize = (page-1) * pageSize;
            var item = db.SearchKhoaHocTheoTheLoai(pMaTheLoai, pSearchString).OrderByDescending(x => x.MaKhoaHoc).ToList();
            total = item.Count();
            return item.Skip(SkipSize ).Take(pageSize).ToList();
        }
        public IEnumerable<KhoaHoc> TimKiemKhoaHocTheoTheLoaiPagingSorting(
            int pMaTheLoai,
            string pSearchString,
            out int total,
            int page,
            int pageSize,
            int type)
        {
            if (string.IsNullOrEmpty(pSearchString))
            {
                var result = SortingBy(db.KhoaHocs.Where(x => x.MaLoai == pMaTheLoai).OrderByDescending(x => x.MaKhoaHoc).ToList(), (SortingType)type);
                total = result.Count();
                return result;
            }
            int SkipSize = (page-1) * pageSize;
            var item = SortingBy(db.SearchKhoaHocTheoTheLoai(pMaTheLoai, pSearchString).OrderByDescending(x => x.MaKhoaHoc).ToList(), (SortingType)type);
            total = item.Count();
            return item.Skip(SkipSize ).Take(pageSize).ToList();
        }
        public AllEnum.KetQuaTraVeKhoaHoc ThemKhoaHoc(int pMaLoai, string pTenKhoaHoc, decimal pDonGia, string pHinhAnh, int pMaGV, string pMoTa)
        {
            if (!db.LoaiKhoaHocs.Any(x => x.MaLoai == pMaLoai))
                return KetQuaTraVeKhoaHoc.TheLoaiKhongTonTai;
            if (db.KhoaHocs.SingleOrDefault(x => x.TenKhoaHoc.ToLower() == pTenKhoaHoc.Trim().ToLower()) != null)
                return KetQuaTraVeKhoaHoc.DaTonTai;
            db.KhoaHocs.Add(new KhoaHoc()
            {
                MaLoai = pMaLoai,
                TenKhoaHoc = pTenKhoaHoc,
                DonGia = pDonGia,
                HinhAnh = pHinhAnh,
                MaGV = pMaGV,
                MOTAKHOAHOC = pMoTa,
                SoLuongMua = 0,
                TrangThai = false,
                NgayTao = DateTime.Now.Date,
                NgayChapThuan = DateTime.MinValue.Date
            });
            try
            {
                db.SaveChanges();
                return KetQuaTraVeKhoaHoc.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVeKhoaHoc.ThatBai;
            }
        }
    }
}