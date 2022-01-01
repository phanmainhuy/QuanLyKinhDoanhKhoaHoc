using Common;
using KhoaHocData.EF;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Data.Entity;
using Database = Microsoft.SqlServer.Management.Smo.Database;

namespace KhoaHocData.DAO
{
    public class GetDAO
    {
        private QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        public DateTime? GetNgayTaoHoaDon(int pMaND, int pMaKH)
        {
            return db.NgayMuaKhoaHoc(pMaND, pMaKH).FirstOrDefault();
        }
        public NguoiDung GetGiaoVienTheoMa(int pMaGV)
        {
            var item = db.NguoiDungs
                .Where(x => x.MaND == (int)AllEnum.MaNhomNguoiDung.Teacher)
                                    .SingleOrDefault(x => x.MaND == pMaGV);
            if (item == null)
                return db.NguoiDungs.Single(x => x.MaND == 0);
            else
                return item;
        }
        public KhuyenMai GetKhuyenMaiTheoMa(int? pMaKm)
        {
            if (pMaKm == null)
                return null;
            return db.KhuyenMais.FirstOrDefault(x => x.MaKM == pMaKm);
        }
        public int LayTongKhoaHocCuaDanhMuc(int pMaDM)
        {
            var dm = db.DanhMucKhoaHocs.FirstOrDefault(x => x.MaDanhMuc == pMaDM);
            if (dm == null)
                return 0;
            var lstTheLoai = db.LoaiKhoaHocs.Where(x => x.MaDanhMuc == dm.MaDanhMuc);
            var khoahoc = db.KhoaHocs.Where(x => lstTheLoai.Any(y => x.MaLoai == y.MaLoai)).ToList();
            return khoahoc.Count();
        }
        public string GetTenNguoiDung(int pMaND)
        {
            return db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND).HoTen;
        }
        public string GetTenKhoaHoc(int pMaKhoaHoc)
        {
            return db.KhoaHocs.FirstOrDefault(x=>x.MaKhoaHoc == pMaKhoaHoc).TenKhoaHoc;
        }
        public string GetHinhAnhNguoiDung(int pMaND)
        {
            var nd = db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND);
            if (nd.HinhAnh == null)
                return "userdefault.png";
            return nd.HinhAnh;
        }
        public decimal GetDanhGiaKhoaHoc(int pMaKhoaHoc)
        {
            int TongDanhGia = 0;
            int SoLuongDanhGia = 0;
            try
            {
                TongDanhGia = db.DanhGiaKhoaHocs.Where(x => x.MaKhoaHoc == pMaKhoaHoc).Sum(x => x.Diem).Value;
                SoLuongDanhGia = db.DanhGiaKhoaHocs.Where(x => x.MaKhoaHoc == pMaKhoaHoc).Count();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return (decimal)TongDanhGia / SoLuongDanhGia;
        }
        public DonThuTien GetDonThuTienTheoMaHoaDon(int pMaHoaDon)
        {
            return db.DonThuTiens.SingleOrDefault(x => x.MaHD == pMaHoaDon);
        }
        public int? LayDiemTheoMaNguoiDung(int pMaND)
        {
            var TichDiem = db.TichDiems.SingleOrDefault(x => x.MaND == pMaND);

            if (TichDiem != null)
            {
                return TichDiem.SoDiem;
            }
            else
            {
                TichDiem = new TichDiem();
                TichDiem.MaND = pMaND;
                TichDiem.SoDiem = 0;
                return 0;
            }

        }
        public decimal? LayLuongTheoMaNguoiDung(int pMaND)
        {
            var Luong = db.Luongs.SingleOrDefault(x => x.MaND == pMaND);

            if (Luong != null)
            {
                return Luong.Luong1;
            }
            else
            {
                Luong = new Luong();
                Luong.MaND = pMaND;
                Luong.Luong1 = 0;
                return 0;
            }
        }
        public KhoaHoc GetKhoaHocTheoMa(int pMaKhoaHoc)
        {
            return db.KhoaHocs.SingleOrDefault(x => x.MaKhoaHoc == pMaKhoaHoc);
        }
        public async Task<int> BackupDatabase(string fileName)
        {
            return await db.Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction, @"EXEC [dbo].[BackUpDataBase] @Path = N'" + fileName + "'");
        }
        public async Task<int> RestoreDatabase(string fileName)
        {
            try
            {
                using (var context = new QL_KHOAHOCEntities())
                using (var commandDB = context.Database.Connection.CreateCommand())
                {
                    commandDB.CommandText = "IF DB_ID('QL_KhoaHoc') IS NOT NULL ALTER DATABASE [QL_KhoaHoc] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    await context.Database.Connection.OpenAsync();
                    commandDB.ExecuteNonQuery();
                    commandDB.CommandText = "use master restore database QL_KHOAHOC FROM DISK ='" + fileName + "' WITH REPLACE";
                    commandDB.ExecuteNonQuery();
                    commandDB.CommandText = "ALTER DATABASE [QL_KhoaHoc] SET MULTI_USER";
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                using (var context = new QL_KHOAHOCEntities())
                using (var commandDB = context.Database.Connection.CreateCommand())
                {
                    commandDB.CommandText = "ALTER DATABASE [QL_KhoaHoc] SET MULTI_USER";
                    commandDB.ExecuteNonQuery();
                }
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public NhomNguoiDung GetNhomNguoiDung(int pMaNhomNguoiDung)
        {
            return db.NhomNguoiDungs.SingleOrDefault(x => x.MaNhomNguoiDung == pMaNhomNguoiDung);
        }
        public Luong GetLuong(int pMaND)
        {
            return db.Luongs.FirstOrDefault(x => x.MaND == pMaND);
        }
        public int GetSLKHTheoMaLoai(int pMaLoai)
        {
            return db.KhoaHocs.Where(x => x.MaLoai == pMaLoai).ToList().Count();
        }
        public List<BaiHoc> LayListBaiHocTheoChuong(int pMaChuong)
        {
            return db.BaiHocs.Where(x => x.MaChuong == pMaChuong).ToList();
        }
    }
}