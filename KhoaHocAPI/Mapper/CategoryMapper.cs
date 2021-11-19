﻿using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaHocAPI.Mapper
{
    public static class CategoryMapper
    {
        public static TheLoaiVM MapCategory(LoaiKhoaHoc lkh)
        {
            return new TheLoaiVM()
            {
                MaTheLoai = lkh.MaLoai,
                TenTheLoai = lkh.TenLoai
            };
        }
        public static IEnumerable<TheLoaiVM> MapListCategory(IEnumerable<LoaiKhoaHoc> lkhs)
        {
            List<TheLoaiVM> lstReturned = new List<TheLoaiVM>();
            foreach(var item in lkhs)
            {
                lstReturned.Add(MapCategory(item));
            }
            return lstReturned;
        }
        public static DanhMucVM MapTopCategory(DanhMucKhoaHoc dm)
        {
            return new DanhMucVM()
            {
                MaDanhMuc = dm.MaDanhMuc,
                DanhSachTheLoai = MapListCategory(dm.LoaiKhoaHocs).ToList(),
                TenDanhMuc = dm.TenDanhMuc,
                HinhAnh = dm.HinhAnh
            };
        }
        public static IEnumerable<DanhMucVM> MapListTopCategory(IEnumerable<DanhMucKhoaHoc> lkhs)
        {
            List<DanhMucVM> lstReturned = new List<DanhMucVM>();
            foreach (var item in lkhs.ToList())
            {
                lstReturned.Add(MapTopCategory(item));
            }
            return lstReturned;
        }
    }
}