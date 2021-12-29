using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using KhoaHocData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NguoiDungController : ApiController
    {
        private readonly NguoiDungDAO ndDAO = new NguoiDungDAO();
        // GET: NguoiDung
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            var result =  Mapper.UserMapper.MapListUser( ndDAO.LayHetNguoiDung());
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, result);
        }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetHocVien(int pMaNhomNguoiDung)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, Mapper.UserMapper.MapListUser(this.ndDAO.LayDanhSachTheoMaNhom(pMaNhomNguoiDung)));
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GetHeThong")]
        public HttpResponseMessage GetHeThong()
        {
            var result = Mapper.UserMapper.MapListUser(ndDAO.LayHetHeThong());
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, result);
        }








        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post(HttpRequestMessage request, NguoiDung model)
        {
            var result =  this.ndDAO.DangKy(model);
            if(result == Common.AllEnum.RegisterResult.ThanhCong)
                return request.CreateResponse(System.Net.HttpStatusCode.Created);
            else
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        }
        
        [System.Web.Http.HttpPut]
        public HttpResponseMessage ThayDoiThongTinNguoiDung(UserViewModel model)
        {
            var result = ndDAO.ThayDoiThongTinNguoiDung(
                model.UserId,
                model.UserName,
                model.GroupID,
                model.Name,
                model.CMND,
                model.HinhAnh,
                model.Number,
                model.Email,
                model.DoB,
                model.Address,
                model.Salary,
                model.Gender
                );
            if (result
                 == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Người dùng không tồn tại");
            }
            else if(result == Common.AllEnum.KetQuaTraVe.DuLieuDaTonTai1)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Email đã tồn tại");
            }
            else if(result == Common.AllEnum.KetQuaTraVe.DuLieuDaTonTai2)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "SĐT đã tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Thay đổi thông tin không thành công");
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
        }
        [System.Web.Http.HttpPatch]
        public HttpResponseMessage ThayDoiTrangThaiNguoiDung([FromBody]List<int> MaND, bool TrangThai)
        {
            var result = ndDAO.ThayDoiTrangThaiNguoiDung(MaND, TrangThai);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Người dùng không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Không thành công");
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
        }

        [System.Web.Http.HttpPatch]
        [System.Web.Http.Route("api/NguoiDung/DoiAvatar")]
        public HttpResponseMessage ThayDoiHinhAnhNguoiDung(int MaND, string HinhAnh)
        {
            var result = ndDAO.ThayDoiAnhDaiDienNguoiDung(MaND, HinhAnh);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Người dùng không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Không thành công");
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
        }

        [System.Web.Http.HttpDelete]
        public HttpResponseMessage XoaNhanVien(int MaND)
        {
            var result = ndDAO.XoaNguoiDung(MaND);
            if (result == Common.AllEnum.KetQuaTraVe.ThanhCong)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Xóa người dùng thất bại");
            }
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/nguoidung/xoanhieunhanvien")]
        public HttpResponseMessage XoaNhieuNhanVien([FromBody]List<int> lstNguoiDung)
        {
            var result = ndDAO.XoaNhieuNguoiDung(lstNguoiDung);
            if (result == Common.AllEnum.KetQuaTraVe.ThanhCong)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Xóa người dùng thất bại");
            }
        }
        
    }
}