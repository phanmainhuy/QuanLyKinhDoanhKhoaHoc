using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class MenuDAO
    {
        QL_KHOAHOCEntities db;
        public MenuDAO()
        {
            db = new QL_KHOAHOCEntities();
        }
        public IEnumerable<LOAIQUYEN> LayTatCaLoaiQuyen()
        {
            return db.LOAIQUYENs.Where(x=>true).OrderBy(x=>x.MaLoaiQuyen).ToList();
        }
    }
}
