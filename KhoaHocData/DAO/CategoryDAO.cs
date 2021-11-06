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
        public IEnumerable<DanhMucKhoaHoc> LayDanhMucKhoaHoc()
        {
            return db.DanhMucKhoaHocs.Where(x=>true).OrderByDescending(x => x.MaDanhMuc);
        }
        public DanhMucKhoaHoc LayDanhMucKhoaHocTheoMa(int pMaDanhMuc)
        {
            return db.DanhMucKhoaHocs.SingleOrDefault(x => x.MaDanhMuc == pMaDanhMuc);
        }
        public IEnumerable<LoaiKhoaHoc> LayLoaiKhoaHocTheoMaDanhMuc(int pMaDanhMuc)
        {
            return db.LoaiKhoaHocs.Where(x => x.MaDanhMuc == pMaDanhMuc);
        }

        

    }
}
