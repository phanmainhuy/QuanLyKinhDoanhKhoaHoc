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
    
    public partial class CT_DonHang
    {
        public int MaCTDH { get; set; }
        public Nullable<int> MaDonHang { get; set; }
        public Nullable<decimal> DonGia { get; set; }
    
        public virtual DonHang DonHang { get; set; }
    }
}
