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
        public AllEnum.KetQuaTraVe ThemKhuyenMai(int pMaNguoiTao, string pTenKM, string pHinhAnh, decimal pGiaTri)
        {
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
            km.GiaTri = pGiaTri;
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
        public AllEnum.KetQuaTraVe MuaKhuyenMai(int pMaND, int pMaKM)
        {
            if (db.KhuyenMai_KhachHang.Any(x => x.MaND == pMaND && x.MaKM == pMaKM))
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
        //public IEnumerable<KhuyenMai> LayToanBoKhuyenMaiConDungDuoc()
        //{
        //    return db.KhuyenMais.Where(x=>x.).ToList();
        //}
    }
}
