using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KhoaHocAPI.Controllers
{
    public class ChuongController : ApiController
    {
        BaiHocDAO db_BaiHoc = new BaiHocDAO();

        [HttpGet]
        public HttpResponseMessage GetById(int MaKhoaHoc)
        {
            var result = db_BaiHoc.LayChuongTheoKhoaHoc(MaKhoaHoc);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi lấy dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UnitMapper.MapListUnit(result));
            }
        }

        [HttpPost]
        public HttpResponseMessage PostBaiHoc(ChuongVM model)
        {
            var result = db_BaiHoc.ThemChuong(model.MaKhoaHoc, model.TenChuong);
            if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi thêm chương");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Khóa học không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Chương đã tồn tại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpPut]
        public HttpResponseMessage PutChuong(ChuongVM model)
        {
            var result = db_BaiHoc.SuaThongTinChuong(model.MaChuong, model.TenChuong);
            if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi sửa thông tin chương");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "chương không tồn tại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteChuong(int pMaChuong)
        {
            var result = db_BaiHoc.XoaChuong(pMaChuong);
            if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi xóa chương");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "chương không tồn tại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}
