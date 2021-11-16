using KhoaHocAPI.Mapper;
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
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
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
        public HttpResponseMessage Get(HttpRequestMessage request, int userId)
        {
            var item = db.LayNguoiDungTheoId(userId);
            if (item != null)
                return request.CreateResponse(HttpStatusCode.OK, UserMapper.MapUser(item));
            else
                return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
