namespace Common
{
    public class AllEnum
    {
        public enum KetQuaTraVeDanhMuc
        {
            ThatBai, ThanhCong, DanhMucKhongTonTai, DanhMucDaTonTai, TheLoaiDaTonTai
        }
        public enum KetQuaTraVeKhoaHoc
        {
            ThatBai, ThanhCong, TheLoaiKhongTonTai, DaTonTai, KhongTonTai
        }
        public enum KetQuaTraVe
        {
            ThatBai, ThanhCong, ChaKhongTonTai, DaTonTai, KhongTonTai
        }

        public enum MaNhomNguoiDung
        {
            Admin,
            Student,
            Teacher,
            Employee
        }
        public enum RegisterResult
        {
            DaTonTai,
            ThanhCong,
            ThatBai
        }
        public enum LoginResult
        {
            ThanhCong,
            SaiTaiKhoanMatKhau
        }
        public enum AddCartItemResult
        {
            ThanhCong, ThemGioHangThatbai, ThatBai, DaTonTai
        }
        public enum SortingType
        {
            MostLearn,
            HighestRate,
            Latest,
            Cheapest,
            MostExpensive
        }
    }
    
}