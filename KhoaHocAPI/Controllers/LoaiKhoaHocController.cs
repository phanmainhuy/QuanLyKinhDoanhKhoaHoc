using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KhoaHocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoaiKhoaHocController : ApiController
    {
        CategoryDAO db = new CategoryDAO();
        [HttpPost]
        public HttpResponseMessage PostTheLoai(TheLoaiVM model)
        {
            var result = db.ThemTheLoai(model.MaDM, model.TenTheLoai);
            if (result == Common.AllEnum.KetQuaTraVe.ThanhCong)
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Vui lòng kiểm tra lại danh mục khóa học");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đã tồn tại thể loại này");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm thể loại không thành công");
            }
        }
        [HttpPut]
        public HttpResponseMessage PutTheLoai(int MaTL, TheLoaiVM model)
        {
            var result = db.ThayDoiThongTinTheLoai(MaTL, model.TenTheLoai);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thể loại không tồn tại");
            }
            else if(result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi thông tin thể loại không thành công");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tên thể loại bị trùng, hãy chọn tên khác");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        public HttpResponseMessage DeleteTheLoai(int MaTL)
        {
            var result = db.XoaTheLoai(MaTL);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thể loại không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongDuocPhep)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Có khóa học mang thể loại này, không xóa");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa thể loại không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpPatch]
        public HttpResponseMessage PatchTrangThaiDanhMuc([FromBody] List<int> lstMa, bool HienThi)
        {
            var result = db.DoiTrangThaiTheLoai(lstMa, HienThi);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thể loại không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đổi trạng thái thể loại không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}
