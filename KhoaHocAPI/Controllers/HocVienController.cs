using KhoaHocAPI.Mapper;
using KhoaHocAPI.Models;
using KhoaHocData.DAO;
using Newtonsoft.Json;
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
    public class HocVienController : ApiController
    {
        private readonly NguoiDungDAO db = new NguoiDungDAO();
        public HttpResponseMessage Get()
        {
            var result = ( Mapper.UserMapper.MapListUser(db.LayDanhSachHocVien()));
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không có dữ liệu");
            else
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpGet]
        public HttpResponseMessage GetPaging([FromUri]string searchString, [FromUri]PagingVM model)
        {
            int total;
            var item = db.LayDanhSachHocVienPaging(model.page, model.pageSize, out total, searchString);
            if (item != null)
            {
                var lstCourseVM = Mapper.UserMapper.MapListUser(item);
                var response = Request.CreateResponse(HttpStatusCode.OK, lstCourseVM);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "pagingheader");
                response.Content.Headers.Add("pagingheader", JsonConvert.SerializeObject(total));
                return response;
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
        }
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, int userId)
        {
            if (userId == -1)
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Người dùng không tồn tại");
            var item = db.LayNguoiDungTheoId(userId);
            if (item != null)
                return request.CreateResponse(HttpStatusCode.OK, UserMapper.MapUser(item));
            else
                return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
