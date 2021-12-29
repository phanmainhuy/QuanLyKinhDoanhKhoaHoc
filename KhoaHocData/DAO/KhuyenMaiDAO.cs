using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class KhuyenMaiDAO
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        XuLy xl = new XuLy();
        public IEnumerable<KhuyenMai> LayToanBoKhuyenMai()
        {
            return db.KhuyenMais.OrderByDescending(x=>x.MaKM).ToList();
        }
        public KhuyenMai LayKhuyenMaiTheoMa(int pMaKM)
        {
            return db.KhuyenMais.SingleOrDefault(x => x.MaKM == pMaKM);
        }
        public KhuyenMai Lay1KhuyenMaiTheoMaNguoiDung(int pMaND, int pMaKM)
        {
            return db.KhuyenMais.SingleOrDefault(x =>x.MaND == pMaND && x.MaKM == pMaKM);
        }
        public int LayDiemTheoNguoiDung(int pMaND)
        {
            var diem = db.TichDiems.FirstOrDefault(x => x.MaND == pMaND);
            return diem == null ? 0 : diem.SoDiem.Value;
        }
        public AllEnum.KetQuaTraVe ThayDoiTrangThaiBanKhuyenMai(List<int> pListMaKM, bool pTrangThai)
        {
            var lstKhuyenMai = db.KhuyenMais.ToList();
            foreach (var item in lstKhuyenMai)
            {
                if (pListMaKM.Contains(item.MaKM))
                {
                    if (item == null)
                        continue;
                    item.DangMoBan = pTrangThai;
                }
            }
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
        public IEnumerable<KhuyenMai> LayTatCaKhuyenMaiTheoMaNguoiDung(int pMaND)
        {
            var lstKhuyenMaiKhachHang = db.KhuyenMai_KhachHang.Where(x => x.MaND == pMaND && (!x.IsSuDung.Value || x.IsSuDung == null)).ToList();
            List<KhuyenMai> lstReturn = new List<KhuyenMai>();
            foreach (var item in db.KhuyenMais.ToList())
            {
                if (lstKhuyenMaiKhachHang.Any(x => x.MaKM == item.MaKM))
                    lstReturn.Add(item);
            }
            return lstReturn;
        }
        public AllEnum.KetQuaTraVe ThemKhuyenMai(int pMaNguoiTao, string pTenKM, string pHinhAnh, 
            decimal pGiaTri, int DiemCanMua, int ThoiGianKeoDai)
        {
            string random = "";
            int length = 7;
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                random += r.Next(9);
            }
            KhuyenMai km = new KhuyenMai();
            km.MaND = pMaNguoiTao;
            if (!string.IsNullOrEmpty(pTenKM))
                km.TenKM = pTenKM;
            if (db.KhuyenMais.Any(x => x.TenKM.Trim().ToLower() == pTenKM.Trim().ToLower()))
                return AllEnum.KetQuaTraVe.DaTonTai;
            if (!string.IsNullOrEmpty(pHinhAnh))
            {
                pHinhAnh = "";
                km.HinhAnh = pHinhAnh;
            }
            if (string.IsNullOrEmpty(pHinhAnh))
            {
                km.HinhAnh = "defaultcoupon.png";
            }
            km.MaApDung = random;
            km.Diem = DiemCanMua;
            km.ThoiGianKeoDai = ThoiGianKeoDai == 0 ? 10 :ThoiGianKeoDai;
            km.GiaTri = pGiaTri;
            km.DangMoBan = false;
            db.KhuyenMais.Add(km);
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
        public AllEnum.KetQuaTraVe XoaKhuyenMai(int pMaKM)
        {
            var km = db.KhuyenMais.FirstOrDefault(x => x.MaKM == pMaKM);
            if (km == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            if (km.KhuyenMai_KhachHang.Any(x=>x.NgayKetThuc <= DateTime.Today && (x.IsSuDung == null?false:!x.IsSuDung.Value)))
                return AllEnum.KetQuaTraVe.KhongDuocPhep;
            db.KhuyenMais.Remove(km);
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;

            }
        }
        public AllEnum.KetQuaTraVe MuaKhuyenMai(int pMaND, int pMaKM)
        {
            if (db.KhuyenMai_KhachHang.Any(x => x.MaND == pMaND && x.MaKM == pMaKM && x.IsSuDung.Value))
                return AllEnum.KetQuaTraVe.DaTonTai;
            var km = db.KhuyenMais.SingleOrDefault(x => x.MaKM == pMaKM);
            var diem = db.TichDiems.SingleOrDefault(x => x.MaND == pMaND);
            if (diem == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            else if (diem.SoDiem.Value < km.Diem.Value)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            KhuyenMai_KhachHang km_kh = new KhuyenMai_KhachHang();
            km_kh.MaKM = pMaKM;
            km_kh.MaND = pMaND;
            km_kh.NgayBatDau = DateTime.Now.Date;
            km_kh.NgayKetThuc = DateTime.Now.Date.AddDays(db.KhuyenMais.SingleOrDefault(x => x.MaKM == pMaKM).ThoiGianKeoDai.Value);
            km_kh.IsSuDung = false;
            db.KhuyenMai_KhachHang.Add(km_kh);
            diem.SoDiem -= km.Diem.Value;
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
        public AllEnum.KetQuaTraVe XoaKhuyenMaiDaMua(int pMaND, int pMaKM)
        {
            var item = db.KhuyenMai_KhachHang.SingleOrDefault(x => x.MaND == pMaND && x.MaKM == pMaKM);
            if(item == null)
                return AllEnum.KetQuaTraVe.ThanhCong;

            db.KhuyenMai_KhachHang.Remove(item);
            try
            {
                db.SaveChanges();
                return AllEnum.KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
        }
        public decimal ApDungKhuyenMai(int pMaND, string pMaApDung)
        {
            var mkm = db.KhuyenMais.FirstOrDefault(x => x.MaApDung == pMaApDung);
            if (db.KhuyenMai_KhachHang.Any(x => x.MaND == pMaND && mkm.MaKM == x.MaKM && (x.IsSuDung == null || !x.IsSuDung.Value)))
                return mkm.GiaTri == null? 0:mkm.GiaTri.Value;
            return -1;
        }
        //public IEnumerable<KhuyenMai> LayToanBoKhuyenMaiConDungDuoc()
        //{
        //    return db.KhuyenMais.Where(x=>x.).ToList();
        //}
    }
}
