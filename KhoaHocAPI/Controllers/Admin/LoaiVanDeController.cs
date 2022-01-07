using KhoaHocAPI.Models.Other;
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
    public class LoaiVanDeController : ApiController
    {
        CSKHDAO db = new CSKHDAO();

        public HttpResponseMessage GetAll()
        {
            var result = db.LayToanBoLoaiVanDe();
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có dữ liệu, vui lòng thử lại sau");
            else
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.ServiceMapper.MapListProblem(result));
        }
        [HttpPost]
        public HttpResponseMessage PostLoaiVanDe(string TenLoaiVanDe)
        {
            var result = db.ThemLoaiVanDe(TenLoaiVanDe);
            if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên loại vấn đề bị trùng, hãy chọn tên khác");
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm loại vấn đề không thành công, hãy thử lại sau");
            else
                return Request.CreateResponse(HttpStatusCode.Created);
        }
        [HttpPut]
        public HttpResponseMessage PutLoaiVanDe(LoaiVanDeVM model)
        {
            var result = db.ThayDoiTenLoaiVanDe(model.MaLoaiVanDe, model.TenLoaiVanDe);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Loại vấn đề này không tồn tại, vui lòng thử lại");
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi tên loại vấn đề không thành công, hãy thử lại sau");
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpDelete]
        public HttpResponseMessage DeleteLoaiVanDe(int MaLoaiVanDe)
        {
            var result = db.XoaLoaiVanDe(MaLoaiVanDe);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Loại vấn đề này không tồn tại, vui lòng thử lại");
            else if (result == Common.AllEnum.KetQuaTraVe.KhongDuocPhep)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vấn đề đang được sử dụng, không thể xóa");
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa dữ liệu không thành công");
            else 
                return Request.CreateResponse(HttpStatusCode.OK);

        }

    }
}
