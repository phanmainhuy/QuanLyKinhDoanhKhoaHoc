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
    
    public partial class KhoaHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhoaHoc()
        {
            this.Chuongs = new HashSet<Chuong>();
            this.DanhGiaKhoaHocs = new HashSet<DanhGiaKhoaHoc>();
        }
    
        public int MaKhoaHoc { get; set; }
        public Nullable<int> MaLoai { get; set; }
        public string TenKhoaHoc { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public Nullable<int> ThoiHanHoanTien { get; set; }
        public Nullable<bool> TrangThai { get; set; }
        public string HinhAnh { get; set; }
        public Nullable<int> MaGV { get; set; }
        public string MOTAKHOAHOC { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<System.DateTime> NgayChapThuan { get; set; }
        public Nullable<bool> HienThi { get; set; }
        public Nullable<int> SoLuongMua { get; set; }
        public Nullable<bool> DaXoa { get; set; }
    
        public virtual NguoiDung NguoiDung { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chuong> Chuongs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGiaKhoaHoc> DanhGiaKhoaHocs { get; set; }
    }
}
