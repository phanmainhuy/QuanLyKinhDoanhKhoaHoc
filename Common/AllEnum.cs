namespace Common
{
    public class AllEnum
    {
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