using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class BaiTapDAO
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        public IEnumerable<BaiTap> LayBaiTapTheoBaiHoc(int pMaBaiHoc)
        {
            return db.BaiTaps.Where(x => x.MaBaiHoc == pMaBaiHoc).ToList();
        }
        public KetQuaTraVe ThemBaiTap(int pMaBaiHoc, string PDF)
        {
            if (!db.BaiHocs.Any(x => x.MaBaiHoc == pMaBaiHoc))
            {
                return KetQuaTraVe.ChaKhongTonTai;
            }
            BaiTap bt = new BaiTap();
            bt.MaBaiHoc = pMaBaiHoc;
            bt.FileLink = PDF;
            db.BaiTaps.Add(bt);
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
        public KetQuaTraVe XoaBaiTap(int pMaBaiTap)
        {
            var bt = db.BaiTaps.FirstOrDefault(x => x.MaBaiTap == pMaBaiTap);
            if (bt == null)
                return KetQuaTraVe.KhongTonTai;
            db.BaiTaps.Remove(bt);
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
