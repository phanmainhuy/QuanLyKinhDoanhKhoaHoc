using Common;
using KhoaHocData.EF;
using System;
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

        public NguoiDung Register(string userName, string Password)
        {
            NguoiDung nd = new NguoiDung()
            {
                TenDN = userName,
                MatKhau = Password
            };
            nd.MaNhomNguoiDung = (int)AllEnum.MaNhomNguoiDung.Student;
            db.NguoiDungs.Add(nd);
            try
            {
                db.SaveChanges();
                return nd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public NguoiDung RegisterEmployee(  string userName, int pMaNhomNguoiDung, string HoTen, string CMND,
                                            string HinhAnh, string SDT, string Email, DateTime NgaySinh, 
                                            string DiaChi)
        {
            NguoiDung nd = new NguoiDung()
            {
                TenDN = userName,
                MatKhau = userName
            };
            
            nd.MaNhomNguoiDung = pMaNhomNguoiDung;
            nd.HoTen = HoTen;
            nd.CMND = CMND;
            nd.HinhAnh = HinhAnh;
            nd.SDT = SDT;
            nd.Email = Email;
            nd.NgaySinh = NgaySinh;
            nd.Diachi = DiaChi;
            db.NguoiDungs.Add(nd);
            try
            {
                db.SaveChanges();
                return nd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
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
            if (db.NhomNguoiDungs.Any(x => x.TenNhomNguoiDung.Trim().ToLower() == pTenNhomNguoiDung.Trim().ToLower()))
                return false;
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
            if(lstAdd.Count() > 0)
                db.Quyen_NhomNguoiDung.AddRange(lstAdd);
            try
            {
                 await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> ThayDoiQuyenCuaNhom(int pMaNhom, IEnumerable<Quyen> lstQuyen)
        {
            var nhom = db.NhomNguoiDungs.SingleOrDefault(x => x.MaNhomNguoiDung == pMaNhom);
            if (nhom == null)
                return false;
            var lstQuyenNhomNguoiDung = db.Quyen_NhomNguoiDung.Where(x => x.MaNhomNguoiDung == pMaNhom).ToList();
            List<Quyen_NhomNguoiDung> newListQuyenNhomNguoiDung = new List<Quyen_NhomNguoiDung>();
            db.Quyen_NhomNguoiDung.RemoveRange(lstQuyenNhomNguoiDung);
            foreach (var item in lstQuyen.ToList())
            {
                newListQuyenNhomNguoiDung.Add(new Quyen_NhomNguoiDung()
                {
                    MaQuyen = item.MaQuyen,
                    MaNhomNguoiDung = pMaNhom,
                    DuocTruyCap = true
                });
            }
            try
            {
                await db.SaveChangesAsync();
                db.Quyen_NhomNguoiDung.AddRange(newListQuyenNhomNguoiDung);
                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;

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
        public IEnumerable<Quyen> GetRolesByUserID(int pMaND)
        {
            var MaNhomNguoiDung = db.NguoiDungs.Single(x => x.MaND == pMaND).MaNhomNguoiDung;
            var lstQuyenNND = db.Quyen_NhomNguoiDung.Where(x => x.MaNhomNguoiDung == MaNhomNguoiDung).Select(x => x.MaQuyen);
            List<Quyen> lstQuyen = new List<Quyen>();
            lstQuyen.AddRange(db.Quyens.Where(x => lstQuyenNND.Contains(x.MaQuyen)));
            return lstQuyen;
        }
        public async Task<AllEnum.KetQuaTraVe> XoaNhomQuyen(int pMaNhomNguoiDung)
        {
            if (pMaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Admin ||
                pMaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Employee ||
                pMaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Student ||
                pMaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Teacher
                )
                return AllEnum.KetQuaTraVe.KhongDuocPhep;
            var nhomquyen = db.NhomNguoiDungs.SingleOrDefault(x => x.MaNhomNguoiDung == pMaNhomNguoiDung);
            if (nhomquyen == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            if (db.NguoiDungs.Any(x => x.MaNhomNguoiDung == pMaNhomNguoiDung))
                return AllEnum.KetQuaTraVe.DaTonTai;
            db.Quyen_NhomNguoiDung.RemoveRange(db.Quyen_NhomNguoiDung.Where(x => x.MaNhomNguoiDung == pMaNhomNguoiDung));
            await db.SaveChangesAsync();
            db.NhomNguoiDungs.Remove(nhomquyen);
            try
            {
                await db.SaveChangesAsync();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
    }
}