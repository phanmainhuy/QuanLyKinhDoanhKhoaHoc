//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KhoaHocData.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonThuTien
    {
        public int MaDonThuTien { get; set; }
        public Nullable<int> MaKH { get; set; }
        public Nullable<int> MaNVHoTro { get; set; }
        public string DonViThuHo { get; set; }
        public string DiaChiThu { get; set; }
        public Nullable<decimal> SoTienThu { get; set; }
        public Nullable<decimal> PhiThuHo { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<System.DateTime> NgayDuKienThu { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public string SDTThu { get; set; }
        public Nullable<int> MaHD { get; set; }
        public string Email { get; set; }
    
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual NguoiDung NguoiDung1 { get; set; }
    }
}
