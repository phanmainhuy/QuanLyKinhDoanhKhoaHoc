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
    
    public partial class LoaiKhoaHoc
    {
        public string MaLoai { get; set; }
        public string MaDanhMuc { get; set; }
        public string TenLoai { get; set; }
    
        public virtual DanhMucKhoaHoc DanhMucKhoaHoc { get; set; }
    }
}
