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
    public class BaiHocController : ApiController
    {
        BaiHocDAO db_BaiHoc = new BaiHocDAO();


        [HttpGet]
        public HttpResponseMessage Get()
        {
            var result = db_BaiHoc.LayToanBoBaiHoc();
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi lấy dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UnitMapper.MapListLesson(result));
            }
        }
        [HttpGet]
        public HttpResponseMessage GetById(int MaBaiHoc)
        {
            var result = db_BaiHoc.LayBaiHoc(MaBaiHoc);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi lấy dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UnitMapper.MapLesson(result));
            }
        }
        [HttpGet]
        [Route("api/BaiHoc/GetByParentId")]
        public HttpResponseMessage GetByParentId(int MaChuong)
        {
            var result = db_BaiHoc.LayBaiHocTheoChuong(MaChuong);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi lấy dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UnitMapper.MapListLesson(result));
            }
        }

        [HttpPost]
        public HttpResponseMessage PostBaiHoc(BaiHocVM model)
        {
            var result = db_BaiHoc.ThemBaiHoc(model.MaChuong, model.TenBaiHoc, model.VideoName);
            if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi thêm bài học");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Chương không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {    
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bài học đã tồn tại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpPut]
        public HttpResponseMessage PutBaiHoc(BaiHocVM model)
        {
            var result = db_BaiHoc.SuaThongTinBaiHoc(model.MaBaiHoc, model.TenBaiHoc, model.VideoName);
            if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi sửa thông tin bài học");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bài học không tồn tại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteBaiHoc(int pMaBaiHoc)
        {
            var result = db_BaiHoc.XoaBaiHoc(pMaBaiHoc);
            if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi khi xóa bài học");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bài học không tồn tại");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpPatch]
        public HttpResponseMessage PatchTrangThaiBaiHoc([FromBody] List<int> lstModel, bool isHienThi)
        {
            var resust = db_BaiHoc.ThayDoiTrangThaiBaiHoc(lstModel, isHienThi);
            if (resust == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}
