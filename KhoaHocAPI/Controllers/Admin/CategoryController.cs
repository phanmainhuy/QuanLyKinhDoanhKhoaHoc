using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KhoaHocAPI.Controllers.Admin
{
    public class CategoryController : ApiController
    {
        private CategoryDAO db_DanhMuc = new CategoryDAO();
        [HttpPost]
        public HttpResponseMessage PostDanhMuc(string TenDanhMuc)
        {
            var result = db_DanhMuc.TaoDanhMuc(TenDanhMuc);
            if (result == Common.AllEnum.KetQuaTraVeDanhMuc.ThanhCong)
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else if (result == Common.AllEnum.KetQuaTraVeDanhMuc.DanhMucDaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Danh mục đã tồn tại");
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm danh mục không thành công");
        }

        [HttpPost]
        public HttpResponseMessage PostTheLoai(int MaDanhMuc, string TenTheLoai)
        {
            var result = db_DanhMuc.ThemTheLoai(MaDanhMuc, TenTheLoai);
            if (result == Common.AllEnum.KetQuaTraVeDanhMuc.ThanhCong)
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else if (result == Common.AllEnum.KetQuaTraVeDanhMuc.DanhMucKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Danh mục không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVeDanhMuc.TheLoaiDaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thể loại đã tồn tại");

            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm thể loại không thành công");
        }
    }
}
