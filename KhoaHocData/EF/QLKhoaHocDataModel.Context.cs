﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class QL_KHOAHOCEntities : DbContext
    {
        public QL_KHOAHOCEntities()
            : base("name=QL_KHOAHOCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BaiHoc> BaiHocs { get; set; }
        public virtual DbSet<BaiTap> BaiTaps { get; set; }
        public virtual DbSet<ChamSocKhachHang> ChamSocKhachHangs { get; set; }
        public virtual DbSet<CT_GioHang> CT_GioHang { get; set; }
        public virtual DbSet<CT_HoaDon> CT_HoaDon { get; set; }
        public virtual DbSet<DanhGiaKhoaHoc> DanhGiaKhoaHocs { get; set; }
        public virtual DbSet<DanhMucKhoaHoc> DanhMucKhoaHocs { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHocs { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }
        public virtual DbSet<KhuyenMai_KhachHang> KhuyenMai_KhachHang { get; set; }
        public virtual DbSet<LichSuLuong> LichSuLuongs { get; set; }
        public virtual DbSet<LoaiKhoaHoc> LoaiKhoaHocs { get; set; }
        public virtual DbSet<LOAIQUYEN> LOAIQUYENs { get; set; }
        public virtual DbSet<LoaiVanDe> LoaiVanDes { get; set; }
        public virtual DbSet<Luong> Luongs { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<NhomNguoiDung> NhomNguoiDungs { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<Quyen_NhomNguoiDung> Quyen_NhomNguoiDung { get; set; }
        public virtual DbSet<SoDuTaiKhoan> SoDuTaiKhoans { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TichDiem> TichDiems { get; set; }
        public virtual DbSet<DonThuTien> DonThuTiens { get; set; }
        public virtual DbSet<Chuong> Chuongs { get; set; }
    
        public virtual ObjectResult<KhoaHoc> SearchKhoaHoc(string tieuDe)
        {
            var tieuDeParameter = tieuDe != null ?
                new ObjectParameter("TieuDe", tieuDe) :
                new ObjectParameter("TieuDe", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<KhoaHoc>("SearchKhoaHoc", tieuDeParameter);
        }
    
        public virtual ObjectResult<KhoaHoc> SearchKhoaHoc(string tieuDe, MergeOption mergeOption)
        {
            var tieuDeParameter = tieuDe != null ?
                new ObjectParameter("TieuDe", tieuDe) :
                new ObjectParameter("TieuDe", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<KhoaHoc>("SearchKhoaHoc", mergeOption, tieuDeParameter);
        }
    
        public virtual ObjectResult<KhoaHoc> SearchKhoahocNangCao(string tenKhoaHoc, string tenTheLoai)
        {
            var tenKhoaHocParameter = tenKhoaHoc != null ?
                new ObjectParameter("TenKhoaHoc", tenKhoaHoc) :
                new ObjectParameter("TenKhoaHoc", typeof(string));
    
            var tenTheLoaiParameter = tenTheLoai != null ?
                new ObjectParameter("TenTheLoai", tenTheLoai) :
                new ObjectParameter("TenTheLoai", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<KhoaHoc>("SearchKhoahocNangCao", tenKhoaHocParameter, tenTheLoaiParameter);
        }
    
        public virtual ObjectResult<KhoaHoc> SearchKhoahocNangCao(string tenKhoaHoc, string tenTheLoai, MergeOption mergeOption)
        {
            var tenKhoaHocParameter = tenKhoaHoc != null ?
                new ObjectParameter("TenKhoaHoc", tenKhoaHoc) :
                new ObjectParameter("TenKhoaHoc", typeof(string));
    
            var tenTheLoaiParameter = tenTheLoai != null ?
                new ObjectParameter("TenTheLoai", tenTheLoai) :
                new ObjectParameter("TenTheLoai", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<KhoaHoc>("SearchKhoahocNangCao", mergeOption, tenKhoaHocParameter, tenTheLoaiParameter);
        }
    
        public virtual ObjectResult<KhoaHoc> SearchKhoaHocTheoTheLoai(Nullable<int> maTheLoai, string tieuDe)
        {
            var maTheLoaiParameter = maTheLoai.HasValue ?
                new ObjectParameter("MaTheLoai", maTheLoai) :
                new ObjectParameter("MaTheLoai", typeof(int));
    
            var tieuDeParameter = tieuDe != null ?
                new ObjectParameter("TieuDe", tieuDe) :
                new ObjectParameter("TieuDe", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<KhoaHoc>("SearchKhoaHocTheoTheLoai", maTheLoaiParameter, tieuDeParameter);
        }
    
        public virtual ObjectResult<KhoaHoc> SearchKhoaHocTheoTheLoai(Nullable<int> maTheLoai, string tieuDe, MergeOption mergeOption)
        {
            var maTheLoaiParameter = maTheLoai.HasValue ?
                new ObjectParameter("MaTheLoai", maTheLoai) :
                new ObjectParameter("MaTheLoai", typeof(int));
    
            var tieuDeParameter = tieuDe != null ?
                new ObjectParameter("TieuDe", tieuDe) :
                new ObjectParameter("TieuDe", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<KhoaHoc>("SearchKhoaHocTheoTheLoai", mergeOption, maTheLoaiParameter, tieuDeParameter);
        }
    
        public virtual ObjectResult<string> SearchTenKhoahoc(string tenKhoaHoc, string tenTheLoai)
        {
            var tenKhoaHocParameter = tenKhoaHoc != null ?
                new ObjectParameter("TenKhoaHoc", tenKhoaHoc) :
                new ObjectParameter("TenKhoaHoc", typeof(string));
    
            var tenTheLoaiParameter = tenTheLoai != null ?
                new ObjectParameter("TenTheLoai", tenTheLoai) :
                new ObjectParameter("TenTheLoai", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SearchTenKhoahoc", tenKhoaHocParameter, tenTheLoaiParameter);
        }
    
        public virtual ObjectResult<string> SearchTenKhoahocNangCao(string tenKhoaHoc, string tenTheLoai)
        {
            var tenKhoaHocParameter = tenKhoaHoc != null ?
                new ObjectParameter("TenKhoaHoc", tenKhoaHoc) :
                new ObjectParameter("TenKhoaHoc", typeof(string));
    
            var tenTheLoaiParameter = tenTheLoai != null ?
                new ObjectParameter("TenTheLoai", tenTheLoai) :
                new ObjectParameter("TenTheLoai", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SearchTenKhoahocNangCao", tenKhoaHocParameter, tenTheLoaiParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<Nullable<decimal>> TinhTongTienMua(Nullable<int> maKhoaHoc)
        {
            var maKhoaHocParameter = maKhoaHoc.HasValue ?
                new ObjectParameter("MaKhoaHoc", maKhoaHoc) :
                new ObjectParameter("MaKhoaHoc", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("TinhTongTienMua", maKhoaHocParameter);
        }
    
        public virtual int BackUpDataBase(string path)
        {
            var pathParameter = path != null ?
                new ObjectParameter("Path", path) :
                new ObjectParameter("Path", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BackUpDataBase", pathParameter);
        }
    }
}
