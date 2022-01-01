using Common;
using Common.CommonModel;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.AllEnum;

namespace KhoaHocData.DAO
{
    public class ThongKeClass
    {
        QL_KHOAHOCEntities db = new QL_KHOAHOCEntities();
        public IEnumerable<MonthlyStatistic> ThongKeTheoTungThang(int Nam)
        {
            var items = db.HoaDons.Where(x => x.ThanhToan == true).OrderByDescending(x => x.MaHD).ToList();
            List<MonthlyStatistic> lstMonthly = new List<MonthlyStatistic>();
            for (int i = 1; i <= 12; i++)
            {
                MonthlyStatistic month = new MonthlyStatistic();
                month.Month = i;
                month.DoanhThu = 0;
                month.SoLuongKhoaHocDaBan = 0;
                foreach (var item in items.Where(x => x.NgayLap.Value.Month == i))
                {
                    month.SoLuongKhoaHocDaBan++;
                    month.DoanhThu += (double)item.TongTien.Value;
                }
                lstMonthly.Add(month);
            }
            return lstMonthly;
        }
        public IEnumerable<DailyAccessStatistic> ThongKeTruyCapTungNgay(DateTime start, DateTime end)
        {
            var items = db.NguoiDungs.Where(x => x.NgayTao >= start
            && x.NgayTao <= end
            && x.MaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Student).ToList();
            List<DailyAccessStatistic> lstDailyAccess = new List<DailyAccessStatistic>();
            if (start.Month != end.Month)
            {
                return lstDailyAccess;
            }
            for (int i = start.Day; i <= end.Day; i++)
            {
                DailyAccessStatistic dl = new DailyAccessStatistic();
                dl.Date = new DateTime(start.Year, start.Month, i);
                dl.NewStudent = 0;
                dl.CourseSellCount = 0;
                dl.CourseSellCount += db.HoaDons.Count(x => x.NgayLap.Value.Day == i && x.ThanhToan.Value);
                foreach (var item in items)
                {
                    if (item.NgayTao.Value.Day == i)
                        dl.NewStudent++;
                }
                lstDailyAccess.Add(dl);
            }
            return lstDailyAccess;
        }
        public IEnumerable<MonthlyAccessStatistic> ThongKeTruyCapTungThang(int Year)
        {
            var items = db.NguoiDungs.Where(x => x.NgayTao.Value.Year == Year
            && x.MaNhomNguoiDung == (int)AllEnum.MaNhomNguoiDung.Student).ToList();
            List<MonthlyAccessStatistic> lstDailyAccess = new List<MonthlyAccessStatistic>();
            for (int i = 1; i <= 12; i++)
            {
                MonthlyAccessStatistic dl = new MonthlyAccessStatistic();
                dl.Month = i;
                dl.NewStudent = 0;
                dl.CourseSellCount = 0;
                dl.CourseSellCount += db.HoaDons.Count(x => x.NgayLap.Value.Month == i && x.ThanhToan.Value);
                foreach (var item in items)
                {
                    if (item.NgayTao.Value.Month == i)
                        dl.NewStudent++;
                }
                lstDailyAccess.Add(dl);
            }
            return lstDailyAccess;
        }
        public IEnumerable<KhoaHocDailyStatistic> ThongKeKhoaHocTungNgay(DateTime start, DateTime end)
        {
            var items = db.HoaDons.Where(x => x.TrangThai != null && x.ThanhToan != null)
                                 .Where(x => x.TrangThai == 1.ToString() && x.ThanhToan.Value)
                                 .Where(x => x.NgayLap.Value >= start && x.NgayLap <= end);
            var items2 = db.CT_HoaDon.Where(x => items.Any(y => y.MaHD == x.MaHD)).ToList();
            var khoahocs = db.KhoaHocs.ToList();
            List<KhoaHocDailyStatistic> lstReturn = new List<KhoaHocDailyStatistic>();
            if (items.Count() == 0)
                return lstReturn;
            int startDay = 0;
            int maxDay = 0;
            int endDay = 0;
            DateTime tempDate;

            if (start.Month != end.Month)
            {
                startDay = start.Day;
                maxDay = DateTime.DaysInMonth(start.Year, start.Month);
                endDay = end.Day + maxDay;
            }
            else
            {
                startDay = start.Day;
                endDay = end.Day;
            }
            foreach (var item in khoahocs)
            {
                KhoaHocDailyStatistic daily = new KhoaHocDailyStatistic();
                daily.MaKH = item.MaKhoaHoc;
                daily.TenKH = item.TenKhoaHoc;
                for (int i = startDay; i <= endDay; i++)
                {
                    if (maxDay != 0)
                    {

                        if (i <= maxDay)
                        {
                            tempDate = new DateTime(start.Year, start.Month, i);
                        }
                        else
                        {
                            tempDate = new DateTime(end.Year, end.Month, i - maxDay);
                        }
                    }
                    else
                    {
                        tempDate = new DateTime(start.Year, start.Month, i);
                    }
                    daily.TongThu = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Sum(x => x.DonGia).Value;
                    daily.SoLuongDKMoi = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Count();
                }
                lstReturn.Add(daily);
            }
            return lstReturn;
        }
        public IEnumerable<KhoaHocDailyStatistic> ThongKeKhoaHocTungNgaySorting(DateTime start, DateTime end, SortingType type)
        {
            var items = db.HoaDons.Where(x => x.TrangThai != null && x.ThanhToan != null)
                                 .Where(x => x.TrangThai == 1.ToString() && x.ThanhToan.Value)
                                 .Where(x => x.NgayLap.Value >= start && x.NgayLap <= end);
            var items2 = db.CT_HoaDon.Where(x => items.Any(y => y.MaHD == x.MaHD)).ToList();
            var khoahocs = db.KhoaHocs.ToList();
            List<KhoaHocDailyStatistic> lstReturn = new List<KhoaHocDailyStatistic>();
            if (items.Count() == 0)
                return lstReturn;
            int startDay = 0;
            int maxDay = 0;
            int endDay = 0;

            if (start.Month != end.Month)
            {
                startDay = start.Day;
                maxDay = DateTime.DaysInMonth(start.Year, start.Month);
                endDay = end.Day + maxDay;
            }
            else
            {
                startDay = start.Day;
                endDay = end.Day;
            }
            foreach (var item in khoahocs)
            {
                KhoaHocDailyStatistic daily = new KhoaHocDailyStatistic();
                daily.MaKH = item.MaKhoaHoc;
                daily.TenKH = item.TenKhoaHoc;
                daily.TongThu = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Sum(x => x.DonGia).Value;
                daily.SoLuongDKMoi = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Count();
                lstReturn.Add(daily);
            }
            lstReturn = SortingBy(lstReturn, type).ToList();
            return lstReturn;
        }
        public IEnumerable<MonthlyKhoaHocStatistic> ThongKeKhoaHocTheoNam(int Year)
        {
            var items = db.HoaDons.Where(x => x.TrangThai != null && x.ThanhToan != null)
                                 .Where(x => x.TrangThai == 1.ToString() && x.ThanhToan.Value)
                                 .Where(x => x.NgayLap.Value.Year == Year);
            var items2 = db.CT_HoaDon.Where(x => items.Any(y => y.MaHD == x.MaHD)).ToList();
            var khoahocs = db.KhoaHocs.ToList();
            List<MonthlyKhoaHocStatistic> lstReturn = new List<MonthlyKhoaHocStatistic>();
            foreach (var item in khoahocs)
            {
                MonthlyKhoaHocStatistic monthly = new MonthlyKhoaHocStatistic();
                monthly.MaKH = item.MaKhoaHoc;
                monthly.TenKH = item.TenKhoaHoc;
                monthly.TongThu = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Sum(x => x.DonGia).Value;
                monthly.SoLuongDKMoi = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Count();
                lstReturn.Add(monthly);
            }
            return lstReturn;
        }
        public IEnumerable<MonthlyKhoaHocStatistic> ThongKeKhoaHocTheoNamSorting(int Year, SortingType type)
        {
            var items = db.HoaDons.Where(x => x.TrangThai != null && x.ThanhToan != null)
                                 .Where(x => x.TrangThai == 1.ToString() && x.ThanhToan.Value)
                                 .Where(x => x.NgayLap.Value.Year == Year);
            var items2 = db.CT_HoaDon.Where(x => items.Any(y => y.MaHD == x.MaHD)).ToList();
            var khoahocs = db.KhoaHocs.ToList();
            List<MonthlyKhoaHocStatistic> lstReturn = new List<MonthlyKhoaHocStatistic>();
            foreach (var item in khoahocs)
            {
                MonthlyKhoaHocStatistic monthly = new MonthlyKhoaHocStatistic();
                monthly.MaKH = item.MaKhoaHoc;
                monthly.TenKH = item.TenKhoaHoc;
                monthly.TongThu = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Sum(x => x.DonGia).Value;
                monthly.SoLuongDKMoi = items2.Where(x => x.MaKhoaHoc == item.MaKhoaHoc).Count();
                lstReturn.Add(monthly);
            }
            lstReturn = SortingByMonthly(lstReturn, type).ToList();
            return lstReturn;
        }


        public IEnumerable<KhoaHocDailyStatistic> SortingBy(IEnumerable<KhoaHocDailyStatistic> input, SortingType type)
        {
            if (type == SortingType.MostLearn)
                return input.OrderByDescending(x => x.SoLuongDKMoi);
            return input.OrderByDescending(x => x.TongThu);
        }
        public IEnumerable<MonthlyKhoaHocStatistic> SortingByMonthly(IEnumerable<MonthlyKhoaHocStatistic> input, SortingType type)
        {
            if (type == SortingType.MostLearn)
                return input.OrderByDescending(x => x.SoLuongDKMoi);
            return input.OrderByDescending(x => x.TongThu);
        }
        public IEnumerable<DailyRevenueStatistic> ThongKeDoanhThuTheoNgay(DateTime start, DateTime end)
        {
            var items = db.HoaDons.Where(x => x.TrangThai != null && x.ThanhToan != null)
                                 .Where(x => x.TrangThai == 1.ToString() && x.ThanhToan.Value)
                                 .Where(x => x.NgayLap.Value >= start && x.NgayLap <= end);
            var items2 = db.CT_HoaDon.Where(x => items.Any(y => y.MaHD == x.MaHD)).ToList();
            List<DailyRevenueStatistic> lstReturn = new List<DailyRevenueStatistic>();
            int startDay = 0;
            int maxDay = 0;
            int endDay = 0;

            if (start.Month != end.Month)
            {
                startDay = start.Day;
                maxDay = DateTime.DaysInMonth(start.Year, start.Month);
                endDay = end.Day + maxDay;
            }
            else
            {
                startDay = start.Day;
                endDay = end.Day;
            }
            if (maxDay > 0)
            {
                for (int i = startDay; i <= endDay; i++)
                {
                    DailyRevenueStatistic daily = new DailyRevenueStatistic();
                    if (i <= maxDay)
                    {
                        daily.Ngay = new DateTime(start.Year, start.Month, i);
                    }
                    else
                    {
                        daily.Ngay = new DateTime(end.Year, end.Month, i-maxDay);
                    }
                    daily.DoanhThu = (double)items.Where(x=>x.NgayLap == daily.Ngay).Count() == 0?0: (double)items.Where(x => x.NgayLap == daily.Ngay).Sum(x=>x.TongTien).Value;
                    daily.SoLuongKhoaHocDaBan = items.Where(x => x.NgayLap == daily.Ngay).Count() ==0?0: items.Where(x => x.NgayLap == daily.Ngay).Sum(x => x.CT_HoaDon.Count());
                    lstReturn.Add(daily);
                }
            }
            else
            {
                for (int i = startDay; i <= endDay; i++)
                {
                    DailyRevenueStatistic daily = new DailyRevenueStatistic();
                    daily.Ngay = new DateTime(start.Year, start.Month, i);
                    var tempVar = items.ToList().Where(x => x.NgayLap == daily.Ngay).Sum(x => x.TongTien);
                    if(tempVar == null || tempVar == 0)
                    {
                        daily.DoanhThu = 0;
                        daily.SoLuongKhoaHocDaBan = 0;
                    }
                    else
                    {
                        daily.DoanhThu = (double)tempVar.Value;
                        daily.SoLuongKhoaHocDaBan = items.Where(x => x.NgayLap == daily.Ngay).Sum(x => x.CT_HoaDon.Count());

                    }
                    lstReturn.Add(daily);
                }
            }
            return lstReturn;
        }
        public IEnumerable<SalaryModel> ThongKeLichSuLuong(DateTime month)
        {
            List<SalaryModel> lstReturn = new List<SalaryModel>();
            var lstLichLuLuong = db.LichSuLuongs.Where(x => x.NgayPhatLuong.Month == month.Month && x.NgayPhatLuong.Year == month.Year).ToList();
            var lstLuong = db.Luongs.ToList();
            var lstNguoiDung = db.NguoiDungs.Where(x => x.MaNhomNguoiDung != (int)AllEnum.MaNhomNguoiDung.Student).ToList();
            var lstNhomNguoiDung = db.NhomNguoiDungs.ToList();
            foreach (var item in lstLichLuLuong.ToList())
            {
                SalaryModel model = new SalaryModel();
                Luong luong = lstLuong.FirstOrDefault(x => x.MaLuong == item.MaLuong);
                NguoiDung nguoidung = lstNguoiDung.FirstOrDefault(x => x.MaND == luong.MaND);
                NhomNguoiDung nhom = lstNhomNguoiDung.FirstOrDefault(x => x.MaNhomNguoiDung == nguoidung.MaNhomNguoiDung);
                model.MaLuong = item.MaLuong;
                model.MaND = nguoidung.MaND;
                model.TenND = nguoidung.HoTen;
                model.SoTien = item.SoTien.Value;
                model.TienPhat = item.TienPhat.Value;
                model.GhiChu = item.GhiChu;
                model.TenChucVu = nhom.TenNhomNguoiDung;
                model.TongTien = luong.Luong1.Value;
                model.NgayPhatLuong = item.NgayPhatLuong;
                lstReturn.Add(model);
            }
            return lstReturn;
        }
    }
}
