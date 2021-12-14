using Common;
using KhoaHocData.EF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace KhoaHocData.DAO
{
    public class GetDAO
    {
        private QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();

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
        public string GetTenNguoiDung(int pMaND)
        {
            return db.NguoiDungs.SingleOrDefault(x => x.MaND == pMaND).HoTen;
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
                    commandDB.CommandText = "use master restore database QL_KHOAHOC FROM DISK ='" + fileName + "'";
                    commandDB.ExecuteNonQuery();
                    commandDB.CommandText = "ALTER DATABASE [QL_KhoaHoc] SET MULTI_USER";
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}