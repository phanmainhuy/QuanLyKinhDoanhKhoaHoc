using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KhoaHocData.EF;

namespace KhoaHocData.DAO
{
    public class KhoaHocDAO
    {
        QL_KHOAHOCEntities db;
        public KhoaHocDAO()
        {
            db = new QL_KHOAHOCEntities();
        }

        public KhoaHoc LayKhoaHocTheoMa(string maKhoaHoc)
        {
            return db.KhoaHocs.Where(x=>x.MaKhoaHoc == maKhoaHoc).SingleOrDefault();
        }

        public IEnumerable<KhoaHoc> LayRaToanBoKhoaHoc()
        {
            return db.KhoaHocs.OrderBy(x => x.MaKhoaHoc);
        }
        public IEnumerable<KhoaHoc> LayRaToanBoKhoaHocTheoTrang(int page, int pageSize, out int totalRow)
        {
            int skipCount = page * pageSize;
            totalRow =(int)Math.Ceiling((float)db.KhoaHocs.Count() / pageSize);
            return db.KhoaHocs.OrderByDescending(x => x.MaKhoaHoc).Skip(skipCount);
        }
        public IEnumerable<KhoaHoc> LayRaKhoaHocTheoMaLoaiKhoaHoc(string maLoai)
        {
            return db.KhoaHocs.Where(x => x.MaLoai == maLoai);
        }
    }
}
