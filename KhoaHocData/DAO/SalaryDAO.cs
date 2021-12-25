using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class SalaryDAO
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();

        
        public Luong getLichSuLuong(int pMaND)
        {
            return db.Luongs.FirstOrDefault(x => x.MaND == pMaND);
        }
        public KetQuaTraVe ThemLuong(int pMaLuong, decimal pTienPhat = 0, string pGhiChu = "")
        {
            var Luong = db.Luongs.FirstOrDefault(x => x.MaLuong == pMaLuong);
            if (Luong == null)
                return KetQuaTraVe.KhongTonTai;
            var lsl = new LichSuLuong();
            lsl.MaLuong = Luong.MaLuong;
            lsl.NgayPhatLuong = DateTime.Today.Date;
            lsl.TienPhat = pTienPhat;
            lsl.SoTien = Luong.Luong1 - pTienPhat;
            lsl.GhiChu = pGhiChu;

            db.LichSuLuongs.Add(lsl);
            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
        public KetQuaTraVe ThayDoiLichSuLuong(int pMaLuong, DateTime pNgayPhatLuong, decimal pTienPhat, string pGhiChu)
        {
            var luong = db.Luongs.FirstOrDefault(x => x.MaLuong == pMaLuong);
            if (luong == null)
                return KetQuaTraVe.ChaKhongTonTai;
            var lsl = db.LichSuLuongs.FirstOrDefault(x => x.MaLuong == pMaLuong && x.NgayPhatLuong == pNgayPhatLuong);
            if (lsl == null)
                return KetQuaTraVe.KhongTonTai;
            
            lsl.TienPhat = pTienPhat;
            lsl.SoTien = luong.Luong1 - pTienPhat;
            lsl.GhiChu = pGhiChu;

            try
            {
                db.SaveChanges();
                return KetQuaTraVe.ThanhCong;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return KetQuaTraVe.ThatBai;
            }
        }
    }
}
