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

        public bool Login(string username, string password)
        {
            return db.NguoiDungs.Any(x => x.TenDN == username && x.MatKhau == password);
        }
        public string[] GetRoles(string pTenDN)
        {
            string nhomnd = db.NguoiDungs.Single(x => x.TenDN == pTenDN).MaNhomNguoiDung;
            return new string[] { nhomnd };
        }
    }
}