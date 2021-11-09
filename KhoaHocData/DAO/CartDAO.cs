using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class CartDAO
    {
        QL_KHOAHOCEntities db;
        public CartDAO()
        {
            db = new QL_KHOAHOCEntities();
        }
        private int AddCart(int pMaND)
        {
            GioHang oldCart = null;
            if (db.GioHangs.Count() < 0) { return -1; }
            oldCart = db.GioHangs.SingleOrDefault(x => x.MaND == pMaND);
            if (oldCart == null)
            {
                GioHang gh = new GioHang();
                gh.MaND = pMaND;
                db.GioHangs.Add(gh);
                try
                {
                    db.SaveChanges();
                    var addedItem = db.GioHangs.SingleOrDefault(x => x.MaND == gh.MaND);
                    if (addedItem != null)
                        return addedItem.MaGioHang;
                    else
                        return -1;
                }
                catch
                {
                    return -1;
                }
            }
            return oldCart.MaGioHang;
        }
        public bool TangTongTienCart(int pMaGioHang, decimal pTongTien)
        {
            if (pTongTien <= 0)
                return false;
            var Cart = db.GioHangs.SingleOrDefault(x => x.MaGioHang == pMaGioHang);
            if (Cart != null)
            {
                Cart.TongTien += pTongTien;
                return true;
            }
            else
                return false;
        }
        public AddCartItemResult AddCartItem(int pMaND, int pMaKhoaHoc, decimal pDonGia)
        {
            int AddCartResult = AddCart(pMaND);
            if (AddCartResult != -1)
            {
                var oldCartItem = db.CT_GioHang.SingleOrDefault(x => x.MaGioHang == AddCartResult && x.MaKhoaHoc == pMaKhoaHoc);
                if (oldCartItem != null)
                    return AddCartItemResult.DaTonTai;
                var CTGH = new CT_GioHang()
                {
                    MaKhoaHoc = pMaKhoaHoc,
                    DonGia = pDonGia,
                    MaGioHang = AddCartResult
                };
                db.CT_GioHang.Add(CTGH);
                try
                {
                    db.SaveChanges();
                    var item = db.GioHangs.SingleOrDefault(x => x.MaGioHang == CTGH.MaGioHang);
                    if (item.TongTien == null)
                        item.TongTien = CTGH.DonGia.Value;
                    else
                        item.TongTien += CTGH.DonGia.Value;
                    db.SaveChanges();
                    return AddCartItemResult.ThanhCong;
                }
                catch
                {
                    return AddCartItemResult.ThatBai;
                }

            }
            else
                return AddCartItemResult.ThemGioHangThatbai;
        }

        public bool DeleteCartItem(int maGioHang, int maKhoaHoc)
        {
            var item = db.CT_GioHang.SingleOrDefault(x => x.MaGioHang == maGioHang && x.MaKhoaHoc == maKhoaHoc);
            if(item  != null)
            {
                try
                {
                    db.GioHangs.Single(x => x.MaGioHang == item.MaGioHang).TongTien -= item.DonGia;
                    db.CT_GioHang.Remove(item);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public GioHang LayGioHangTheoID(int pId)
        {
            return db.GioHangs.SingleOrDefault(x => x.MaGioHang == pId);
        }
        public GioHang LayGioHangTheoUserID(int pUserId)
        {
            return db.GioHangs.SingleOrDefault(x => x.MaND == pUserId);
        }
        public IEnumerable<CT_GioHang> LayDanhSachTrongGioHang(int pMaGioHang)
        {
            return db.CT_GioHang.Where(x => x.MaGioHang == pMaGioHang);
        }
    }
}
