using Common;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public static class GetDAO
    {
        private static QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        
        public static NguoiDung GetGiaoVienTheoMa(int pMaGV)
        {
            return db.NguoiDungs
                .Where(x => x.MaND == (int)AllEnum.MaNhomNguoiDung.Teacher)
                                    .SingleOrDefault(x => x.MaND == pMaGV);
        }
        public static decimal GetDanhGiaKhoaHoc(int pMaKhoaHoc)
        {
            
            int TongDanhGia = 0;
            int SoLuongDanhGia = 0;
            TongDanhGia = db.DanhGiaKhoaHocs.Where(x=>x.MaKhoaHoc == pMaKhoaHoc).Sum(x => x.Diem).Value;
            SoLuongDanhGia = db.DanhGiaKhoaHocs.Where(x => x.MaKhoaHoc == pMaKhoaHoc).Count();
            return (decimal)TongDanhGia / SoLuongDanhGia;

        }
    }
}
