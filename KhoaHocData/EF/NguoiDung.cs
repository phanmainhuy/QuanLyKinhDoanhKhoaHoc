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
    
    public partial class NguoiDung
    {
        public string MaND { get; set; }
        public string TenDN { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string MaNhomNguoiDung { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string Diachi { get; set; }
    
        public virtual NhomNguoiDung NhomNguoiDung { get; set; }
        public virtual TichDiem TichDiem { get; set; }
    }
}
