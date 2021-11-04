using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<KhoaHoc> LayRaKhoaHocTheoMaLoaiKhoaHoc(int maLoai)
        {
            return db.KhoaHocs.Where(x => x.MaLoai == maLoai);
        }

        public IEnumerable<KhoaHoc> LayKhoaHocMoiNhat(int limit)
        {
            return db.KhoaHocs.OrderByDescending(x => x.MaKhoaHoc).Take(limit);
        }

        public IEnumerable<KhoaHoc> LayKhoaHocMuaNhieu(int pLimit)
        {
            return db.KhoaHocs.OrderByDescending(x => LayTongTienMua(x.MaKhoaHoc)).Take(pLimit);
        }

        //private method
        private decimal LayTongTienMua(int pMaKhoaHoc)
        {
            return db.CT_HoaDon.Where(x => x.MaKhoaHoc == pMaKhoaHoc).Sum(x => x.DonGia.Value);
        }
    }
}