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
    
    public partial class TichDiem
    {
        public string MaND { get; set; }
        public Nullable<int> SoDiem { get; set; }
    
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
