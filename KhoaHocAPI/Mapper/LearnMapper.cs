using KhoaHocAPI.Models;
using KhoaHocData.EF;
using System.Linq;

namespace KhoaHocAPI.Mapper
{
    public static class LearnMapper
    {
        public static LearnVM MapLearnVM(KhoaHoc khoahoc)
        {
            return new LearnVM()
            {
                MaKH = khoahoc.MaKhoaHoc,
                TenKH = khoahoc.TenKhoaHoc,
                DanhSachChuong = UnitMapper.MapListUnit(khoahoc.Chuongs.ToList())
            };
        }
    }
}