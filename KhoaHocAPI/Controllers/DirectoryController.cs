using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DirectoryController : ApiController
    {
        private CategoryDAO db = new CategoryDAO();

        [HttpPost]
        public HttpResponseMessage PostDanhMuc(DanhMucVM model)
        {
            var result = db.TaoDanhMuc(model.TenDanhMuc, model.HinhAnh);
            if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Danh mục đã tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm danh mục không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
        }
        [HttpPut]
        public HttpResponseMessage PutDanhMuc(int MaDM, [FromBody] DanhMucVM model)
        {
            var result = db.SuaThongTinDanhMuc(MaDM, model.TenDanhMuc, model.HinhAnh);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Danh mục không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên danh mục bị trùng, hãy đổi tên khác");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi thông tin danh mục thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteDanhMuc(int MaDM)
        {
            var result = db.XoaDanhMuc(MaDM);
            if (result == Common.AllEnum.KetQuaTraVe.KhongDuocPhep)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Danh mục có chứa thể loại, không xóa");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Danh mục không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa danh mục không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpPatch]
        public HttpResponseMessage PatchTrangThaiDanhMuc([FromBody] List<int> lstMa, bool HienThi)
        {
            var result = db.DoiTrangThaiDanhMuc(lstMa, HienThi);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Danh mục không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đổi trạng thái danh mục không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}