using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class UnitMapper
    {
        public static BaiHocVM MapLesson(BaiHoc item)
        {
            return new BaiHocVM()
            {
                MaBaiHoc = item.MaBaiHoc,
                MaChuong = item.MaChuong.Value,
                TenBaiHoc = item.TenBaiHoc,
                TenChuong = item.Chuong.TenChuong
            };
        }
        public static List<BaiHocVM> MapListLesson(IEnumerable<BaiHoc> lstBaiHoc)
        {
            List<BaiHocVM> lstReturn = new List<BaiHocVM>();
            foreach (var item in lstBaiHoc)
            {
                lstReturn.Add(MapLesson(item));
            }
            return lstReturn;
        }
        public static ChuongVM MapUnit(Chuong item)
        {
            return new ChuongVM()
            {
                MaChuong = item.MaChuong,
                TenChuong = item.TenChuong,
                MaKhoaHoc = item.MaKhoaHoc.Value,
                TenKhoaHoc = new GetDAO().GetKhoaHocTheoMa(item.MaKhoaHoc.Value).TenKhoaHoc,
                DanhSachBaiHoc = MapListLesson(item.BaiHocs.ToList())
            };
        }
        public static List<ChuongVM> MapListUnit(IEnumerable<Chuong> lstChuong)
        {
            List<ChuongVM> lstReturn = new List<ChuongVM>();
            foreach (var item in lstChuong)
            {
                lstReturn.Add(MapUnit(item));
            }
            return lstReturn;
        }
    }
}