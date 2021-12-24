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
    public class DanhGiaController : ApiController
    {
        DanhGiaDAO db = new DanhGiaDAO();
        public HttpResponseMessage GetById(int MaKhoaHoc)
        {
            var result = db.LayDanhGiaKhoaHocTheoMaKhoaHoc(MaKhoaHoc);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có dữ liệu");
            }
            else if (result.Count() == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.RatingMapper.MapListRating(result));
            }
        }
        public HttpResponseMessage GetById2(int MaND, int MaKhoaHoc)
        {
            var result = db.LayDanhGiaDaDanhGia(MaND, MaKhoaHoc);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.RatingMapper.MapRating(result));
            }
        }
        public HttpResponseMessage PostDanhGia(RatingVM model)
        {
            var result = db.ThemMoiDanhGia(model.MaND, model.MaKhoaHoc, model.NoiDung, model.Diem);
            
            if (result == Common.AllEnum.KetQuaTraVe.KhongDuocPhep)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bạn chưa mua khóa học, không được đánh giá, không công tâm");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.DaTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đánh giá đã tồn tại");

            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongHopLe)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Hãy kiểm tra lại điểm đã nhập");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm đánh giá không thành công");
            }
            else
            {
                var newModel = db.LayDanhGiaDaDanhGia(model.MaND, model.MaKhoaHoc);
                return Request.CreateResponse(HttpStatusCode.Created, Mapper.RatingMapper.MapRating(newModel));
            }
        }
        [HttpPut]
        public HttpResponseMessage PutDanhGia(RatingVM model)
        {
            var result = db.ThayDoiDanhGia(model.MaND, model.MaKhoaHoc, model.NoiDung, model.Diem);
            if (result == Common.AllEnum.KetQuaTraVe.KhongDuocPhep)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bạn chưa mua khóa học, không được đánh giá, không công tâm");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không thể sửa vì đánh giá không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongHopLe)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Hãy kiểm tra lại điểm đã nhập");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi thông tin đánh giá không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteDanhGia(int MaKhoaHoc, int MaND)
        {
            var result = db.XoaDanhGia(MaND, MaKhoaHoc);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Đánh giá không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa đánh giá không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

        }
    }
}
