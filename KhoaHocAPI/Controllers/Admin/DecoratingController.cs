using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers.Admin
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DecoratingController : ApiController
    {
        TrangTriDAO db = new TrangTriDAO();
        [HttpGet]
        public HttpResponseMessage GetByParentId(int MaLoaiTrangTri)
        {
            var result = db.LayTrangTriTheoLoai(MaLoaiTrangTri);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có");

            }
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPost]
        public HttpResponseMessage PostTrangTri(DecoratingVM model)
        {
            var result = db.ThemTrangTri(model.MaLoaiTrangTri, model.GiaTri);
            if (result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Kiểm tra lại mã loại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Kiểm tra lại giá trị và mã loại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm không thành công");
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpPut]
        public HttpResponseMessage PutTrangTri(DecoratingVM model)
        {
            var result = db.ThayDoiGiaTriTrangTri(model.MaTrangTri, model.GiaTri);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi không thành công");
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpDelete]
        public HttpResponseMessage DeleteTrangTri(int MaTrangTri)
        {
            var result = db.XoaTrangTri(MaTrangTri);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi không thành công");
            }
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
