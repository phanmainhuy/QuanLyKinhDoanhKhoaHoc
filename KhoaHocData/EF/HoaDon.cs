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
    
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            this.CT_HoaDon = new HashSet<CT_HoaDon>();
        }
    
        public int MaHD { get; set; }
        public Nullable<int> MaND { get; set; }
        public Nullable<int> MaKM { get; set; }
        public Nullable<decimal> GiamGia { get; set; }
        public Nullable<decimal> TongTien { get; set; }
        public Nullable<System.DateTime> NgayLap { get; set; }
        public Nullable<bool> ThanhToan { get; set; }
        public string TrangThai { get; set; }
        public string HinhThucThanhToan { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_HoaDon> CT_HoaDon { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }
}