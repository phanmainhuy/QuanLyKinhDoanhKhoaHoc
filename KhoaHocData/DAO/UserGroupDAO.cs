using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class UserGroupDAO
    {
        QL_KHOAHOCEntities db;
        public UserGroupDAO()
        {
            db = new QL_KHOAHOCEntities();
        }
        public IEnumerable<NhomNguoiDung> LayTatCaNhomNguoiDung()
        {
            return db.NhomNguoiDungs;
        }
    }
}
