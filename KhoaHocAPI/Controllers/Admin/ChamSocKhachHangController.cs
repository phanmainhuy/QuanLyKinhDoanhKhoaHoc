using KhoaHocAPI.Models;
using KhoaHocAPI.Models.Service;
using KhoaHocData.DAO;
using Newtonsoft.Json;
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
    public class ChamSocKhachHangController : ApiController
    {
        CSKHDAO db = new CSKHDAO();
        public HttpResponseMessage GetAll(PagingVM model, string SDT = "", string TenKH = "")
        {
            int total;
            var result = db.GetAllChamSocKhachHang(SDT, TenKH, model.page, model.pageSize, out total);
            if (result != null)
            {
                var lstCourseVM = Mapper.ServiceMapper.MapListCustomerService(result);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);

                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response;
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không lấy được dữ liệu");
        }
        
        [HttpPost]
        public HttpResponseMessage PostChamSocKhachHang(CustomerServiceVM model)
        {
            var result = db.ThemChamSocKhachHang(model.MaLoaiVanDe.Value, model.MaNV.Value, model.SDT, model.TenKhachHang, model.NoiDung);
            if (result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Loại vấn đề không tồn tại, vui lòng kiểm tra và thử lại");
            else if(result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thêm dữ liệu không thành công");
            else
                return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage PutChamSocKhachHang(CustomerServiceVM model)
        {
            var result = db.ThayDoiThongTinChamSocKhachHang(model.MaCSKH, model.MaLoaiVanDe.Value, model.SDT, model.TenKhachHang, model.NoiDung, model.CachXuLy);
            if(result == Common.AllEnum.KetQuaTraVe.KhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dữ liệu không tồn tại, vui lòng kiểm tra và thử lại");
            else if(result == Common.AllEnum.KetQuaTraVe.ChaKhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Loại vấn đề không tồn tại, vui lòng kiểm tra và thử lại");
            else if (result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Thay đổi dữ liệu không thành công");
            else
                return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpDelete]
        public HttpResponseMessage DeleteChamSocKhachHang(int MaCSKH)
        {
            var result = db.XoaChamSocKhachHang(MaCSKH);
            if(result== Common.AllEnum.KetQuaTraVe.KhongTonTai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dữ liệu không tồn tại, vui lòng kiểm tra và thử lại");
            else if(result == Common.AllEnum.KetQuaTraVe.ThatBai)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Xóa dữ liệu không thành công");
            else
                return Request.CreateResponse(HttpStatusCode.OK);


        }
    }
}
