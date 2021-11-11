using KhoaHocData.EF;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public bool hasPermission(int pMaND, int pMaQuyen)
        {
            var nd = db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND);
            if(nd == null)
                return false;
            var nhoms = db.NhomNguoiDungs.Where(x => x.Quyens.Any(y =>y.MaQuyen == pMaQuyen)).ToList();
            if(nhoms != null)
            {
                if(nhoms.Any(x=>x.MaNhomNguoiDung == nd.MaNhomNguoiDung))
                {
                    return true;
                }
            }
            return false;
        }
        public IEnumerable<Quyen> LayTatCaQuyen()
        {
            return db.Quyens;
        }
        public async Task<bool> ThemNhomNguoiDung(string pTenNhomNguoiDung, IEnumerable<Quyen> pDanhSachQuyen)
        {
            NhomNguoiDung nnd = new NhomNguoiDung();
            nnd.TenNhomNguoiDung = pTenNhomNguoiDung;
            foreach(var item in pDanhSachQuyen.ToList())
            {
                var tempRole = db.Quyens.SingleOrDefault(x => x.MaQuyen == item.MaQuyen);
                if (tempRole == null)
                    continue;
                nnd.Quyens.Add(tempRole);
            }
            db.NhomNguoiDungs.Add(nnd);

            return (await db.SaveChangesAsync()) > 0;
        }
        public async Task<bool> ThayDoiNhomNguoiDung(int pMaND, int pMaNhom)
        {
            var nd = db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND);
            if (nd == null)
                return false;
            try
            {
                nd.MaNhomNguoiDung = pMaNhom;
                return await db.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }


        }
    }
}