using KhoaHocData.EF;
using System.Collections.Generic;
using System.Linq;

namespace KhoaHocData.DAO
{
    public class Account
    {
        private QL_KHOAHOCEntities db;

        public Account()
        {
            db = new QL_KHOAHOCEntities();
        }

        public NguoiDung Login(string username, string password)
        {
            return db.NguoiDungs.SingleOrDefault(x => x.TenDN == username && x.MatKhau == password);
        }
        public int[] GetRoles(string pTenDN)
        {
            int nhomnd = db.NguoiDungs.Single(x => x.TenDN == pTenDN).MaNhomNguoiDung.Value;
            return new int[] { nhomnd };
        }
    }
}