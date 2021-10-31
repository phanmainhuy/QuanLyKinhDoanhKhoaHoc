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
    }
}
