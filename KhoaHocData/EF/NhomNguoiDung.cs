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
    
    public partial class NhomNguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhomNguoiDung()
        {
            this.NguoiDungs = new HashSet<NguoiDung>();
            this.Quyen_NhomNguoiDung = new HashSet<Quyen_NhomNguoiDung>();
        }
    
        public int MaNhomNguoiDung { get; set; }
        public string TenNhomNguoiDung { get; set; }
        public string HinhAnh { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quyen_NhomNguoiDung> Quyen_NhomNguoiDung { get; set; }
    }
}
