using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class BaiHocDAO
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        public AllEnum.KetQuaTraVe ThemChuong(int pMaKhoaHoc, string pTenChuong)
        {
            if (!db.KhoaHocs.Any(x => x.MaKhoaHoc == pMaKhoaHoc))
            {
                return AllEnum.KetQuaTraVe.ChaKhongTonTai;
            }
            if (db.Chuongs.SingleOrDefault(x => x.MaKhoaHoc == pMaKhoaHoc && x.TenChuong.Trim().ToLower() == pTenChuong.Trim().ToLower()) != null)
            {
                return AllEnum.KetQuaTraVe.DaTonTai;
            }
            db.Chuongs.Add(new Chuong()
            {
                MaKhoaHoc = pMaKhoaHoc,
                TenChuong = pTenChuong
            });
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
        public AllEnum.KetQuaTraVe XoaChuong(int pMaChuong)
        {
            var item = db.Chuongs.SingleOrDefault(x => x.MaChuong == pMaChuong);
            if (item == null)
            {
                return AllEnum.KetQuaTraVe.KhongTonTai;
            }
            try
            {
                if (item.BaiHocs.Count() > 0)
                {
                    foreach (var item2 in item.BaiHocs.ToList())
                    {
                        XoaBaiHoc(item2.MaBaiHoc);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AllEnum.KetQuaTraVe.ThatBai;
            }
            db.Chuongs.Remove(item);
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
        public AllEnum.KetQuaTraVe SuaThongTinChuong(int pMaChuong, string pTenChuong)
        {
            Chuong chuong = db.Chuongs.SingleOrDefault(x => x.MaChuong == pMaChuong);
            if (chuong == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            if (!string.IsNullOrEmpty(pTenChuong))
                chuong.TenChuong= pTenChuong;
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
        public AllEnum.KetQuaTraVe ThemBaiHoc(int pMaChuong, string pTenBaiHoc, string pVideoName)
        {
            if (!db.Chuongs.Any(x => x.MaChuong == pMaChuong))
            {
                return AllEnum.KetQuaTraVe.ChaKhongTonTai;
            }
            if (db.BaiHocs.Any(x => x.MaChuong == pMaChuong && x.TenBaiHoc.Trim().ToLower() == pTenBaiHoc.Trim().ToLower()))
            {
                return AllEnum.KetQuaTraVe.DaTonTai;
            }
            BaiHoc bh = new BaiHoc();
            bh.MaChuong = pMaChuong;
            if (!string.IsNullOrEmpty(pTenBaiHoc))
                bh.TenBaiHoc = pTenBaiHoc;
            if (!string.IsNullOrEmpty(pVideoName))
                bh.VideoLink = pVideoName;
            db.BaiHocs.Add(bh);
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
        public AllEnum.KetQuaTraVe SuaThongTinBaiHoc(int pMaBaiHoc, string pTenBaiHoc, string pVideoName)
        {
            BaiHoc bh = db.BaiHocs.SingleOrDefault(x => x.MaBaiHoc == pMaBaiHoc);
            if (bh == null)
                return AllEnum.KetQuaTraVe.KhongTonTai;
            if (!string.IsNullOrEmpty(pTenBaiHoc))
                bh.TenBaiHoc = pTenBaiHoc;
            if (!string.IsNullOrEmpty(pVideoName))
                bh.VideoLink = pVideoName;
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
        public AllEnum.KetQuaTraVe XoaBaiHoc(int pMaBaiHoc)
        {
            var item = db.BaiHocs.SingleOrDefault(x => x.MaBaiHoc == pMaBaiHoc);
            if (item == null)
            {
                return AllEnum.KetQuaTraVe.KhongTonTai;
            }
            if(item.BaiTaps.Count()>0)
            {
                db.BaiTaps.RemoveRange(item.BaiTaps);
            }

            db.BaiHocs.Remove(item);
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
        public BaiHoc LayBaiHoc(int pMaBaiHoc)
        {
            return db.BaiHocs.SingleOrDefault(x => x.MaBaiHoc == pMaBaiHoc);
        }
        
        public List<BaiHoc> LayToanBoBaiHoc()
        {
            return db.BaiHocs.ToList();
        }
        public List<BaiHoc> LayBaiHocTheoChuong(int pMaChuong)
        {
            return db.BaiHocs.Where(x => x.MaChuong == pMaChuong).ToList();
        }
        public List<Chuong> LayChuongTheoKhoaHoc(int pMaKhoaHoc)
        {
            return db.Chuongs.Where(x => x.MaKhoaHoc == pMaKhoaHoc).ToList();
        }
            
    }
}
