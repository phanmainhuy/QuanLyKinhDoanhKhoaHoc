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
    public class LuongController : ApiController
    {
        SalaryDAO db = new SalaryDAO();
        public HttpResponseMessage GetById(int MaND)
        {
            var result = db.getLichSuLuong(MaND);
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.SalaryMapper.MapSalary(result));
            }
        }
        public HttpResponseMessage GetAllSystem()
        {
            var result = new NguoiDungDAO().LayHetHeThong();
            if (result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Gặp vấn đề khi lấy dữ liệu");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, Mapper.UserMapper.MapListUserSalary(result));
            }
        }
        public HttpResponseMessage PostLuong(SalaryHistoryItemVM model)
        {
            var result = db.ThemLuong(model.MaLuong, model.TienPhat, model.GhiChu);
            if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Kiểm tra lại người dùng này");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm không thành công");
            }

            else
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetThongKeLuong(DateTime date)
        {
            var result = new ThongKeClass().ThongKeLichSuLuong(date);
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
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        [HttpPut]
        public HttpResponseMessage PutLichSuLuong(SalaryHistoryItemVM model)
        {
            var result = db.ThayDoiLichSuLuong(model.MaLuong, model.NgayPhatLuong, model.TienPhat, model.GhiChu);
            if (result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi, lương không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không tồn tại");
            }
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không thành công");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
    }
}
