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
            if (nd == null)
                return false;
            return db.Quyen_NhomNguoiDung.Any(x => x.MaNhomNguoiDung == nd.MaNhomNguoiDung && x.MaQuyen == pMaQuyen);
        }

        public IEnumerable<Quyen> LayTatCaQuyen()
        {
            return db.Quyens;
        }

        public async Task<bool> ThemNhomNguoiDung(string pTenNhomNguoiDung, IEnumerable<Quyen> pDanhSachQuyen)
        {
            NhomNguoiDung nnd = new NhomNguoiDung();
            nnd.TenNhomNguoiDung = pTenNhomNguoiDung;
            db.NhomNguoiDungs.Add(nnd);
            await db.SaveChangesAsync();
            List<Quyen_NhomNguoiDung> lstAdd = new List<Quyen_NhomNguoiDung>();
            foreach (var item in pDanhSachQuyen.ToList())
            {
                Quyen_NhomNguoiDung qnnd = new Quyen_NhomNguoiDung()
                {
                    MaNhomNguoiDung = nnd.MaNhomNguoiDung,
                    MaQuyen = item.MaQuyen
                };
                lstAdd.Add(qnnd);
            }
            db.Quyen_NhomNguoiDung.AddRange(lstAdd);
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

        public async Task<IEnumerable<NguoiDung>> ThayDoiNhomNhieuNguoiDung(IEnumerable<NguoiDung> Users, int pMaNhom)
        {
            List<NguoiDung> lstReturned = new List<NguoiDung>();
            foreach (var item in Users)
            {
                var nd = db.NguoiDungs.SingleOrDefault(x => x.MaND == item.MaND);
                if (nd == null)
                    continue;
                nd.MaNhomNguoiDung = pMaNhom;
                lstReturned.Add(nd);
            }
            try
            {
                await db.SaveChangesAsync();
                return lstReturned;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<Quyen> GetRolesByGroupID(int pMaNhomNguoiDung)
        {
            var lstQuyenNND = db.Quyen_NhomNguoiDung.Where(x => x.MaNhomNguoiDung == pMaNhomNguoiDung).Select(x=>x.MaQuyen);
            List<Quyen> lstQuyen = new List<Quyen>();
            lstQuyen.AddRange(db.Quyens.Where(x => lstQuyenNND.Contains(x.MaQuyen)));
            return lstQuyen;
        }
    }
}