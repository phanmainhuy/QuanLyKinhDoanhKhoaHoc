using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class SalaryMapper
    {
        public static SalaryVM MapSalary(Luong luong)
        {
            return new SalaryVM()
            {
                MaLuong = luong.MaLuong,
                TongLuong = luong.Luong1.Value,
                MaND = luong.MaND.Value,
                DanhSachLichSu = MapListSalaryHistoryItem(luong.LichSuLuongs).ToList()
            };
        }
        public static SalaryHistoryItemVM MapSalaryHistoryItem(LichSuLuong lsl)
        {
            return new SalaryHistoryItemVM()
            {
                MaLuong = lsl.MaLuong,
                GhiChu = lsl.GhiChu,
                NgayPhatLuong = lsl.NgayPhatLuong,
                SoTien = lsl.SoTien.Value,
                TienPhat = lsl.TienPhat.Value
            };
        }
        public static IEnumerable<SalaryHistoryItemVM> MapListSalaryHistoryItem(IEnumerable<LichSuLuong> lstModel)
        {
            List<SalaryHistoryItemVM> lstReturn = new List<SalaryHistoryItemVM>();
            foreach (var item in lstModel.ToList())
            {
                lstReturn.Add(MapSalaryHistoryItem(item));
            }
            return lstReturn;
        }
    }
}